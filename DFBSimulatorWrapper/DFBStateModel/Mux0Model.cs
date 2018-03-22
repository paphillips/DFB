using DFB_v1_40.Asm;
using DFBSimulatorWrapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFBSimulatorWrapper.DFBStateModel
{
	/// <summary>
	/// Mux0 connects Stage inputs to Mux 1
	/// </summary>
	public class Mux0Model : DFBModelBase
	{
		#region Private Members

		public const int VALUE_WIDTH = 6;
		private List<string> addresses;

		#endregion
		#region Public Members

		public static int PIPELINE_DELAY => 1;

		/// <summary>
		/// Address value associated with dmux command
		/// </summary>
		/// <remarks>addr is latched with the dmux command and follows it through the pipeline</remarks>
		public List<LabeledValue<string>> Addresses
		{
			get
			{
				var rtnList = new List<LabeledValue<string>>();
				foreach (var addr in addresses)
				{
					var r = new LabeledValue<string>("Addr:");
					r.Value = addr;
					r.FormattedValue = r.Value;
					rtnList.Add(r);
				}
				return rtnList;
			}
		}

		#endregion
		#region Constructor

		/// <summary>
		/// Create a new instance of this device for the given cycle
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		public Mux0Model(int cycle) : base(Bank.NotApplicable, cycle, VALUE_WIDTH, 0, PIPELINE_DELAY)
		{
			// name
			name = DevicePort.Mux_0.ToString();

			// instructions
			instructions.Add(ActiveInstr(cycle));
			instructions.Add(ActiveInstr(cycle - 1));

			// connectionInputs
			connectionInputs = Connections(cycle);

			// output
			output = OutputCalc(cycle);

			// addr
			addresses = new List<string>();
			addresses.Add(AddrCalc(cycle));
			addresses.Add(AddrCalc(cycle - 1));
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
			var muxInstrWord = ActiveInstrWord(cycle - PIPELINE_DELAY);
			if (muxInstrWord == null) { return NullLabeledValue<long?>(label); }

			var busChannel = muxInstrWord.AddrOp.Equals(1) ? "A" : "B";
			if (busChannel.Equals("A"))
			{
				return BusInModel.OutputCalc(Bank.Bank_A, cycle);
			}
			else
			{
				return BusInModel.OutputCalc(Bank.Bank_B, cycle);
			}
		}

		/// <summary>
		/// Active deviceport for cycle
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static DevicePort ActiveInputPort(int cycle)
		{
			var muxInstrWord = ActiveInstrWord(cycle - PIPELINE_DELAY);
			if (muxInstrWord == null) { return DevicePort.Default; }

			bool busRead_A;
			bool busRead_B;

			BusInModel.BusRead(cycle,
				out busRead_A,
				out busRead_B);

			if (busRead_A)
			{
				return DevicePort.Stage_A;
			}
			else if (busRead_B)
			{
				return DevicePort.Stage_B;
			}
			else { return DevicePort.Default; }
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

			// "ba":  // bus to alu
			// "bra": // bus to ram, ram to alu
			// "bm":  // bus to mac, mac to alu
			// "brm": // bus to ram, to mac
			var instr = CodeStoreModel.Instruction(cycle);

			if (instr != null)
			{
				return string.Format("({0},{1})", instr.MuxA, instr.MuxB);
			}
			else
			{
				return "";
			}
		}

		/// <summary>
		/// Instruction for the cycle given
		/// Returns null if the instruction doesn't apply to this device
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static CyControlWord ActiveInstrWord(int cycle)
		{
			// "ba":  // bus to alu
			// "bra": // bus to ram, ram to alu
			// "bm":  // bus to mac, mac to alu
			// "brm": // bus to ram, to mac
			var instr = CodeStoreModel.Instruction(cycle);
			if (instr == null || instr.MuxA == null || instr.MuxB == null) { return null; }

			if ((!instr.MuxA.StartsWith("b")
				&& !instr.MuxB.StartsWith("b")))
			{ return null; }

			return instr;
		}

		/// <summary>
		/// Input connections to device
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static List<Connection> Connections(int cycle)
		{
			var conns = new List<Connection>();

			DevicePort stagePortAOut = DevicePort.Stage_A;
			DevicePort stagePortBOut = DevicePort.Stage_B;
			DevicePort muxStagePortAIn = DevicePort.Mux_0;
			DevicePort muxStagePortBIn = DevicePort.Mux_0;

			PortStatus stagePortAOut_Stat;
			PortStatus stagePortBOut_Stat;
			PortStatus muxStagePortAIn_Stat;
			PortStatus muxStagePortBIn_Stat;

			InputsUsed(cycle,
				out stagePortAOut_Stat,
				out stagePortBOut_Stat,
				out muxStagePortAIn_Stat,
				out muxStagePortBIn_Stat);

			bool busRead_A;
			bool busRead_B;

			BusInModel.BusRead(cycle,
				out busRead_A,
				out busRead_B);

			var bankID = busRead_A
				? Bank.Bank_A
				: Bank.Bank_B;

			var outputCalc = BusInModel.OutputCalc(bankID, cycle);
			var activeLabel = outputCalc.FormattedValue;

			conns.Add(new Connection(
				BusType.Data,
				stagePortAOut,
				stagePortAOut_Stat,
				activeLabel,
				null,
				muxStagePortAIn,
				muxStagePortAIn_Stat));

			conns.Add(new Connection(
				BusType.Data,
				stagePortBOut,
				stagePortBOut_Stat,
				activeLabel,
				null,
				muxStagePortBIn,
				muxStagePortBIn_Stat));

			return conns;
		}

		/// <summary>
		/// Address of instruction for cycle
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static string AddrCalc(int cycle)
		{
			if (cycle < 0) { return ""; }

			var instr = CodeStoreModel.Instruction(cycle);

			return string.Format("addr({0})", instr.AddrOp);
		}

		/// <summary>
		/// Inputs used by the device in the active instruction
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <param name="stagePortAOut_Stat"></param>
		/// <param name="stagePortBOut_Stat"></param>
		/// <param name="muxStagePortAIn_Stat"></param>
		/// <param name="muxStagePortBIn_Stat"></param>
		private static void InputsUsed(
			int cycle,
			out PortStatus stagePortAOut_Stat,
			out PortStatus stagePortBOut_Stat,
			out PortStatus muxStagePortAIn_Stat,
			out PortStatus muxStagePortBIn_Stat)
		{
			stagePortAOut_Stat = PortStatus.Inactive;
			stagePortBOut_Stat = PortStatus.Inactive;
			muxStagePortAIn_Stat = PortStatus.Inactive;
			muxStagePortBIn_Stat = PortStatus.Inactive;

			bool busRead_A;
			bool busRead_B;

			BusInModel.BusRead(cycle,
				out busRead_A,
				out busRead_B);

			if (busRead_A)
			{
				stagePortAOut_Stat = PortStatus.Active;
				muxStagePortAIn_Stat = PortStatus.Active;
			}

			if (busRead_B)
			{
				stagePortBOut_Stat = PortStatus.Active;
				muxStagePortBIn_Stat = PortStatus.Active;
			}
		}

		#endregion
	}
}
