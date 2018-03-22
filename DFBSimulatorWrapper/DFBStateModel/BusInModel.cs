using DFB_v1_40.Asm;
using DFBSimulatorWrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DFBSimulatorWrapper.DFBStateModel
{
	/// <summary>
	/// Bus in represents data being loaded into the DFB from Stage_A or Stage_B
	/// </summary>
	public class BusInModel : DFBModelBase
	{
		#region Private Members

		public static int VALUE_WIDTH = 6;

		#endregion
		#region Public Members

		public static int PIPELINE_DELAY => 0;

		/// <summary>
		/// Output fixed to hex format
		/// </summary>
		public LabeledValue<long?> OutputHex
		{
			get
			{
				var r = new LabeledValue<long?>("Val (hex):");
				r.Value = output.Value;
				r.FormattedValue = r.Value.HasValue ? FormatHex(DataValueWidth, r.Value.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Output fixed to integer format
		/// </summary>
		public LabeledValue<long?> OutputInt
		{
			get
			{
				var r = new LabeledValue<long?>("Val (int):");
				r.Value = output.Value;
				r.FormattedValue = r.Value.HasValue ? FormatIntegral(r.Value.Value) : "";
				return r;
			}
		}

		/// <summary>
		/// Output fixed to integer format
		/// </summary>
		public LabeledValue<long?> OutputDFBDec
		{
			get
			{
				var r = new LabeledValue<long?>("Val (q.23):");
				r.Value = output.Value;
				r.FormattedValue = r.Value.HasValue ? FormatDfbDecimal(DataValueWidth, r.Value.Value) : "";
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
		public BusInModel(Bank bankID, int cycle) : base(bankID, cycle, VALUE_WIDTH, 0, PIPELINE_DELAY)
		{
			// name
			name = bankID == Bank.Bank_A
				? DevicePort.Stage_A.ToString()
				: DevicePort.Stage_B.ToString();

			// instructions not used for busIn

			// connectionInputs not used for busIn

			// output
			output = OutputCalc(bankID, CycleActive.Value);
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
			int? val = null;

			bool busRead_A;
			bool busRead_B;

			BusRead(cycle,
				out busRead_A,
				out busRead_B);

			if (bankID == Bank.Bank_A && !busRead_A)
			{
				return NullLabeledValue<long?>(label);
			}
			else if (bankID == Bank.Bank_B && !busRead_B)
			{
				return NullLabeledValue<long?>(label);
			}

			val = DFBState.GetCySimulatorPrivateField<int?>(cycle, "p3bus");
			if (val == null || !val.HasValue) { return NullLabeledValue<long?>(label); }

			var r = new LabeledValue<long?>("Out:");
			r.Value = Convert.ToInt32(val.Value);
			r.FormattedValue = r.Value.HasValue ? FormatValue(VALUE_WIDTH, (int)r.Value) : "";

			return r;
		}

		/// <summary>
		/// Bus that was read
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <param name="busA"></param>
		/// <param name="busB"></param>
		public static void BusRead(int cycle, out bool busA, out bool busB)
		{
			busA = false;
			busB = false;
			bool busRead = false;

			var val = DFBState.GetCySimulatorPrivateField<int?>(cycle, "p3busflag");
			if (val != null) { busRead = Convert.ToBoolean(val.Value); }
			if (!busRead) { return; }

			int acuaddr1 = 0;
			val = DFBState.GetCySimulatorPrivateField<int?>(cycle, "acuaddr1");
			if (val == null || !val.HasValue) { return; }

			acuaddr1 = Convert.ToInt32(val.Value);
			if ((acuaddr1 & 1) != 0)
			{
				busA = true;
			}
			else
			{
				busB = true;
			}
		}

		/// <summary>
		/// Active instruction for the cycle given the pipeline length
		/// Returns null if the instruction doesn't apply to this device
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static CyControlWord ActiveInstr(int cycle)
		{
			// "ba":  // bus to alu
			// "bra": // bus to ram, ram to alu
			// "bm":  // bus to mac, mac to alu
			// "brm": // bus to ram, to mac
			var instr = CodeStoreModel.Instruction(cycle - PIPELINE_DELAY);
			if (instr == null) { return null; }

			if ((!instr.MuxA.StartsWith("b")
				&& !instr.MuxB.StartsWith("b")))
			{ return null; }

			return instr;
		}

		#endregion
	}
}
