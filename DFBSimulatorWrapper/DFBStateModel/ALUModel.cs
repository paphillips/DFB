using DFBSimulatorWrapper;
using System.Collections.Generic;

namespace DFBSimulatorWrapper.DFBStateModel
{
	/// <summary>
	/// The arithmetic logic unit (ALU) provides data control on the output end of the datapath
	/// </summary>
	public class ALUModel : DFBModelBase
	{
		#region Nested Structures

		public struct PipelineItem
		{
			public LabeledValue<long?> InputA;
			public LabeledValue<DevicePort?> InputASource;
			public LabeledValue<long?> InputB;
			public LabeledValue<DevicePort?> InputBSource;
			public LabeledValue<string> OutputFormula;
		}

		#endregion
		#region Private Members

		public const int VALUE_WIDTH = 6;
		public const int ADDR_WIDTH = 2;
		
		private List<PipelineItem> pipelineItems;

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

		#endregion
		#region Constructor

		/// <summary>
		/// Create a new instance of this device for the given cycle
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		public ALUModel(int cycle) : base(Bank.NotApplicable, cycle, VALUE_WIDTH, ADDR_WIDTH, PIPELINE_DELAY)
		{
			// name
			name = DevicePort.ALU.ToString();

			// instructions
			instructions.Add(Instr(cycle));
			instructions.Add(Instr(cycle - 1));
			var activeInstr = Instr(cycle - 2);
			instructions.Add(activeInstr);

			// pipelineItems
			pipelineItems = BuildPipelineItems(cycle, instructions);

			// connectionInputs
			connectionInputs = Connections(cycle, pipelineItems);

			// output
			output = OutputCalc(cycle);
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

			var aluout = DFBState.GetCySimulatorPrivateField<long?>(cycle, "aluout");
			if (aluout != null)
			{
				var r = new LabeledValue<long?>(label);
				r.Value = aluout;
				r.FormattedValue = r.Value.HasValue ? FormatValue(VALUE_WIDTH, (int)r.Value) : "";
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
		/// <param name="pipelineItems"></param>
		/// <returns></returns>
		private static List<Connection> Connections(int cycle, List<PipelineItem> pipelineItems)
		{
			var conns = new List<Connection>();

			var instr = Instr(cycle - PIPELINE_DELAY);

			PortStatus statMux3A_ALUIn;
			PortStatus statMux3B_ALUIn;
			PortStatus statRamA_ALUIn;
			PortStatus statRamB_ALUIn;

			statMux3A_ALUIn = pipelineItems[1].InputASource.Value == DevicePort.Mux_3A 
				? PortStatus.Active 
				: PortStatus.Inactive;

			statMux3B_ALUIn = pipelineItems[1].InputBSource.Value == DevicePort.Mux_3B
				? PortStatus.Active
				: PortStatus.Inactive;

			statRamA_ALUIn = pipelineItems[1].InputASource.Value == DevicePort.DataRam_A
				? PortStatus.Active
				: PortStatus.Inactive;

			statRamB_ALUIn = pipelineItems[1].InputBSource.Value == DevicePort.DataRam_B
				? PortStatus.Active
				: PortStatus.Inactive;

			var activeLabel_Mux_3A = Mux3Model.OutputCalc(Bank.Bank_A, cycle).FormattedValue;
			var activeLabel_Mux_3B = Mux3Model.OutputCalc(Bank.Bank_B, cycle).FormattedValue;
			var activeLabel_Ram_A = DataRamModel.OutputCalc(Bank.Bank_A, cycle).FormattedValue;
			var activeLabel_Ram_B = DataRamModel.OutputCalc(Bank.Bank_B, cycle).FormattedValue;

			// Show ALU inputs based on connection
			// non-banked
			conns.Add(new Connection(
				BusType.Data,
				DevicePort.Mux_3A,
				PortStatus.Active,
				activeLabel_Mux_3A,
				null,
				DevicePort.ALU,
				statMux3A_ALUIn));

			conns.Add(new Connection(
				BusType.Data,
				DevicePort.Mux_3B,
				PortStatus.Active,
				activeLabel_Mux_3B,
				null,
				DevicePort.ALU,
				statMux3B_ALUIn));

			conns.Add(new Connection(
				BusType.Data,
				DevicePort.DataRam_A,
				PortStatus.Active,
				activeLabel_Ram_A,
				null,
				DevicePort.ALU,
				statRamA_ALUIn));

			conns.Add(new Connection(
				BusType.Data,
				DevicePort.DataRam_B,
				PortStatus.Active,
				activeLabel_Ram_B,
				null,
				DevicePort.ALU,
				statRamB_ALUIn));

			return conns;
		}

		/// <summary>
		/// Instruction for the cycle
		/// Returns null if the instruction doesn't apply to this device
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static string Instr(int cycle)
		{
			if (cycle < 0) { return ""; }

			var instr = CodeStoreModel.Instruction(cycle);
			return instr.Alu;
		}

		/// <summary>
		/// Builds a list of PipelineItems
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <param name="instructions">Instructions for pipeline</param>
		/// <returns></returns>
		private static List<PipelineItem> BuildPipelineItems(int cycle, List<string> instructions)
		{
			var pipelineItems = new List<PipelineItem>();

			for (int i = 0; i < instructions.Count; i++)
			{
				var instr = instructions[i];
				long? inputA = null;
				long? inputB = null;
				DevicePort? inputASource = null;
				DevicePort? inputBSource = null;
				string outputFormula = null;

				if (i > 0)
				{
					switch (instr ?? "")
					{
						case "set0":
							outputFormula = "0";
							break;
						case "set1":
							outputFormula = "1";
							break;
						case "seta":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							outputFormula = "A";
							break;
						case "setb":
							inputB = Mux3Model.OutputCalc(Bank.Bank_B, cycle - i + 1).Value;
							inputBSource = DevicePort.Mux_3B;
							outputFormula = "B";
							break;
						case "nega":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							outputFormula = "-A";
							break;
						case "negb":
							inputB = Mux3Model.OutputCalc(Bank.Bank_B, cycle - i + 1).Value;
							inputBSource = DevicePort.Mux_3B;
							outputFormula = "-B";
							break;
						case "passrama":
							inputA = DataRamModel.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.DataRam_A;
							outputFormula = "Ram A";
							break;
						case "passramb":
							inputB = DataRamModel.OutputCalc(Bank.Bank_B, cycle - i + 1).Value;
							inputBSource = DevicePort.DataRam_B;
							outputFormula = "Ram B";
							break;
						case "add":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							inputB = Mux3Model.OutputCalc(Bank.Bank_B, cycle - i + 1).Value;
							inputBSource = DevicePort.Mux_3B;
							outputFormula = "A+B";
							break;
						case "tdeca":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							outputFormula = "A-1";
							break;
						case "suba":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							inputB = Mux3Model.OutputCalc(Bank.Bank_B, cycle - i + 1).Value;
							inputBSource = DevicePort.Mux_3B;
							outputFormula = "B-A";
							break;
						case "subb":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							inputB = Mux3Model.OutputCalc(Bank.Bank_B, cycle - i + 1).Value;
							inputBSource = DevicePort.Mux_3B;
							outputFormula = "A-B";
							break;
						case "absa":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							outputFormula = "abs(A)";
							break;
						case "absb":
							inputB = Mux3Model.OutputCalc(Bank.Bank_B, cycle - i + 1).Value;
							inputBSource = DevicePort.Mux_3B;
							outputFormula = "abs(B)";
							break;
						case "addabsa":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							inputB = Mux3Model.OutputCalc(Bank.Bank_B, cycle - i + 1).Value;
							inputBSource = DevicePort.Mux_3B;
							outputFormula = "abs(A)+B";
							break;
						case "addabsb":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							inputB = Mux3Model.OutputCalc(Bank.Bank_B, cycle - i + 1).Value;
							inputBSource = DevicePort.Mux_3B;
							outputFormula = "A+abs(B)";
							break;
						case "hold":
							outputFormula = "ALU Hold";
							break;
						case "englobals":
							outputFormula = "";
							break;
						case "ensatrnd":
							outputFormula = "";
							break;
						case "ensem":
							outputFormula = "";
							break;
						case "setsem":
							outputFormula = "";
							break;
						case "clearsem":
							outputFormula = "";
							break;
						case "tsuba":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							inputB = Mux3Model.OutputCalc(Bank.Bank_B, cycle - i + 1).Value;
							inputBSource = DevicePort.Mux_3B;
							outputFormula = "B-A thr";
							break;
						case "tsubb":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							inputB = Mux3Model.OutputCalc(Bank.Bank_B, cycle - i + 1).Value;
							inputBSource = DevicePort.Mux_3B;
							outputFormula = "A-B thr";
							break;
						case "taddabsa":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							inputB = Mux3Model.OutputCalc(Bank.Bank_B, cycle - i + 1).Value;
							inputBSource = DevicePort.Mux_3B;
							outputFormula = "A+abs(B) thr";
							break;
						case "taddabsb":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							inputB = Mux3Model.OutputCalc(Bank.Bank_B, cycle - i + 1).Value;
							inputBSource = DevicePort.Mux_3B;
							outputFormula = "A+abs(B) thr";
							break;
						case "sqlcmp":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							outputFormula = "A -> sqlcmp";
							break;
						case "sqlcnt":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							outputFormula = "A>>16 -> sqcval";
							break;
						case "sqa":
							inputA = Mux3Model.OutputCalc(Bank.Bank_A, cycle - i + 1).Value;
							inputASource = DevicePort.Mux_3A;
							outputFormula = "Squelch(A)";
							break;
						case "sqb":
							inputB = Mux3Model.OutputCalc(Bank.Bank_B, cycle - i + 1).Value;
							inputBSource = DevicePort.Mux_3B;
							outputFormula = "Squelch(B)";
							break;
					}
				}

				var pipelineItem = new PipelineItem();

				var lvA = new LabeledValue<long?>("In A:");
				lvA.Value = inputA.HasValue ? inputA.Value : (long?)null;
				lvA.FormattedValue = FormatValue(VALUE_WIDTH, lvA.Value);
				pipelineItem.InputA = lvA;

				var lvB = new LabeledValue<long?>("In B:");
				lvB.Value = inputB.HasValue ? inputB.Value : (long?)null;
				lvB.FormattedValue = FormatValue(VALUE_WIDTH, lvB.Value);
				pipelineItem.InputB = lvB;

				var lvASrc = new LabeledValue<DevicePort?>("Src A:");
				lvASrc.Value = inputASource == DevicePort.Default ? (DevicePort?)null : inputASource;
				lvASrc.FormattedValue = lvASrc.Value.ToString();
				pipelineItem.InputASource = lvASrc;

				var lvBSrc = new LabeledValue<DevicePort?>("Src B:");
				lvBSrc.Value = inputBSource == DevicePort.Default ? (DevicePort?)null : inputBSource;
				lvBSrc.FormattedValue = lvBSrc.Value.ToString();
				pipelineItem.InputBSource = lvBSrc;

				var lvOutputFormula = new LabeledValue<string>("Equation:");
				lvOutputFormula.Value = outputFormula;
				lvOutputFormula.FormattedValue = outputFormula;
				pipelineItem.OutputFormula = lvOutputFormula;

				pipelineItems.Add(pipelineItem);
			}

			return pipelineItems;
		}

		#endregion
	}
}
