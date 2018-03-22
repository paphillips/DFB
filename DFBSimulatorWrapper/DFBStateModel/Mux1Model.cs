using DFB_v1_40.Asm;
using DFBSimulatorWrapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFBSimulatorWrapper.DFBStateModel
{
	/// <summary>
	/// Mux 1 connects Mux0 or the Shifter to Mux2 and DataRam
	/// </summary>
	public class Mux1Model : DFBModelBase
	{
		#region Private Members

		private const int VALUE_WIDTH = 6;

		#endregion
		#region Public Members

		public static int PIPELINE_DELAY => 1;

		#endregion	
		#region Constructor

		/// <summary>
		/// Create a new instance of this device for the given cycle
		/// </summary>
		/// <param name="bankID">BankID of the device</param>
		/// <param name="cycle">Zero-based program cycle number</param>
		public Mux1Model(Bank bankID, int cycle) : base(bankID, cycle, VALUE_WIDTH, 0, PIPELINE_DELAY)
		{
			// name
			name = bankID == Bank.Bank_A
				? DevicePort.Mux_1A.ToString()
				: DevicePort.Mux_1B.ToString();

			// instructions
			instructions.Add(InstrForCycle(bankID, cycle));
			instructions.Add(InstrForCycle(bankID, cycle - 1));

			// connectionInputs
			connectionInputs = Connections(bankID, cycle);

			// output
			output = OutputCalc(bankID, cycle);
		}

		#endregion
		#region Methods

		/// <summary>
		/// Output value of the device for the given cycle
		/// </summary>
		/// <param name="bankID">BankID of the device</param>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		public static LabeledValue<long?> OutputCalc(Bank bankID, int cycle)
		{

			var label = "Out:";

			var field = bankID == DFBStateModel.Bank.Bank_A
						? "a1mux"
						: "b1mux";
			var val = DFBState.GetCySimulatorPrivateField<long?>(cycle, field);

			if (val != null && val.HasValue)
			{
				var r = new LabeledValue<long?>(label);
				r.Value = val.Value;
				r.FormattedValue = r.Value.HasValue ? FormatValue(VALUE_WIDTH, r.Value) : "";
				return r;
			}
			else
			{
				return NullLabeledValue<long?>(label);
			}
		}

		/// <summary>
		/// Input connections to device
		/// </summary>
		/// <param name="bankID">BankID of the device</param>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static List<Connection> Connections(Bank bankID, int cycle)
		{
			var conns = new List<Connection>();

			DevicePort mux0PortOut = DevicePort.Mux_0;
			DevicePort shifterPortOut = DevicePort.Shifter;
			DevicePort mux1Mux0PortIn = DevicePort.Default;
			DevicePort mux1ShifterPortIn = DevicePort.Default;

			PortStatus mux0PortOut_Stat;
			PortStatus shifterPortOut_Stat;
			PortStatus mux1Mux0PortIn_Stat;
			PortStatus mux1ShifterPortIn_Stat;

			InputsUsed(bankID, cycle,
				out mux0PortOut_Stat,
				out shifterPortOut_Stat,
				out mux1Mux0PortIn_Stat,
				out mux1ShifterPortIn_Stat);

			var mux0_label = Mux0Model.OutputCalc(cycle).FormattedValue;
			var shifter_label = ShifterModel.OutputCalc(cycle).FormattedValue;

			switch (bankID)
			{
				case Bank.Bank_A:
					mux1Mux0PortIn = DevicePort.Mux_1A;
					mux1ShifterPortIn = DevicePort.Mux_1A;
					break;

				case Bank.Bank_B:
					mux1Mux0PortIn = DevicePort.Mux_1B;
					mux1ShifterPortIn = DevicePort.Mux_1B;
					break;
			}

			conns.Add(new Connection(
				BusType.Data,
				mux0PortOut,
				mux0PortOut_Stat,
				mux0_label,
				null,
				mux1Mux0PortIn,
				mux1Mux0PortIn_Stat));

			conns.Add(new Connection(
				BusType.Data,
				shifterPortOut,
				shifterPortOut_Stat,
				shifter_label,
				null,
				mux1ShifterPortIn,
				mux1ShifterPortIn_Stat));

			return conns;
		}

		/// <summary>
		/// Inputs used by the device in the active instruction
		/// </summary>
		/// <param name="bankID">BankID of the device</param>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <param name="mux0PortOut_Stat">Port status</param>
		/// <param name="shifterPortOut_Stat">Port status</param>
		/// <param name="mux1Mux0PortIn_Stat">Port status</param>
		/// <param name="mux1ShifterPortIn_Stat">Port status</param>
		private static void InputsUsed(
			Bank bankID,
			int cycle,
			out PortStatus mux0PortOut_Stat,
			out PortStatus shifterPortOut_Stat,
			out PortStatus mux1Mux0PortIn_Stat,
			out PortStatus mux1ShifterPortIn_Stat)
		{
			mux0PortOut_Stat = PortStatus.Inactive;
			shifterPortOut_Stat = PortStatus.Inactive;
			mux1Mux0PortIn_Stat = PortStatus.Inactive;
			mux1ShifterPortIn_Stat = PortStatus.Inactive;

			// source port
			var instrWord = CodeStoreModel.Instruction(cycle - PIPELINE_DELAY);
			if (instrWord != null)
			{
				var instr = bankID == Bank.Bank_A
					? instrWord.MuxA
					: instrWord.MuxB;

				switch (instr)
				{
					case "ba":  // Bus to ALU
					case "bm":  // Bus to MAC, MAC to ALU
					case "brm": // Bus to RAM, to MAC
					case "bra": // Bus to RAM, RAM to ALU
						mux0PortOut_Stat = PortStatus.Active;
						mux1Mux0PortIn_Stat = PortStatus.Active;
						break;

					case "sa":  // Shifter to ALU
					case "sm":  // Shifter to MAC, MAC to ALU
					case "sra": // Shifter to RAM, RAM to ALU
					case "srm": // Shifter to RAM to MAC
						shifterPortOut_Stat = PortStatus.Active;
						mux1ShifterPortIn_Stat = PortStatus.Active;
						break;
				}
			}
		}

		/// <summary>
		/// Instruction for the cycle
		/// Returns null if the instruction doesn't apply to this device
		/// </summary>
		/// <param name="bankID">BankID of the device</param>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static string InstrForCycle(Bank bankID, int cycle)
		{
			if (cycle < 0) { return ""; }

			// "ba":  // bus to alu
			// "bra": // bus to ram, ram to alu
			// "bm":  // bus to mac, mac to alu
			// "brm": // bus to ram, to mac
			var instrWord = CodeStoreModel.Instruction(cycle);

			var muxInstr = bankID == Bank.Bank_A
				? CodeStoreModel.Instruction(cycle).MuxA
				: CodeStoreModel.Instruction(cycle).MuxB;

			return muxInstr;
		}

		#endregion
	}
}
