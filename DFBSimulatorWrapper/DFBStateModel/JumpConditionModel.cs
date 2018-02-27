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
	/// Displays the status of values that can be evaluated in jump condition true branch decisions
	/// </summary>
	public class JumpConditionModel : DFBModelBase
	{
		#region Nested Structures

		public struct JumpConditionItem
		{
			public string Name;
			public int PipelineDelay;
			public bool Enabled;
			public bool? T_2Value;
			public bool? T_1Value;
			public bool T_0Value;
			public bool Jump;
		}

		#endregion
		#region Private Members

		private const int VALUE_WIDTH = 6;
		private const int ADDR_WIDTH = 2;
		private const int PIPELINE_DELAY = 0;
		private List<JumpConditionItem> jumpConditionItems;
		private string jumpConditionsReport;

		#endregion
		#region Public Members
		
		/// <summary>
		/// Values
		/// </summary>
		public List<JumpConditionItem> JumpConditionItems
		{
			get
			{
				return jumpConditionItems;
			}
		}

		/// <summary>
		/// String representing the state of jump conditions for use in a UI
		/// </summary>
		public string JumpConditionsReport
		{
			get
			{
				return jumpConditionsReport;
			}
		}

		public bool AcuAEQ { get; private set; }
		public bool AcuBEQ { get; private set; }
		public bool DpEQ { get; private set; }
		public bool DpSign { get; private set; }
		public bool DpThresh { get; private set; }
		public bool Eob { get; private set; }
		public bool In1 { get; private set; }
		public bool In2 { get; private set; }
		public bool Sat { get; private set; }
		public bool Sem0 { get; private set; }
		public bool Sem1 { get; private set; }
		public bool Sem2 { get; private set; }
		public bool Glob_Int1 { get; private set; }
		public bool Glob_Int2 { get; private set; }
		
		#endregion
		#region Constructor

		/// <summary>
		/// Create a new instance of this model
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		public JumpConditionModel(int cycle) : base(Bank.NotApplicable, cycle, VALUE_WIDTH, ADDR_WIDTH, PIPELINE_DELAY)
		{
			// name
			name = DevicePort.JumpConditions.ToString();

			// instructions
			instructions.Add(Instr(cycle));
			instructions.Add(Instr(cycle - 1));
			var activeInstr = Instr(cycle - 2);
			instructions.Add(activeInstr);

			// pipelineItems
			jumpConditionItems = BuildJumpConditionItems(cycle, instructions);

			var sb = new StringBuilder();
			sb.AppendFormat("{0,9} {1,6}\n",
				"Jmp Cond",
				"[now]");
			sb.AppendFormat("------------------------------\n");

			foreach (var item in jumpConditionItems)
			{
				sb.AppendFormat("{0,9} {1,6}\n",
					item.Name,
					string.Format(new FormatProviderBoolean(), "{0:true}", item.T_0Value));

				switch (item.Name)
				{
					case "eob":
					case "dpsign":
					case "dpthresh":
					case "dpeq":
					case "acubeq":
					case "in2":
					case "sem_en0":
					case "sem_en1":
					case "sem_en2":
					case "glob_en1":
					case "glob_en2":
					case "sat_en":
						sb.AppendLine();
						break;
				}


			}
			jumpConditionsReport = sb.ToString();
		}

		#endregion
		#region Methods

		/// <summary>
		/// Return the output of this device
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		public static LabeledValue<long?> OutputCalc(int cycle)
		{
			var label = "Out:";
			return NullLabeledValue<long?>(label);
		}

		/// <summary>
		/// Input connections to device
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static List<Connection> Connections(int cycle)
		{
			var conns = new List<Connection>();
			return conns;
		}

		/// <summary>
		/// Returns the instruction text for the cycle
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
		/// Build a list of jump conditions
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <param name="instructions"></param>
		/// <returns></returns>
		private List<JumpConditionItem> BuildJumpConditionItems(int cycle, List<string> instructions)
		{
			var jumpConditionItems = new List<JumpConditionItem>();

			var item = new JumpConditionItem();
			item.Name = "eob";
			item.PipelineDelay = 0;
			item.T_2Value = true;
			item.T_1Value = true;
			item.T_0Value = true;
			jumpConditionItems.Add(item);
			Eob = true;

			item = new JumpConditionItem();
			item.Name = "dpsign";
			item.PipelineDelay = 2;
			item.T_2Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 2, "dpsign"));
			item.T_1Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 1, "dpsign"));
			item.T_0Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 0, "dpsign"));
			jumpConditionItems.Add(item);
			DpSign = item.T_0Value;

			item = new JumpConditionItem();
			item.Name = "dpthresh";
			item.PipelineDelay = 2;
			item.T_2Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 2, "tflag"));
			item.T_1Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 1, "tflag"));
			item.T_0Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 0, "tflag"));
			jumpConditionItems.Add(item);
			DpThresh = item.T_0Value;

			item = new JumpConditionItem();
			item.Name = "dpeq";
			item.PipelineDelay = 2;
			item.T_2Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 2, "eqflag"));
			item.T_1Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 1, "eqflag"));
			item.T_0Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 0, "eqflag"));
			jumpConditionItems.Add(item);
			DpEQ = item.T_0Value;

			item = new JumpConditionItem();
			item.Name = "acuaeq";
			item.PipelineDelay = 1;
			item.T_2Value = null;
			item.T_1Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 1, "aacueq"));
			item.T_0Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 0, "aacueq"));
			jumpConditionItems.Add(item);
			AcuAEQ = item.T_0Value;

			item = new JumpConditionItem();
			item.Name = "acubeq";
			item.PipelineDelay = 1;
			item.T_2Value = null;
			item.T_1Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 1, "bacueq"));
			item.T_0Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 0, "bacueq"));
			jumpConditionItems.Add(item);
			AcuBEQ = item.T_0Value;

			bool busRead_A_1;
			bool busRead_B_1;
			bool busRead_A_0;
			bool busRead_B_0;
			BusInModel.BusRead(cycle, out busRead_A_1, out busRead_B_1);
			BusInModel.BusRead(cycle - 1, out busRead_A_0, out busRead_B_0);

			item = new JumpConditionItem();
			item.Name = "in1";
			item.PipelineDelay = 1;
			item.T_2Value = null;
			item.T_1Value = busRead_A_1;
			item.T_0Value = busRead_A_0;
			jumpConditionItems.Add(item);
			In1 = item.T_0Value;

			item = new JumpConditionItem();
			item.Name = "in2";
			item.PipelineDelay = 1;
			item.T_2Value = null;
			item.T_1Value = busRead_B_1;
			item.T_0Value = busRead_B_0;
			jumpConditionItems.Add(item);
			In2 = item.T_0Value;

			var semT_0 = DFBState.GetCySimulatorPrivateField<int[]>(cycle, "sem");

			var sem_enT_2 = DFBState.GetCySimulatorPrivateField<int[]>(cycle + 2, "sem_en");
			var sem_enT_1 = DFBState.GetCySimulatorPrivateField<int[]>(cycle + 1, "sem_en");
			var sem_enT_0 = DFBState.GetCySimulatorPrivateField<int[]>(cycle + 0, "sem_en");

			item = new JumpConditionItem();
			item.Name = "sem_0";
			item.PipelineDelay = 1;
			item.T_2Value = null;
			item.T_1Value = null;
			item.T_0Value = Convert.ToBoolean(semT_0 == null ? 0 : semT_0[0]);
			jumpConditionItems.Add(item);
			Sem0 = item.T_0Value;

			item = new JumpConditionItem();
			item.Name = "sem_en0";
			item.PipelineDelay = 2;
			item.T_2Value = Convert.ToBoolean(sem_enT_2 == null ? 0 : sem_enT_2[0]);
			item.T_1Value = Convert.ToBoolean(sem_enT_1 == null ? 0 : sem_enT_1[0]);
			item.T_0Value = Convert.ToBoolean(sem_enT_0 == null ? 0 : sem_enT_0[0]);
			jumpConditionItems.Add(item);

			item = new JumpConditionItem();
			item.Name = "sem_1";
			item.PipelineDelay = 1;
			item.T_2Value = null;
			item.T_1Value = null;
			item.T_0Value = Convert.ToBoolean(semT_0 == null ? 0 : semT_0[1]);
			jumpConditionItems.Add(item);
			Sem1 = item.T_0Value;

			item = new JumpConditionItem();
			item.Name = "sem_en1";
			item.PipelineDelay = 2;
			item.T_2Value = Convert.ToBoolean(sem_enT_2 == null ? 0 : sem_enT_2[1]);
			item.T_1Value = Convert.ToBoolean(sem_enT_1 == null ? 0 : sem_enT_1[1]);
			item.T_0Value = Convert.ToBoolean(sem_enT_0 == null ? 0 : sem_enT_0[1]);
			jumpConditionItems.Add(item);

			item = new JumpConditionItem();
			item.Name = "sem_2";
			item.PipelineDelay = 1;
			item.T_2Value = null;
			item.T_1Value = null;
			item.T_0Value = Convert.ToBoolean(semT_0 == null ? 0 : semT_0[2]);
			jumpConditionItems.Add(item);
			Sem2 = item.T_0Value;

			item = new JumpConditionItem();
			item.Name = "sem_en2";
			item.PipelineDelay = 2;
			item.T_2Value = Convert.ToBoolean(sem_enT_2 == null ? 0 : sem_enT_2[2]);
			item.T_1Value = Convert.ToBoolean(sem_enT_1 == null ? 0 : sem_enT_1[2]);
			item.T_0Value = Convert.ToBoolean(sem_enT_0 == null ? 0 : sem_enT_0[2]);
			jumpConditionItems.Add(item);

			var global_enT_2 = DFBState.GetCySimulatorPrivateField<int[]>(cycle + 2, "global_en");
			var global_enT_1 = DFBState.GetCySimulatorPrivateField<int[]>(cycle + 1, "global_en");
			var global_enT_0 = DFBState.GetCySimulatorPrivateField<int[]>(cycle, "global_en");

			item = new JumpConditionItem();
			item.Name = "glob_in1";
			item.PipelineDelay = 1;
			item.T_2Value = null;
			item.T_1Value = null;
			item.T_0Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 0, "g1"));
			jumpConditionItems.Add(item);
			Glob_Int1 = item.T_0Value;

			item = new JumpConditionItem();
			item.Name = "glob_en1";
			item.PipelineDelay = 2;
			item.T_2Value = Convert.ToBoolean(global_enT_2 == null ? 0 : global_enT_2[0]);
			item.T_1Value = Convert.ToBoolean(global_enT_1 == null ? 0 : global_enT_1[0]);
			item.T_0Value = Convert.ToBoolean(global_enT_0 == null ? 0 : global_enT_0[0]);
			jumpConditionItems.Add(item);

			item = new JumpConditionItem();
			item.Name = "glob_in2";
			item.PipelineDelay = 1;
			item.T_2Value = null;
			item.T_1Value = null;
			item.T_0Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 0, "g2"));
			jumpConditionItems.Add(item);
			Glob_Int2 = item.T_0Value;

			item = new JumpConditionItem();
			item.Name = "glob_en2";
			item.PipelineDelay = 2;
			item.T_2Value = Convert.ToBoolean(global_enT_2 == null ? 0 : global_enT_2[1]);
			item.T_1Value = Convert.ToBoolean(global_enT_1 == null ? 0 : global_enT_1[1]);
			item.T_0Value = Convert.ToBoolean(global_enT_0 == null ? 0 : global_enT_0[1]);
			jumpConditionItems.Add(item);

			item = new JumpConditionItem();
			item.Name = "sat";
			item.PipelineDelay = 1;
			item.T_2Value = null;
			item.T_1Value = null;
			item.T_0Value = Convert.ToBoolean(DFBState.GetCySimulatorPrivateField<int?>(cycle + 0, "satflag"));
			jumpConditionItems.Add(item);
			Sat = item.T_0Value;

			item = new JumpConditionItem();
			item.Name = "sat_en";
			item.PipelineDelay = 2;
			item.T_2Value = Convert.ToBoolean(global_enT_2 == null ? 0 : global_enT_2[2]);
			item.T_1Value = Convert.ToBoolean(global_enT_1 == null ? 0 : global_enT_1[2]);
			item.T_0Value = Convert.ToBoolean(global_enT_0 == null ? 0 : global_enT_0[2]);
			jumpConditionItems.Add(item);

			return jumpConditionItems;
		}

		#endregion
	}
}
