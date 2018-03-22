using DFB_v1_40.Asm;
using DFBSimulatorWrapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFBSimulatorWrapper.DFBStateModel
{
	/// <summary>
	/// Mux2 connects Mux 1 and DataRam to the MAC and Mux 3
	/// </summary>
	public class Mux2Model : DFBModelBase
	{
		#region Private Members

		private const int VALUE_WIDTH = 6;
		private LabeledValue<long?> input1;
		private LabeledValue<DevicePort?> input1src;
		private LabeledValue<long?> input0;
		private LabeledValue<DevicePort?> input0src;

		#endregion
		#region Public Members

		public static int PIPELINE_DELAY => 1;

		/// <summary>
		/// Input value latched at current cycle [now]
		/// </summary>
		public LabeledValue<long?> Input1
		{
			get
			{
				return input1;
			}
		}
		
		/// <summary>
		/// Input source latched at current cycle [now]
		/// </summary>
		public LabeledValue<DevicePort?> Input1Src
		{
			get
			{
				return input1src;
			}
		}
				
		/// <summary>
		/// Input value latched at [t-1]
		/// </summary>
		public LabeledValue<long?> Input0
		{
			get
			{
				return input0;
			}
		}
		
		/// <summary>
		/// Input source latched at [t-1]
		/// </summary>
		public LabeledValue<DevicePort?> Input0Src
		{
			get
			{
				return input0src;
			}
		}
		
		#endregion
		#region Constructor

		/// <summary>
		/// Create a new instance of this device for the given cycle
		/// </summary>
		/// <param name="bankID">BankID of the device</param>
		/// <param name="cycle">Zero-based program cycle number</param>
		public Mux2Model(Bank bankID, int cycle) : base(bankID, cycle, VALUE_WIDTH, 0, PIPELINE_DELAY)
		{
			// name
			name = bankID == Bank.Bank_A
				? DevicePort.Mux_2A.ToString()
				: DevicePort.Mux_2B.ToString();

			// instructions
			instructions.Add(InstrForCycle(bankID, cycle));
			instructions.Add(InstrForCycle(bankID, cycle - 1));

			// connectionInputs
			connectionInputs = Connections(bankID, cycle);

			// output
			output = OutputCalc(bankID, cycle);

			// inputs
			InputCalc(bankID, cycle,
				out input0,
				out input0src,
				out input1,
				out input1src);
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
						? "a2mux"
						: "b2mux";
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
		/// Input to the device for the given cycle
		/// </summary>
		/// <param name="bankID">BankID of the device</param>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <param name="input0">First input</param>
		/// <param name="input0src">First input source</param>
		/// <param name="input1">Second input</param>
		/// <param name="input1src">Second input source</param>
		private static void InputCalc(Bank bankID, int cycle,
			out LabeledValue<long?> input0,
			out LabeledValue<DevicePort?> input0src,
			out LabeledValue<long?> input1,
			out LabeledValue<DevicePort?> input1src)
		{
			var valLabel = "In:";
			var srcLabel = "Src:";

			input0 = new LabeledValue<long?>(valLabel);
			input1 = new LabeledValue<long?>(valLabel);
			input0src = new LabeledValue<DevicePort?>(srcLabel);
			input1src = new LabeledValue<DevicePort?>(srcLabel);

			// Input latching occurs as follows (variant pipeline):
			// Pipeline					[t-1]		[now]
			// Cycle					cycle		cycle-1
			// dmux(_r_, _r_)			->ram in	output->
			// dmux(__, __)							->mux1in & output->

			var ramSource = bankID == Bank.Bank_A
						? DevicePort.DataRam_A
						: DevicePort.DataRam_B;

			var mux1Source = bankID == Bank.Bank_A
						? DevicePort.Mux_1A
						: DevicePort.Mux_1B;

			// Get [now] / cycle-1 input values
			var instr1 = InstrForCycle(bankID, cycle - 1);
			switch (instr1)
			{
				// These inputs latched from data ram in [t-1]
				case "bra":
				case "sra":
				case "brm":
				case "srm":
					input1.Value = DataRamModel.OutputCalc(bankID, cycle - 1).Value;
					input1.FormattedValue = FormatValue(VALUE_WIDTH, input1.Value);
					input1src.Value = ramSource;
					input1src.FormattedValue = input1src.Value.ToString();
					break;

				// These inputs latch from mux1 [now]
				case "ba":
				case "sa":
				case "bm":
				case "sm":
					input1.Value = Mux1Model.OutputCalc(bankID, cycle).Value;
					input1.FormattedValue = FormatValue(VALUE_WIDTH, input1.Value);
					input1src.Value = mux1Source;
					input1src.FormattedValue = input1src.Value.ToString();
					break;
			}

			// Get [t-1] / cycle input values
			var instr0 = InstrForCycle(bankID, cycle);
			switch (instr0)
			{
				case "bra":
				case "sra":
				case "brm":
				case "srm":
					input0.Value = DataRamModel.OutputCalc(bankID, cycle).Value;
					input0.FormattedValue = FormatValue(VALUE_WIDTH, input0.Value);
					input0src.Value = ramSource;
					input0src.FormattedValue = input0src.Value.ToString();
					break;

				// no op:
				case "ba":
				case "sa":
				case "bm":
				case "sm":
				default:
					break;
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

			DevicePort mux1PortOut = DevicePort.Default;
			DevicePort dataRamPortOut = DevicePort.Default;
			DevicePort mux2Mux1PortIn = DevicePort.Default;
			DevicePort mux2DataRamPortIn = DevicePort.Default;

			PortStatus mux1PortOut_Stat;
			PortStatus dataRamPortOut_Stat;
			PortStatus mux2Mux1PortIn_Stat;
			PortStatus mux2DataRamPortIn_Stat;

			var mux1_label = Mux1Model.OutputCalc(bankID, cycle).FormattedValue;
			var dataRam_label = DataRamModel.OutputCalc(bankID, cycle).FormattedValue;

			InputsUsed(bankID, cycle,
				out mux1PortOut_Stat,
				out dataRamPortOut_Stat,
				out mux2Mux1PortIn_Stat,
				out mux2DataRamPortIn_Stat);

			switch (bankID)
			{
				case Bank.Bank_A:
					mux1PortOut = DevicePort.Mux_1A;
					dataRamPortOut = DevicePort.DataRam_A;
					mux2Mux1PortIn = DevicePort.Mux_2A;
					mux2DataRamPortIn = DevicePort.Mux_2A;
					break;

				case Bank.Bank_B:
					mux1PortOut = DevicePort.Mux_1B;
					dataRamPortOut = DevicePort.DataRam_B;
					mux2Mux1PortIn = DevicePort.Mux_2B;
					mux2DataRamPortIn = DevicePort.Mux_2B;
					break;

				default:
					break;
			}

			conns.Add(new Connection(
				BusType.Data,
				mux1PortOut,
				mux1PortOut_Stat,
				mux1_label,
				null,
				mux2Mux1PortIn,
				mux2Mux1PortIn_Stat));

			conns.Add(new Connection(
				BusType.Data,
				dataRamPortOut,
				dataRamPortOut_Stat,
				dataRam_label,
				null,
				mux2DataRamPortIn,
				mux2DataRamPortIn_Stat));

			return conns;
		}

		/// <summary>
		/// Inputs used by the device in the active instruction
		/// </summary>
		/// <param name="bankID">BankID of the device</param>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <param name="mux1PortOut_Stat">Port status</param>
		/// <param name="dataRamPortOut_Stat">Port status</param>
		/// <param name="mux2Mux1PortIn_Stat">Port status</param>
		/// <param name="mux2DataRamPortIn_Stat">Port status</param>
		private static void InputsUsed(
			Bank bankID,
			int cycle,
			out PortStatus mux1PortOut_Stat,
			out PortStatus dataRamPortOut_Stat,
			out PortStatus mux2Mux1PortIn_Stat,
			out PortStatus mux2DataRamPortIn_Stat)
		{
			mux1PortOut_Stat = PortStatus.Inactive;
			dataRamPortOut_Stat = PortStatus.Inactive;
			mux2Mux1PortIn_Stat = PortStatus.Inactive;
			mux2DataRamPortIn_Stat = PortStatus.Inactive;

			// active instruction (pipeline delayed)
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
					case "bm":  // Bus to MAC, MAC to ALU
					case "sm":  // Shifter to MAC, MAC to ALU
						mux1PortOut_Stat = PortStatus.Active;
						mux2Mux1PortIn_Stat = PortStatus.Active;
						break;

					case "bra": // Bus to RAM, RAM to ALU
					case "sra": // Shifter to RAM, RAM to ALU
					case "brm": // Bus to RAM, to MAC
					case "srm": // Shifter to RAM to MAC
						dataRamPortOut_Stat = PortStatus.Active;
						mux2DataRamPortIn_Stat = PortStatus.Active;
						break;

					default:
						break;
				}
			}

			// current instruction
			instrWord = CodeStoreModel.Instruction(cycle);
			if (instrWord != null)
			{
				var instr = bankID == Bank.Bank_A
					? instrWord.MuxA
					: instrWord.MuxB;

				switch (instr)
				{
					// ignore - not latched in until [cycle - PIPELINE_DELAY]
					case "ba":  // Bus to ALU
					case "sa":  // Shifter to ALU
					case "bm":  // Bus to MAC, MAC to ALU
					case "sm":  // Shifter to MAC, MAC to ALU
						break;

					// latched in now
					case "bra": // Bus to RAM, RAM to ALU
					case "sra": // Shifter to RAM, RAM to ALU
					case "brm": // Bus to RAM, to MAC
					case "srm": // Shifter to RAM to MAC
						dataRamPortOut_Stat = PortStatus.Active;
						mux2DataRamPortIn_Stat = PortStatus.Active;
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
