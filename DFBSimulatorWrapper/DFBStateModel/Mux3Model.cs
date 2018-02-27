using DFB_v1_40.Asm;
using DFBSimulatorWrapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFBSimulatorWrapper.DFBStateModel
{
	/// <summary>
	/// Mux3 connects the MAC and Mux2 to the ALU
	/// </summary>
	public class Mux3Model : DFBModelBase
	{
		#region Nested Structures

		public struct PipelineItem
		{
			public LabeledValue<long?> Input;
			public LabeledValue<DevicePort?> InputSource;
		}

		#endregion
		#region Private Members

		private const int VALUE_WIDTH = 6;
		private const int PIPELINE_DELAY = 1;
		private List<PipelineItem> pipelineItems;

		#endregion
		#region Public Members

		/// <summary>
		/// Values
		/// </summary>
		public List<PipelineItem> PipelineItems
		{
			get
			{
				return pipelineItems;
			}
		}

		#endregion
		#region Constructor

		/// <summary>
		/// Create a new instance of this device for the given cycle
		/// </summary>
		/// <param name="bankID">BankID of the device</param>
		/// <param name="cycle">Zero-based program cycle number</param>
		public Mux3Model(Bank bankID, int cycle) : base(bankID, cycle, VALUE_WIDTH, 0, PIPELINE_DELAY)
		{
			// name
			name = bankID == Bank.Bank_A
				? DevicePort.Mux_3A.ToString()
				: DevicePort.Mux_3B.ToString();

			// instructions
			instructions.Add(InstrForCycle(bankID, cycle));
			instructions.Add(InstrForCycle(bankID, cycle - 1));

			// connectionInputs
			connectionInputs = Connections(bankID, cycle);

			// output
			output = OutputCalc(bankID, cycle);

			// pipelineItems
			pipelineItems = new List<PipelineItem>();
			for (int i = 0; i < instructions.Count; i++)
			{
				var instr = instructions[i];
				DevicePort activeInput = DevicePort.Default;
				long? activeValue = null;

				if (i == 1)
				{
					switch (instr)
					{
						case "ba":  // Bus to ALU
						case "sa":  // Shifter to ALU
						case "bra": // Bus to RAM, RAM to ALU
						case "sra": // Shifter to RAM, RAM to ALU
							activeInput = bankID == Bank.Bank_A
								? DevicePort.Mux_2A
								: DevicePort.Mux_2B;
							activeValue = Mux2Model.OutputCalc(bankID, cycle).Value;
							break;

						case "brm": // Bus to RAM, to MAC
						case "srm": // Shifter to RAM to MAC
							activeInput = DevicePort.MAC;
							activeValue = MACModel.OutputCalc(cycle).Value;
							break;

						default:
							break;
					}
				}

				var pipelineItem = new PipelineItem();

				var lvValue = new LabeledValue<long?>("In:");
				lvValue.Value = activeValue.HasValue ? activeValue.Value : (long?)null;
				lvValue.FormattedValue = FormatValue(VALUE_WIDTH, lvValue.Value);
				pipelineItem.Input = lvValue;

				var lvValueSrc = new LabeledValue<DevicePort?>("Src:");
				lvValueSrc.Value = activeInput == DevicePort.Default ? (DevicePort?)null : activeInput;
				lvValueSrc.FormattedValue = lvValueSrc.Value.ToString();
				pipelineItem.InputSource = lvValueSrc;

				PipelineItems.Add(pipelineItem);
			}
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

			var field = bankID == Bank.Bank_A
						? "a3mux"
						: "b3mux";
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

			DevicePort mux2PortOut = DevicePort.Default;
			DevicePort macPortOut = DevicePort.Default;
			DevicePort mux3Mux2PortIn = DevicePort.Default;
			DevicePort mux2MacRamPortIn = DevicePort.Default;

			PortStatus mux2PortOut_Stat;
			PortStatus macPortOut_Stat;
			PortStatus mux3Mux2PortIn_Stat;
			PortStatus mux2MacRamPortIn_Stat;

			InputsUsed(bankID, cycle,
				out mux2PortOut_Stat,
				out macPortOut_Stat,
				out mux3Mux2PortIn_Stat,
				out mux2MacRamPortIn_Stat);

			var mux2_label = Mux2Model.OutputCalc(bankID, cycle).FormattedValue;
			var mac_label = MACModel.OutputCalc(cycle).FormattedValue;

			switch (bankID)
			{
				case Bank.Bank_A:
					mux2PortOut = DevicePort.Mux_2A;
					macPortOut = DevicePort.MAC;
					mux3Mux2PortIn = DevicePort.Mux_3A;
					mux2MacRamPortIn = DevicePort.Mux_3A;
					break;

				case Bank.Bank_B:
					mux2PortOut = DevicePort.Mux_2B;
					macPortOut = DevicePort.MAC;
					mux3Mux2PortIn = DevicePort.Mux_3B;
					mux2MacRamPortIn = DevicePort.Mux_3B;
					break;

				default:
					break;
			}

			conns.Add(new Connection(
				BusType.Data,
				mux2PortOut,
				mux2PortOut_Stat,
				mux2_label,
				null,
				mux3Mux2PortIn,
				mux3Mux2PortIn_Stat));

			conns.Add(new Connection(
				BusType.Data,
				macPortOut,
				macPortOut_Stat,
				mac_label,
				null,
				mux2MacRamPortIn,
				mux2MacRamPortIn_Stat));

			return conns;
		}

		/// <summary>
		/// Inputs used by the device in the active instruction
		/// </summary>
		/// <param name="bankID">BankID of the device</param>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <param name="mux2PortOut_Stat">Port status</param>
		/// <param name="macPortOut_Stat">Port status</param>
		/// <param name="mux3Mux2PortIn_Stat">Port status</param>
		/// <param name="mux2MacRamPortIn_Stat">Port status</param>
		private static void InputsUsed(
			Bank bankID,
			int cycle,
			out PortStatus mux2PortOut_Stat,
			out PortStatus macPortOut_Stat,
			out PortStatus mux3Mux2PortIn_Stat,
			out PortStatus mux2MacRamPortIn_Stat)
		{
			mux2PortOut_Stat = PortStatus.Inactive;
			macPortOut_Stat = PortStatus.Inactive;
			mux3Mux2PortIn_Stat = PortStatus.Inactive;
			mux2MacRamPortIn_Stat = PortStatus.Inactive;

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
					case "sa":  // Shifter to ALU
					case "bra": // Bus to RAM, RAM to ALU
					case "sra": // Shifter to RAM, RAM to ALU
						mux2PortOut_Stat = PortStatus.Active;
						mux3Mux2PortIn_Stat = PortStatus.Active;
						break;

					case "bm":  // Bus to MAC, MAC to ALU
					case "sm":  // Shifter to MAC, MAC to ALU
					case "brm": // Bus to RAM, to MAC
					case "srm": // Shifter to RAM to MAC
						macPortOut_Stat = PortStatus.Active;
						mux2MacRamPortIn_Stat = PortStatus.Active;
						break;

					default:
						break;
				}
			}
		}

		/// <summary>
		/// Instruction for the cycle
		/// Returns null if the instruction doesn't apply to this device
		/// </summary>
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
