using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFB_v1_40.Asm;
using DFBSimulatorWrapper.DFBStateModel;
using DFB_v1_40.Simulator;
using System.Diagnostics;
using DFBSimulatorWrapper;

namespace DFBSimulatorWrapper.DFBStateModel
{
	/// <summary>
	/// The shifter enables bitshifting on datapath output values
	/// </summary>
	public class ShifterModel : DFBModelBase
	{
		#region Private Members

		public const int VALUE_WIDTH = 6;
		public const int ADDR_WIDTH = 2;
		public const int PIPELINE_DELAY = 1;
		private LabeledValue<long?> input;

		#endregion
		#region Public Members

		/// <summary>
		/// Input value latched at current cycle [now] from instruction with 1 pipelined instr delay
		/// </summary>
		public LabeledValue<long?> Input
		{
			get
			{
				return input;
			}

		}

		#endregion
		#region Constructor

		/// <summary>
		/// Create a new instance of this device for the given cycle
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		public ShifterModel(int cycle) : base(Bank.NotApplicable, cycle, VALUE_WIDTH, ADDR_WIDTH, PIPELINE_DELAY)
		{
			// name
			name = DevicePort.Shifter.ToString();

			// instructions
			instructions.Add(ActiveInstr(cycle));
			instructions.Add(ActiveInstr(cycle - 1));

			// connectionInputs
			connectionInputs = Connections(cycle);

			// output
			output = OutputCalc(cycle);

			// input
			input = InputCalc(cycle);
		}

		#endregion
		#region Methods
		
		/// <summary>
		/// Output value of the device for the given cycle
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		public static LabeledValue<long?> OutputCalc(int cycle)
		{
			var label = "Out:";
			if (cycle - PIPELINE_DELAY < 0) { return NullLabeledValue<long?>(label); }
			
			var val = DFBState.GetCySimulatorPrivateField<long?>(cycle, "shift");
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
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static LabeledValue<long?> InputCalc(int cycle)
		{
			var label = "In:";

			var val = ALUModel.OutputCalc(cycle);
			if (val != null)
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
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static List<Connection> Connections(int cycle)
		{
			var conns = new List<Connection>();

			DevicePort aluPortOut = DevicePort.ALU;
			DevicePort shifterAluPortIn = DevicePort.Shifter;

			PortStatus aluPortOut_Stat;
			PortStatus shifterAluPortIn_Stat;

			InputsUsed(cycle,
				out aluPortOut_Stat,
				out shifterAluPortIn_Stat);

			var alu_label = ALUModel.OutputCalc(cycle).FormattedValue;

			conns.Add(new Connection(
				BusType.Data,
				aluPortOut,
				aluPortOut_Stat,
				alu_label,
				null,
				shifterAluPortIn,
				shifterAluPortIn_Stat));

			return conns;
		}

		/// <summary>
		/// Inputs used by the device in the active instruction
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <param name="aluPortOut_Stat">Status of port</param>
		/// <param name="shifterAluPortIn">Status of port</param>
		private static void InputsUsed(
			int cycle,
			out PortStatus aluPortOut_Stat,
			out PortStatus shifterAluPortIn)
		{
			aluPortOut_Stat = PortStatus.Inactive;
			shifterAluPortIn = PortStatus.Inactive;

			// alu
			var aluOutput = ALUModel.OutputCalc(cycle);
			if (aluOutput.Value.HasValue)
			{
				aluPortOut_Stat = PortStatus.Active;
				shifterAluPortIn = PortStatus.Active;
			}
		}
			
		/// <summary>
		/// Instruction text for the cycle
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static string ActiveInstr(int cycle)
		{
			if (cycle < 0) { return ""; }
			var instr = CodeStoreModel.Instruction(cycle);
			if (instr.ShiftOp != 0)
			{
				switch (instr.ShiftOp)
				{
					case 1:
						return "shift >> 1";
					case 2:
						return "shift >> 2";
					case 3:
						return "shift >> 3";
					case 4:
						return "shift >> 4";
					case 5:
						return "shift >> 8";
					case 6:
						return "shift << 1";
					case 7:
						return "shift << 2";
					default:
						return "unknown";
				}
			}
			else
			{
				return "";
			}
		}
		
		#endregion
	}
}
