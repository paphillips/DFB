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
	/// DataRam represents the working data storage in the DFB
	/// </summary>
	public class DataRamModel : DFBModelBase
	{
		#region Private Members

		public readonly Bank BankId;
		private const int RAM_LENGTH = 128;
		private const int VALUE_WIDTH = 6;
		private const int ADDR_WIDTH = 2;
		private long[] ram;
		private int? address;
		private int? addressPrev;
		private long? valuePrev;
		private long? valueWritten;
		private List<RamItem> ramItems;
		private string ramString;

		#endregion
		#region Public Members

		public static int PIPELINE_DELAY => 0;

		/// <summary>
		/// Left (Bank A) or right (Bank B) 7 bits of the ACU ram array
		/// </summary>
		public long[] Ram
		{
			get
			{
				return ram;
			}
		}

		/// <summary>
		/// List of RamItems
		/// </summary>
		public List<RamItem> RamItems
		{
			get
			{
				return ramItems;
			}
		}

		/// <summary>
		/// Ram address of ACU
		/// </summary>
		public LabeledValue<int?> Address
		{
			get
			{
				var r = new LabeledValue<int?>("Addr:");
				r.Value = address;
				r.FormattedValue = r.Value.HasValue ? FormatHex(ADDR_WIDTH, r.Value.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Previous Ram address of ACU
		/// </summary>
		public LabeledValue<int?> AddressPrev
		{
			get
			{
				var r = new LabeledValue<int?>("Addr Prev:");
				r.Value = addressPrev;
				r.FormattedValue = r.Value.HasValue ? FormatHex(ADDR_WIDTH, r.Value.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Previous Ram address of ACU
		/// </summary>
		public LabeledValue<long?> ValuePrev
		{
			get
			{
				var r = new LabeledValue<long?>("Value Prev:");
				r.Value = valuePrev;
				r.FormattedValue = r.Value.HasValue ? FormatValue(DataValueWidth, r.Value.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Value written to the Ram address for write instructions
		/// </summary>
		public LabeledValue<long?> ValueWrite
		{
			get
			{
				var r = new LabeledValue<long?>("Written:");
				r.Value = valueWritten;
				r.FormattedValue = r.Value.HasValue ? FormatValue(DataValueWidth, r.Value.Value) : "";
				return r;
			}

		}
		
		/// <summary>
		/// String representing the ram addresses and values for display in a UI
		/// </summary>
		public string RamString
		{
			get
			{
				return ramString;
			}
		}

		#endregion
		#region Constructor

		/// <summary>
		/// Create a new instance of this device for the given cycle
		/// </summary>
		/// <param name="bankID">BankID of the device</param>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <param name="code">Data area section string from the assembly code file</param>
		public DataRamModel(Bank bankID, int cycle, string code) : base(bankID, cycle, VALUE_WIDTH, ADDR_WIDTH, PIPELINE_DELAY)
		{
			// name
			name = bankID == Bank.Bank_A
				? DevicePort.DataRam_A.ToString()
				: DevicePort.DataRam_B.ToString();

			// instructions
			var instr = ActiveInstr(bankID, cycle);
			instructions.Add(instr);

			// connectionInputs
			connectionInputs = Connections(bankID, cycle);

			// output
			output = OutputCalc(bankID, cycle);

			// ram
			var field = bankID == Bank.Bank_A ? "DAram" : "DBram";
			ram = DFBState.GetCySimulatorPrivateField<long[]>(cycle, field);

			// address / addressPrev
			field = bankID == Bank.Bank_A ? "Aaddrnext" : "Baddrnext";
			address = DFBState.GetCySimulatorPrivateField<int?>(cycle, field);
			addressPrev = DFBState.GetCySimulatorPrivateField<int?>(cycle - 1, field);

			// valuePrev
			valuePrev = OutputCalc(bankID, cycle - 1).Value;

			// valueWritten
			var writeInstr = instructions[0].Equals("write(db)") || instructions[0].Equals("write(da)");
			if (writeInstr)
			{
				var mux1_Output = Mux1Model.OutputCalc(bankID, cycle);
				if (mux1_Output.Value.HasValue)
				{
					valueWritten = mux1_Output.Value;
				}
			}

			ramItems = BuildRamItems(bankID, this.ram, code);

			var sb = new StringBuilder();
			foreach (var ramItem in RamItems)
			{
				sb.AppendFormat("{0,6} {1,8} {2,10} {3,11} {4}\n", "[" + ramItem.Address + "]", ramItem.ValHex, ramItem.ValDfb, ramItem.ValInt, ramItem.Comment);
			}
			ramString = sb.ToString().TrimEnd('\n');
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

			// ram
			var field = bankID == Bank.Bank_A ? "DAram" : "DBram";
			var ram = DFBState.GetCySimulatorPrivateField<long[]>(cycle, field);

			// address
			field = bankID == Bank.Bank_A ? "Aaddrnext" : "Baddrnext";
			var address = DFBState.GetCySimulatorPrivateField<int?>(cycle, field);

			// ramVal / output
			if (address != null
				&& address.HasValue
				&& ram != null)
			{
				var r = new LabeledValue<long?>(label);
				r.Value = (int?)ram[address.Value];
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
		/// <param name="bankID">BankID of the device</param>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static string ActiveInstr(Bank bankID, int cycle)
		{
			if (cycle < 0) { return ""; }

			string ramWriteOp = null;
			string ramALUReadOp = null;

			if (bankID == Bank.Bank_A && CodeStoreModel.Instruction(cycle).Da == 1)
			{
				ramWriteOp = "da";
			}
			if (bankID == Bank.Bank_A && CodeStoreModel.Instruction(cycle).Db == 1)
			{
				ramWriteOp = "db";
			}

			if (bankID == Bank.Bank_A && CodeStoreModel.Instruction(cycle).Equals("passrama"))
			{
				ramALUReadOp = "passrama";
			}
			if (bankID == Bank.Bank_B && CodeStoreModel.Instruction(cycle).Equals("passramb"))
			{
				ramALUReadOp = "passramb";
			}

			var sb = new StringBuilder();
			if (ramWriteOp != null)
			{
				sb.AppendFormat("write({0})", ramWriteOp);
			}
			if (ramWriteOp != null && ramALUReadOp != null)
			{
				sb.Append(", ");
			}
			if (ramALUReadOp != null)
			{
				sb.AppendFormat("alu({0})", ramALUReadOp);
			}

			return sb.ToString();
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

			DevicePort acuAddrPortOut = DevicePort.Default;
			DevicePort mux1PortOut = DevicePort.Default;

			DevicePort ramACUPortIn = DevicePort.Default;
			DevicePort ramMux1PortIn = DevicePort.Default;

			PortStatus acuAddrPortOut_Stat;
			PortStatus mux1PortOut_Stat;

			PortStatus ramACUPortIn_Stat;
			PortStatus ramMux1PortIn_Stat;

			InputsUsed(bankID, cycle,
				out acuAddrPortOut_Stat,
				out mux1PortOut_Stat,
				out ramACUPortIn_Stat,
				out ramMux1PortIn_Stat);

			var mux1_Model = Mux1Model.OutputCalc(bankID, cycle);
			var mux1_label = mux1_Model.FormattedValue;

			var acu_Model = ACUModel.OutputCalc(bankID, cycle);
			var acu_label = acu_Model.FormattedValue;

			switch (bankID)
			{
				case Bank.Bank_A:
					acuAddrPortOut = DevicePort.ACU_A;
					ramACUPortIn = DevicePort.DataRam_A;
					mux1PortOut = DevicePort.Mux_1A;
					ramMux1PortIn = DevicePort.DataRam_A;
					break;

				case Bank.Bank_B:
					acuAddrPortOut = DevicePort.ACU_B;
					ramACUPortIn = DevicePort.DataRam_B;
					mux1PortOut = DevicePort.Mux_1B;
					ramMux1PortIn = DevicePort.DataRam_B;
					break;
			}

			conns.Add(new Connection(
				BusType.Address,
				acuAddrPortOut,
				acuAddrPortOut_Stat,
				acu_label,
				null,
				ramACUPortIn,
				ramACUPortIn_Stat));

			conns.Add(new Connection(
				BusType.Data,
				mux1PortOut,
				mux1PortOut_Stat,
				mux1_label,
				null,
				ramMux1PortIn,
				ramMux1PortIn_Stat));

			return conns;
		}

		/// <summary>
		/// Inputs used by the device in the active instruction
		/// </summary>
		/// <param name="bankID">BankID of the device</param>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <param name="acuAddrPortOut_Stat"></param>
		/// <param name="ramACUPortIn_Stat"></param>
		/// <param name="mux1PortOut_Stat"></param>
		/// <param name="ramMux1PortIn_Stat"></param>
		private static void InputsUsed(
			Bank bankID,
			int cycle,
			out PortStatus acuAddrPortOut_Stat,
			out PortStatus mux1PortOut_Stat,
			out PortStatus ramACUPortIn_Stat,
			out PortStatus ramMux1PortIn_Stat)
		{
			ramMux1PortIn_Stat = PortStatus.Inactive;

			// acu addr out, ram addr, and mux1 out in are always active
			acuAddrPortOut_Stat = PortStatus.Active;
			ramACUPortIn_Stat = PortStatus.Active;
			mux1PortOut_Stat = PortStatus.Active;

			// mux1in and is active if a ram write operation is in progress
			if (cycle < 0) { return; }

			var instr = CodeStoreModel.Instruction(cycle);
			if (instr == null) { return; }

			// ram write instruction?
			if ((bankID == Bank.Bank_A && instr.Da == 1)
				|| (bankID == Bank.Bank_B && instr.Db == 1))
			{
				ramMux1PortIn_Stat = PortStatus.Active;
			}
		}

		/// <summary>
		/// Build a list of RamItems
		/// </summary>
		/// <param name="bankID">BankID of the device</param>
		/// <param name="ram">Ram array</param>
		/// <param name="code">Data area section string from the assembly code file</param>
		/// <returns></returns>
		private static List<RamItem> BuildRamItems(Bank bankID, long[] ram, string code)
		{
			var rtnRamItems = new List<RamItem>();

			var ramList = ram.ToList();
			var outList = new List<RamItem>();

			var area = bankID == Bank.Bank_A
				? RamItem.Area.data_a
				: RamItem.Area.data_b;
			var commentsList = RamItem.RamCodeComments(area, code);

			for (int i = 0; i < RAM_LENGTH; i++)
			{
				var label = string.Format("{0} ", i);

				if (ramList != null && ramList.Count() > 0)
				{
					outList.Add(new RamItem()
					{
						Address = FormatHex(ADDR_WIDTH, i),
						ValInt = FormatIntegral(ramList[i]),
						ValHex = FormatHex(VALUE_WIDTH, ramList[i]),
						ValDfb = FormatDfbDecimal(VALUE_WIDTH, ramList[i]),
						Comment = commentsList[i]
					});
				}
				else
				{
					outList.Add(new RamItem()
					{
						Address = FormatHex(ADDR_WIDTH, i),
						ValInt = null,
						ValHex = null,
						ValDfb = null,
						Comment = null
					});
				}
			}
			return outList;
		}

		#endregion
	}
}
