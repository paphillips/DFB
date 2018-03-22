using System;
using System.Collections.Generic;

namespace DFBSimulatorWrapper.DFBStateModel
{
	/// <summary>
	/// Multiply and Accumulate Unit (MAC)
	/// </summary>
	public class MACModel : DFBModelBase
	{
		#region Nested Structures

		public struct PipelineItem
		{
			public LabeledValue<long?> A;
			public LabeledValue<long?> B;
			public LabeledValue<int?> Accumulator;
			public LabeledValue<long?> AluValue;
			public LabeledValue<string> OutputFormula;
		}

		#endregion
		#region Private Members
		
		private const int VALUE_WIDTH = 6;
		private List<PipelineItem> pipelineItems;
		protected string outputFormula;

		#endregion
		#region Public Members

		public static int PIPELINE_DELAY => 2;

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

		/// <summary>
		/// Formula indicating how the output was calculated for the current instruction
		/// </summary>
		public LabeledValue<string> OutputFormula
		{
			get
			{
				var r = new LabeledValue<string>("Equation:");
				r.Value = outputFormula;
				r.FormattedValue = outputFormula;
				return r;
			}
		}

		#endregion
		#region Constructor

		/// <summary>
		/// Create a new instance of this device for the given cycle
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		public MACModel(int cycle) : base(Bank.NotApplicable, cycle, VALUE_WIDTH, 0, PIPELINE_DELAY)
		{
			// name
			name = DevicePort.MAC.ToString();

			// instructions
			instructions.Add(ActiveInstr(cycle));       // [t-2]
			instructions.Add(ActiveInstr(cycle - 1));   // [t-1]
			instructions.Add(ActiveInstr(cycle - 2));   // [now]

			// connectionInputs
			connectionInputs = Connections(cycle);

			// output
			output = OutputCalc(cycle);

			// pipelineItems
			pipelineItems = new List<PipelineItem>();

			for (int i = 0; i < instructions.Count; i++)
			{
				var instr = instructions[i];
				long? aluValue = null;
				long? a = null;
				long? b = null;
				string outputFormula = null;
				int? accum = null;

				switch (instr)
				{
					case "loadalu":
						if (i > 0)
						{
							// Adds the previous ALU output (from the shifter) to the product and starts a new accumulation.
							aluValue = DFBState.GetCySimulatorPrivateField<long?>(cycle - i + 1, "aluout");
							a = DFBState.GetCySimulatorPrivateField<long?>(cycle - i + 1, "a2mux");
							b = DFBState.GetCySimulatorPrivateField<long?>(cycle - i + 1, "b2mux");
							if (i == PIPELINE_DELAY)
							{
								outputFormula = "a * b + [Prev ALU]";
							}
						}
						break;

					case "clra":
						if (i > 0)
						{
							// Clears the accumulator and stores the current product
							a = DFBState.GetCySimulatorPrivateField<long?>(cycle - i + 1, "a2mux");
							b = DFBState.GetCySimulatorPrivateField<long?>(cycle - i + 1, "b2mux");
							if (i == PIPELINE_DELAY)
							{
								accum = 0;
								outputFormula = "a * b";
							}
						}
						break;

					case "hold":
						if (i > 0)
						{
							// Holds the value in the accumulator from the previous cycle. No multiply
							if (i == PIPELINE_DELAY)
							{
								accum = Convert.ToInt32(OutputCalc(cycle - i+1).Value);
								outputFormula = "Accum";
							}
						}
						break;

					case "macc":
						if (i > 0)
						{
							// Multiplies the values on mux2 of side A and side B. 
							// Adds the product to the current value of the accumulator.
							a = DFBState.GetCySimulatorPrivateField<long?>(cycle - i + 1, "a2mux");
							b = DFBState.GetCySimulatorPrivateField<long?>(cycle - i + 1, "b2mux");
							if (i == PIPELINE_DELAY)
							{
								accum = Convert.ToInt32(OutputCalc(cycle - i + 1).Value);
								outputFormula = "a * b + Accum";
							}
						}
						break;
				}

				var pipelineItem = new PipelineItem();

				var lvAluValue = new LabeledValue<long?>("Prev ALU:");
				lvAluValue.Value = aluValue.HasValue ? aluValue.Value : (long?)null;
				lvAluValue.FormattedValue = FormatValue(VALUE_WIDTH, lvAluValue.Value);
				pipelineItem.AluValue = lvAluValue;

				var lvA = new LabeledValue<long?>("a:");
				lvA.Value = a.HasValue ? a.Value : (long?)null;
				lvA.FormattedValue = FormatValue(VALUE_WIDTH, lvA.Value);
				pipelineItem.A = lvA;

				var lvB = new LabeledValue<long?>("b:");
				lvB.Value = b.HasValue ? b.Value : (long?)null;
				lvB.FormattedValue = FormatValue(VALUE_WIDTH, lvB.Value);
				pipelineItem.B = lvB;

				var lvOutputFormula = new LabeledValue<string>("Formula:");
				lvOutputFormula.Value = outputFormula;
				lvOutputFormula.FormattedValue = outputFormula;
				pipelineItem.OutputFormula = lvOutputFormula;

				var lvAccum = new LabeledValue<int?>("Accum:");
				lvAccum.Value = accum.HasValue ? accum.Value : (int?)null;
				lvAccum.FormattedValue = FormatValue(VALUE_WIDTH, lvAccum.Value);
				pipelineItem.Accumulator = lvAccum;

				pipelineItems.Add(pipelineItem);
			}
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

			var macacc = DFBState.GetCySimulatorPrivateField<long?>(cycle, "MACacc");
			if (macacc != null)
			{
				int maclower = Convert.ToInt32(macacc >> 23);

				var r = new LabeledValue<long?>("Out:");
				r.Value = maclower;
				r.FormattedValue = r.Value.HasValue ? FormatValue(VALUE_WIDTH, r.Value) : "";
				return r;
			}
			else
			{
				return NullLabeledValue<long?>(label);
			}
		}

		/// <summary>
		/// Instruction for the cycle
		/// Returns null if the instruction doesn't apply to this device
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static string ActiveInstr(int cycle)
		{
			if (cycle < 0) { return ""; }

			var instr = CodeStoreModel.Instruction(cycle);
			if(instr.Mac == null)
			{
				return "";
			}
			else
			{
				return instr.Mac.Replace("holdmac", "hold");
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

			var instr = ActiveInstr(cycle - PIPELINE_DELAY);

			PortStatus mux2APortOut_Stat;
			PortStatus mux2BPortOut_Stat;
			PortStatus shifterPortOut_Stat;

			PortStatus macMux2APortIn_Stat;
			PortStatus macMux2BPortIn_Stat;
			PortStatus macShifterPortIn_Stat;

			InputsUsed(cycle,
				out mux2APortOut_Stat,
				out mux2BPortOut_Stat,
				out shifterPortOut_Stat,
				out macMux2APortIn_Stat,
				out macMux2BPortIn_Stat,
				out macShifterPortIn_Stat);

			var activeLabel_Mux_2A = Mux2Model.OutputCalc(Bank.Bank_A, cycle).FormattedValue;
			var activeLabel_Mux_2B = Mux2Model.OutputCalc(Bank.Bank_B, cycle).FormattedValue;
			var activeLabel_Shifter = ShifterModel.OutputCalc(cycle).FormattedValue;

			// Show inputs based on connection:
			conns.Add(new Connection(
				BusType.Data,
				DevicePort.Mux_2A,
				mux2APortOut_Stat,
				activeLabel_Mux_2A,
				null,
				DevicePort.MAC,
				macMux2APortIn_Stat));

			conns.Add(new Connection(
				BusType.Data,
				DevicePort.Mux_2B,
				mux2BPortOut_Stat,
				activeLabel_Mux_2B,
				null,
				DevicePort.MAC,
				macMux2BPortIn_Stat));

			conns.Add(new Connection(
				BusType.Data,
				DevicePort.Shifter,
				shifterPortOut_Stat,
				activeLabel_Shifter,
				null,
				DevicePort.MAC,
				macShifterPortIn_Stat));

			return conns;
		}

		/// <summary>
		/// Inputs used by the device in the active instruction
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <param name="mux2APortOut_Stat"></param>
		/// <param name="mux2BPortOut_Stat"></param>
		/// <param name="shifterPortOut_Stat"></param>
		/// <param name="macMux2APortIn_Stat"></param>
		/// <param name="macMux2BPortIn_Stat"></param>
		/// <param name="macShifterPortIn_Stat"></param>
		private static void InputsUsed(
			int cycle,
			out PortStatus mux2APortOut_Stat,
			out PortStatus mux2BPortOut_Stat,
			out PortStatus shifterPortOut_Stat,
			out PortStatus macMux2APortIn_Stat,
			out PortStatus macMux2BPortIn_Stat,
			out PortStatus macShifterPortIn_Stat)
		{
			// Mux 2 and shifter outputs are always active
			mux2APortOut_Stat = PortStatus.Active;
			mux2BPortOut_Stat = PortStatus.Active;
			shifterPortOut_Stat = PortStatus.Active;

			// Default
			macMux2APortIn_Stat = PortStatus.Inactive;
			macMux2BPortIn_Stat = PortStatus.Inactive;
			macShifterPortIn_Stat = PortStatus.Inactive;

			var instr = CodeStoreModel.Instruction(cycle - 1);
			if (instr == null) { return; }

			var instMAC = instr.Mac;

			switch (instMAC ?? "")
			{
				case "loadalu":
					// Adds the previous ALU output (from the shifter) to the product and 
					// starts a new accumulation.
					macMux2APortIn_Stat = PortStatus.Active;
					macMux2BPortIn_Stat = PortStatus.Active;
					macShifterPortIn_Stat = PortStatus.Active;
					break;

				case "clra":
					// Clears the accumulator and stores the current product
					macMux2APortIn_Stat = PortStatus.Active;
					macMux2BPortIn_Stat = PortStatus.Active;
					break;

				case "hold":
					// Holds the value in the accumulator from the previous cycle. 
					// No multiply
					break;

				case "macc":
					// Multiplies the values on mux2 of side A and side B. 
					// Adds the product to the current value of the accumulator.
					macMux2APortIn_Stat = PortStatus.Active;
					macMux2BPortIn_Stat = PortStatus.Active;
					break;
			}
		}

		#endregion
	}
}