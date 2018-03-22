using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFBSimulatorWrapper.DFBStateModel
{
	/// <summary>
	/// The address calculation unit (ACU) outputs the data RAM 
	/// addresses required for the next instruction cycle
	/// </summary>
	public class ACUModel : DFBModelBase
	{
		#region Private Members

		private const int RAM_LENGTH = 16;
		private const int VALUE_WIDTH = 2;
		private const int ADDR_WIDTH = 1;
		private int? reg_reg;
		private int? reg_freg;
		private int? reg_mreg;
		private int? reg_lreg;
		private int? reg_flag_mod;
		private int[] ram;
		private int? address;
		private int? addressPrev;
		private List<RamItem> ramItems;
		private string ramString;

		#endregion
		#region Public Members

		public static int PIPELINE_DELAY => 0;

		/// <summary>
		/// reg is the current DataRam address
		/// </summary>
		public LabeledValue<int?> Reg_reg
		{
			get
			{
				var r = new LabeledValue<int?>("reg:");
				r.Value = reg_reg;
				r.FormattedValue = r.Value.HasValue ? FormatHex(DataValueWidth, r.Value.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// freg can be loaded with the value that the data RAMs increment or decrement, when using the addf and subf commands.
		/// For example: load three into freg and you can increment through the data RAMs by three using ACU’s ‘addf’ instruction.Default = 2
		/// </summary>
		public LabeledValue<int?> Reg_freg
		{
			get
			{
				var r = new LabeledValue<int?>("freg:");
				r.Value = reg_freg;
				r.FormattedValue = r.Value.HasValue ? FormatIntegral(r.Value.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Stores the maximum value before wraparound to the lreg value when modulus arithmetic is enabled. Default = 127
		/// </summary>
		public LabeledValue<int?> Reg_mreg
		{
			get
			{
				var r = new LabeledValue<int?>("mreg:");
				r.Value = reg_mreg;
				r.FormattedValue = r.Value.HasValue ? FormatHex(DataValueWidth, r.Value.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// lreg stores the minimum value before wraparound to the mreg value when modulus arithmetic is enabled. Default = 0
		/// </summary>
		public LabeledValue<int?> Reg_lreg
		{
			get
			{
				var r = new LabeledValue<int?>("lreg:");
				r.Value = reg_lreg;
				r.FormattedValue = r.Value.HasValue ? FormatHex(DataValueWidth, r.Value.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Modulus arithmetic flag
		/// </summary>
		public LabeledValue<int?> Reg_flag_mod
		{
			get
			{
				var r = new LabeledValue<int?>("flag_mod:");
				r.Value = reg_flag_mod;
				r.FormattedValue = r.Value.HasValue ? FormatFlag(r.Value.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Left (Bank A) or right (Bank B) 7 bits of the ACU ram array
		/// </summary>
		public int[] Ram
		{
			get
			{
				return ram;
			}
		}

		/// <summary>
		/// Left (Bank A) or right (Bank B) 7 bits of the ACU ram array
		/// </summary>
		public List<RamItem> RamItems
		{
			get
			{
				return ramItems;
			}
		}

		/// <summary>
		/// String to display all ram details in a UI
		/// </summary>
		public string RamString
		{
			get
			{
				return ramString;
			}
		}

		/// <summary>
		/// Ram address of ACU
		/// </summary>
		public LabeledValue<int?> Address
		{
			get
			{
				var r = new LabeledValue<int?>("addr:");
				r.Value = address;
				r.FormattedValue = r.Value.HasValue ? FormatHex(DataAddressWidth, r.Value.Value) : "";
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
				var r = new LabeledValue<int?>("addr:");
				r.Value = addressPrev;
				r.FormattedValue = r.Value.HasValue ? FormatHex(DataAddressWidth, r.Value.Value) : "";
				return r;
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
		public ACUModel(Bank bankID, int cycle, string code) : base(bankID, cycle, VALUE_WIDTH, ADDR_WIDTH, PIPELINE_DELAY)
		{
			// ACU values update during simulator makestep(), so properties must be read from previous cycle
			//cycle -= 1;
			if (cycle < 0) { return; }

			// name
			name = bankID == Bank.Bank_A
				? DevicePort.ACU_A.ToString()
				: DevicePort.ACU_B.ToString();

			// instructions
			var instr = bankID == Bank.Bank_A
				? CodeStoreModel.Instruction(cycle).AcuA
				: CodeStoreModel.Instruction(cycle).AcuB;
			instructions.Add(instr);

			// ACU has no connectionInputs
			// connectionInputs

			// output
			output = OutputCalc(bankID, cycle);

			// reg
			var field = bankID == Bank.Bank_A ? "Aacc" : "Bacc";
			reg_reg = DFBState.GetCySimulatorPrivateField<int?>(cycle, field);

			// freg
			field = bankID == Bank.Bank_A ? "Afreg" : "Bfreg";
			reg_freg = DFBState.GetCySimulatorPrivateField<int?>(cycle, field);

			// mreg
			field = bankID == Bank.Bank_A ? "Amreg" : "Bmreg";
			reg_mreg = DFBState.GetCySimulatorPrivateField<int?>(cycle, field);

			// lreg
			field = bankID == Bank.Bank_A ? "Alreg" : "Blreg";
			reg_lreg = DFBState.GetCySimulatorPrivateField<int?>(cycle, field);

			// flag_mod
			field = bankID == Bank.Bank_A ? "Amodflag" : "Bmodflag";
			reg_flag_mod = DFBState.GetCySimulatorPrivateField<int?>(cycle, field);

			// ram
			field = bankID == Bank.Bank_A ? "aacuram" : "bacuram";
			ram = DFBState.GetCySimulatorPrivateField<int[]>(cycle, field);

			// address
			field = "acuaddr";
			address = DFBState.GetCySimulatorPrivateField<int?>(cycle - 0, field);

			// addressPrev
			field = "acuaddr";
			addressPrev = DFBState.GetCySimulatorPrivateField<int?>(cycle - 1, field);

			ramItems = BuildRamItems(bankID, this.ram, code);

			var sb = new StringBuilder();
			foreach (var ramItem in RamItems)
			{
				sb.AppendFormat("{0,4} {1,4} {2,3} {3}\n", "[" + ramItem.Address + "]", ramItem.ValHex, ramItem.ValInt, ramItem.Comment);
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

			// reg
			var field = bankID == Bank.Bank_A ? "Aacc" : "Bacc";
			var reg_reg = DFBState.GetCySimulatorPrivateField<int?>(cycle, field);
			if (reg_reg != null)
			{
				var r = new LabeledValue<long?>(label);
				r.Value = reg_reg.Value;
				r.FormattedValue = r.Value.HasValue ? FormatHex(VALUE_WIDTH, r.Value) : "";
				return r;
			}
			else
			{
				return NullLabeledValue<long?>(label);
			}
		}

		/// <summary>
		/// Build a list of RamItems
		/// </summary>
		/// <param name="bankID">BankID of the device</param>
		/// <param name="ram">Ram array</param>
		/// <param name="code">Data area section string from the assembly code file</param>
		/// <returns></returns>
		private static List<RamItem> BuildRamItems(Bank bankID, int[] ram, string code)
		{
			var rtnRamItems = new List<RamItem>();

			var ramList = ram.ToList();
			var outList = new List<RamItem>();

			var area = bankID == Bank.Bank_A
				? RamItem.Area.acu_a
				: RamItem.Area.acu_b;

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
	}

	#endregion
}