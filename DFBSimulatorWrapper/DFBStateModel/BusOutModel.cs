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
	/// Bus out represents data being output from the DFB to Hold_A or Hold_B
	/// </summary>
	public class BusOutModel : DFBModelBase
	{
		#region Private Members

		private const int VALUE_WIDTH = 6;
		private const int PIPELINE_DELAY = 0;

		#endregion
		#region Public Members

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
		public BusOutModel(Bank bankID, int cycle) : base(bankID, cycle, VALUE_WIDTH, 0, PIPELINE_DELAY)
		{
			// name
			name = bankID == Bank.Bank_A
				? DevicePort.Hold_A.ToString()
				: DevicePort.Hold_B.ToString();

			// instructions
			instructions.Add(ActiveInstr(bankID, cycle));

			// connectionInputs
			connectionInputs = Connections(bankID, cycle);

			// output
			output = OutputCalc(bankID, cycle);
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

			var busWritten = BusWritten(cycle);
			if (busWritten == null) { return NullLabeledValue<long?>(label); }

			if (bankID == Bank.Bank_A && busWritten.Equals("A") ||
				bankID == Bank.Bank_B && busWritten.Equals("B"))
			{
				// Value
				var shift = DFBState.GetCySimulatorPrivateField<long>(cycle, "shift");

				var r = new LabeledValue<long?>("Out:");
				r.Value = shift;
				r.FormattedValue = r.Value.HasValue ? FormatValue(VALUE_WIDTH, (int)r.Value) : "";

				return r;
			}
			else
			{
				return NullLabeledValue<long?>(label);
			}
		}

		/// <summary>
		/// Bus that was written: "A" | "B" | null
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static string BusWritten(int cycle)
		{
			// Was bus written?
			int cword;
			bool busWasWritten;
			bool busAWritten;
			bool busBWritten;

			cword = DFBState.GetCySimulatorPrivateField<int>(cycle, "cword");
			busWasWritten = (cword >> 13 & 1) == 1 ? true : false;
			if (!busWasWritten) { return null; }

			// Bus A or B?
			var busAddr = DFBState.GetCySimulatorPrivateField<int>(cycle, "acuaddr");   // 1 = A, 2 = B
			busAWritten = busAddr.Equals(1);
			busBWritten = busAddr.Equals(0);
			
			if (busAWritten) { return "A"; }
			else if (busBWritten) { return "B"; }
			else { return null; }
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

			var busWritten = BusWritten(cycle);
			if (busWritten == null) { return ""; }
			if (bankID == Bank.Bank_A && busWritten.Equals("A"))
			{
				return "write(abus)";
			}
			else if (bankID == Bank.Bank_B && busWritten.Equals("B"))
			{
				return "write(bbus)";
			}
			else { return ""; }
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

			var lbl_Shifter_Out = ShifterModel.OutputCalc(cycle).FormattedValue;

			PortStatus statHold_A;
			PortStatus statHold_B;

			InputsUsed(cycle, out statHold_A, out statHold_B);

			switch (bankID)
			{
				case Bank.Bank_A:
					conns.Add(new Connection(
						BusType.Data,
						DevicePort.Shifter,
						statHold_A,
						lbl_Shifter_Out,
						null,
						DevicePort.Hold_A,
						statHold_A));
					break;

				case Bank.Bank_B:
					conns.Add(new Connection(
						BusType.Data,
						DevicePort.Shifter,
						statHold_B,
						lbl_Shifter_Out,
						null,
						DevicePort.Hold_B,
						statHold_B));
					break;
			}

			return conns;
		}

		/// <summary>
		/// Inputs used by the device in the active instruction
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <param name="holdA"></param>
		/// <param name="holdB"></param>
		private static void InputsUsed(int cycle, out PortStatus holdA, out PortStatus holdB)
		{
			var busActive = BusWritten(cycle);

			holdA = busActive != null && busActive.Equals("A")
				? PortStatus.Active
				: PortStatus.Inactive;
			holdB = busActive != null && busActive.Equals("B")
				? PortStatus.Active
				: PortStatus.Inactive;
		}

		#endregion
	}
}