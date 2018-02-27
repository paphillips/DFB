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
	public class GlobalModel : DFBModelBase
	{
		#region Constants

		private const int VALUE_WIDTH = 6;
		private const int ADDR_WIDTH = 2;
		private const int PIPELINE_DELAY = 0;

		#endregion
		#region Members

		private int? global_en0;
		private int? global_en1;
		private int? global_en2;
		private int? satEn;
		private int? sqcnt;
		private int? sqcval;
		private int? satflag;
		private int? rflag;
		private int? tflag;
		private int? tsign;
		private int? sem_en0;
		private int? sem_en1;
		private int? sem_en2;
		private int? dpsign;
		private int? dpeq;

		/// <summary>
		/// Global enable 0
		/// </summary>
		public LabeledValue<int?> Global_en0
		{
			get
			{
				var r = new LabeledValue<int?>("Glob Enab 0:");
				r.Value = global_en0;
				r.FormattedValue = r.Value.HasValue ? FormatFlag(r.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Global enable 1
		/// </summary>
		public LabeledValue<int?> Global_en1
		{
			get
			{
				var r = new LabeledValue<int?>("Glob Enab 1:");
				r.Value = global_en1;
				r.FormattedValue = r.Value.HasValue ? FormatFlag(r.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Global enable 2
		/// </summary>
		public LabeledValue<int?> Global_en2
		{
			get
			{
				var r = new LabeledValue<int?>("Glob Enab 2:");
				r.Value = global_en2;
				r.FormattedValue = r.Value.HasValue ? FormatFlag(r.Value) : "";
				return r;
			}

		}

		/// <summary>
		/// Global enable
		/// </summary>
		public LabeledValue<int?> SatEn
		{
			get
			{
				var r = new LabeledValue<int?>("Saturation Enable:");
				r.Value = satEn;
				r.FormattedValue = r.Value.HasValue ? FormatFlag(r.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Squelch Count
		/// </summary>
		public LabeledValue<int?> Sqcnt
		{
			get
			{
				var r = new LabeledValue<int?>("Squelch Comp:");
				r.Value = sqcnt;
				r.FormattedValue = r.Value.HasValue ? FormatDfbDecimal(7, r.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Squelch Count
		/// </summary>
		public LabeledValue<int?> Sqcval
		{
			get
			{
				var r = new LabeledValue<int?>("Squelch Count:");
				r.Value = sqcval;
				r.FormattedValue = r.Value.HasValue ? FormatIntegral(r.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Saturation Flag
		/// </summary>
		public LabeledValue<int?> Satflag
		{
			get
			{
				var r = new LabeledValue<int?>("Saturation Flag:");
				r.Value = satflag;
				r.FormattedValue = r.Value.HasValue ? FormatFlag(r.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Rounding Flag
		/// </summary>
		public LabeledValue<int?> Rflag
		{
			get
			{
				var r = new LabeledValue<int?>("Rounding Flag:");
				r.Value = rflag;
				r.FormattedValue = r.Value.HasValue ? FormatFlag(r.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Threshold Det Flag
		/// </summary>
		public LabeledValue<int?> Tflag
		{
			get
			{
				var r = new LabeledValue<int?>("Thresh Det Flg:");
				r.Value = tflag;
				r.FormattedValue = r.Value.HasValue ? FormatFlag(r.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Threshold Sign
		/// </summary>
		public LabeledValue<int?> Tsign
		{
			get
			{
				var r = new LabeledValue<int?>("Datapath Thrsh:");
				r.Value = tsign;
				r.FormattedValue = r.Value.HasValue ? FormatFlag(r.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Datapath Sign
		/// </summary>
		public LabeledValue<int?> DPsign
		{
			get
			{
				var r = new LabeledValue<int?>("Datapath Sign:");
				r.Value = dpsign;
				r.FormattedValue = r.Value.HasValue ? FormatFlag(r.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Datapath Sign
		/// </summary>
		public LabeledValue<int?> DPeq
		{
			get
			{
				var r = new LabeledValue<int?>("Datapath Zero:");
				r.Value = dpeq;
				r.FormattedValue = r.Value.HasValue ? FormatFlag(r.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Sem 0 Enable
		/// </summary>
		public LabeledValue<int?> Sem_en0
		{
			get
			{
				var r = new LabeledValue<int?>("Sem Enable 0:");
				r.Value = sem_en0;
				r.FormattedValue = r.Value.HasValue ? FormatFlag(r.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Sem 1 Enable
		/// </summary>
		public LabeledValue<int?> Sem_en1
		{
			get
			{
				var r = new LabeledValue<int?>("Sem Enable 1:");
				r.Value = sem_en1;
				r.FormattedValue = r.Value.HasValue ? FormatFlag(r.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Sem 2 Enable
		/// </summary>
		public LabeledValue<int?> Sem_en2
		{
			get
			{
				var r = new LabeledValue<int?>("Sem Enable 2:");
				r.Value = sem_en2;
				r.FormattedValue = r.Value.HasValue ? FormatFlag(r.Value) : "";
				return r;
			}
		}

		#endregion
		#region Constructor

		/// <summary>
		/// Create a new instance of this device for the given cycle
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		public GlobalModel(int cycle) : base(Bank.NotApplicable, cycle, VALUE_WIDTH, ADDR_WIDTH, PIPELINE_DELAY)
		{
			// name
			name = DevicePort.Global.ToString();

			// instructions
			instructions.Add(ActiveInstr(cycle - ALUModel.PIPELINE_DELAY));

			// connectionInputs
			connectionInputs = Connections(cycle);

			// output
			output = OutputCalc(cycle);

			// global_en
			var global_en = DFBState.GetCySimulatorPrivateField<int[]>(cycle, "global_en");
			if (global_en != null)
			{
				global_en0 = global_en[0];
				global_en1 = global_en[1];
				global_en2 = global_en[2];
			}

			satEn = DFBState.GetCySimulatorPrivateField<int?>(cycle, "satEn");
			sqcnt = DFBState.GetCySimulatorPrivateField<int?>(cycle, "sqcnt");
			sqcval = DFBState.GetCySimulatorPrivateField<int?>(cycle, "sqcval");
			satflag = DFBState.GetCySimulatorPrivateField<int?>(cycle, "satflag");
			rflag = DFBState.GetCySimulatorPrivateField<int?>(cycle, "rflag");
			tflag = DFBState.GetCySimulatorPrivateField<int?>(cycle, "tflag");
			tsign = DFBState.GetCySimulatorPrivateField<int?>(cycle, "tsign");
			dpsign = DFBState.GetCySimulatorPrivateField<int?>(cycle, "dpsign");
			dpeq = DFBState.GetCySimulatorPrivateField<int?>(cycle, "eqflag");


			// sem_en0/1/2
			var sem_en = DFBState.GetCySimulatorPrivateField<int[]>(cycle, "sem_en");
			if (sem_en != null)
			{
				sem_en0 = sem_en[0];
				sem_en1 = sem_en[1];
				sem_en2 = sem_en[2];
			}
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
		/// Instruction for the cycle given
		/// Returns null if the instruction doesn't apply to this device
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		public static CyControlWord ActiveInstrWord(int cycle)
		{
			return CodeStoreModel.Instruction(cycle);
		}

		/// <summary>
		/// Instruction for the cycle
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		public static string ActiveInstr(int cycle)
		{
			if (cycle < 0) { return ""; }

			var instr = CodeStoreModel.Instruction(cycle);
			string activeInstr = null;

			switch (instr.Alu ?? "")
			{
				case "set0":
				case "set1":
				case "seta":
				case "setb":
				case "nega":
				case "negb":
				case "passrama":
				case "passramb":
				case "add":
				case "tdeca":
				case "suba":
				case "subb":
				case "absa":
				case "absb":
				case "addabsa":
				case "addabsb":
				case "hold":
					break;

				case "englobals":
				case "ensatrnd":
				case "ensem":
				case "setsem":
				case "clearsem":
					activeInstr = instr.Alu;
					break;

				case "tsuba":
				case "tsubb":
				case "taddabsa":
				case "taddabsb":
				case "sqlcmp":
				case "sqlcnt":
				case "sqa":
				case "sqb":
					break;
			}

			return activeInstr;
		}

		/// <summary>
		/// Input connections to device
		/// </summary>
		/// <returns></returns>
		public static List<Connection> Connections(int cycle)
		{
			var conns = new List<Connection>();
			var instr = ActiveInstr(cycle - ALUModel.PIPELINE_DELAY);

			PortStatus aluActive = PortStatus.Inactive;

			switch (instr ?? "")
			{
				case "set0":
				case "set1":
				case "seta":
				case "setb":
				case "nega":
				case "negb":
				case "passrama":
				case "passramb":
				case "add":
				case "tdeca":
				case "suba":
				case "subb":
				case "absa":
				case "absb":
				case "addabsa":
				case "addabsb":
				case "hold":
					break;

				case "englobals":
					aluActive = PortStatus.Active;
					break;
				case "ensatrnd":
					aluActive = PortStatus.Active;
					break;
				case "ensem":
					aluActive = PortStatus.Active;
					break;
				case "setsem":
					aluActive = PortStatus.Active;
					break;
				case "clearsem":
					aluActive = PortStatus.Active;
					break;

				case "tsuba":
				case "tsubb":
				case "taddabsa":
				case "taddabsb":
				case "sqlcmp":
				case "sqlcnt":
				case "sqa":
				case "sqb":
					break;
			}

			conns.Add(new Connection(
				BusType.Data,
				DevicePort.ALU,
				aluActive,
				instr,
				null,
				DevicePort.Global,
				aluActive));

			return conns;
		}

		#endregion
	}
}
