using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFB_v1_40.Asm;
using DFB_v1_40.Simulator;
using DFBSimulatorWrapper;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;

namespace DFBSimulatorWrapper.DFBStateModel
{
	public class PortPosition
	{
		public enum Position
		{
			north,
			northeast,
			east,
			southeast,
			south,
			southwest,
			west,
			northwest,
			center,
			auto,
		}

		public Position Pos;
		public PortPosition(Position portPosition)
		{
			this.Pos = portPosition;
		}

		public override string ToString()
		{
			switch (Pos)
			{
				case Position.auto:
					return "_";
				case Position.center:
					return "c";
				case Position.east:
					return "e";
				case Position.north:
					return "n";
				case Position.northeast:
					return "ne";
				case Position.northwest:
					return "nw";
				case Position.south:
					return "s";
				case Position.southeast:
					return "se";
				case Position.southwest:
					return "sw";
				case Position.west:
					return "w";
				default:
					return "";
			}
		}
	}

	/// <summary>
	/// A ram item display for databinding
	/// </summary>
	public class RamItem
	{

		private static Regex numberNoComment;

		public enum Area
		{
			data_a,
			data_b,
			acu_a,
			acu_b
		}

		private string address;
		public string Address
		{
			get { return address; }
			set { address = value; }
		}

		private string valHex;
		public string ValHex
		{
			get { return valHex; }
			set { valHex = value; }
		}

		private string valDfb;
		public string ValDfb
		{
			get { return valDfb; }
			set { valDfb = value; }
		}

		private string valInt;
		public string ValInt
		{
			get { return valInt; }
			set { valInt = value; }
		}

		private string comment;
		public string Comment
		{
			get { return comment; }
			set { comment = value; }
		}
		
		/// <summary>
		/// Parses the code and returns a list of code comments corresponding to each area index position
		/// </summary>
		/// <param name="area"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		public static List<string> RamCodeComments(Area area, string code)
		{
			if(numberNoComment == null) { numberNoComment = new Regex(@"\d+([^\/]+)*\b"); }

			var rtnList = new List<string>();
			if (string.IsNullOrEmpty(code)) { return rtnList; }

			var codeLines = code.Split('\n');
			string sectionStart = null;

			var ramLen = 0;
			switch (area)
			{
				case Area.acu_a:
				case Area.acu_b:
					sectionStart = "area acu";
					ramLen = 16;
					for (int i = 0; i < ramLen; i++) { rtnList.Add(""); }
					break;

				case Area.data_a:
					sectionStart = "area data_a";
					ramLen = 128;
					for (int i = 0; i < ramLen; i++) { rtnList.Add(""); }
					break;

				case Area.data_b:
					sectionStart = "area data_b";
					ramLen = 128;
					for (int i = 0; i < ramLen; i++) { rtnList.Add(""); }
					break;
			}

			bool sectionActive = false;
			bool orgActive = false;
			bool orgChanged = false;
			int orgIndex = 0;
			int ramIndex = 0;
			for (int i = 0; i < codeLines.Count(); i++)
			{
				// area and org may be in parens or not - both are legal
				var line = codeLines[i].Replace("(", "").Replace(")", "");

				// Latch when section found
				if (line.StartsWith(sectionStart, StringComparison.InvariantCultureIgnoreCase))
				{
					sectionActive = true;
					continue;
				}
				if (line.StartsWith("org"))
				{
					// remove comments if any
					
					orgActive = true;
					orgChanged = true;
					orgIndex = int.Parse(numberNoComment.Match(line).Value);
					continue;
				}

				if (sectionActive && orgActive)
				{
					// advance to the specified org location if it changed
					if (orgChanged)
					{
						for (ramIndex = 0; ramIndex < ramLen; ramIndex++)
						{
							if (ramIndex == orgIndex) { break; }
						}
						orgChanged = false;
					}

					if (line.StartsWith("dw"))
					{
						// find comment
						var commentStartIdx = line.IndexOf("//");
						if (commentStartIdx > 0)
						{
							string newItem = null;
							if ((area == Area.acu_a || area == Area.acu_b) && line.Contains("|"))
							{
								var split = line.Substring(commentStartIdx, line.Length - commentStartIdx).Split('|');
								// split comment with pipe character for acu
								if (area == Area.acu_a)
								{
									newItem = split[0].Replace("\r", "");
								}
								else if (area == Area.acu_b && split.Length > 0)
								{
									newItem = "// " + split[1].Replace("\r", "");
								}
							}
							else
							{
								newItem = line.Substring(commentStartIdx, line.Length - commentStartIdx).Replace("\r", "");
							}

							rtnList[ramIndex] = newItem;
						}
						ramIndex += 1;
					}

					// break if we reach another area section
					if (line.StartsWith("area")) { break; }

					// break if we reach a code label (areas may be declared above or below asm code)
					var comment2StartIdx = line.IndexOf("//");
					string lineBeginning = line;
					if (comment2StartIdx > 0)
					{
						lineBeginning = line.Substring(0, comment2StartIdx);
					}
					if (line.Contains(":")) { break; }
				}
			}

			return rtnList;
		}
	}

	public class EdgeColor
	{
		public readonly KnownColor Color;
		public readonly decimal? Ratio;

		public EdgeColor(KnownColor color, decimal? ratio)
		{
			if (ratio < 0 || ratio > 1) { throw new ArgumentException("EdgeColorList ratio must be between - and 1"); }

			Color = color;
			Ratio = ratio;
		}
	}

	public enum DirType
	{
		forward,
		back,
		both,
		none
	}

	/// <summary>
	/// Represents a connection between two DFB hardware areas
	/// </summary>
	public class Connection
	{
		public readonly BusType BusType;

		public readonly DevicePort From;
		public readonly PortStatus FromPortActive;

		public readonly string FromPortActiveText;
		public readonly string FromPortInactiveText;

		public readonly DevicePort To;
		public readonly PortStatus ToPortActive;

		//public PortPosition FromPortPosition;
		//public PortPosition ToPortPosition;
		//public double? Weight;
		//public double? Length;
		//public double? EdgePos_X;
		//public double? EdgePos_Y;
		//public List<EdgeColor> EdgeColorList;
		//public decimal? Penwidth;
		//public DirType? DirType;

		public Connection(
			BusType busType,
			DevicePort from,
			PortStatus fromPortActive,
			string fromPortActiveLabel,
			string fromPortInactiveLabel,
			DevicePort to,
			PortStatus toPortActive)
		{
			BusType = busType;

			From = from;
			FromPortActive = fromPortActive;
			FromPortActiveText = fromPortActiveLabel;
			FromPortInactiveText = fromPortInactiveLabel;

			To = to;
			ToPortActive = toPortActive;

			//EdgeColorList = new List<EdgeColor>();
		}
	}

	public enum BusType
	{
		Address,
		Data
	}

	public enum DevicePort
	{
		Default,
		Stage_A,
		Stage_B,
		Mux_0,
		Mux_1A,
		Mux_1B,
		DataRam_A_Addr,
		DataRam_B_Addr,
		DataRam_A,
		DataRam_B,
		Shifter,
		Mux_2A,
		Mux_2B,
		MAC,
		Mux_3A,
		Mux_3B,
		ACU_A,
		ACU_B,
		ALU,
		ALUIn_A,
		ALUIn_B,
		ALUOut,
		Hold_A,
		Hold_B,
		Global,
		JumpConditions
	}

	public enum PortStatus
	{
		Active,
		Inactive
	}

	public enum Bank
	{
		NotApplicable,
		Bank_A,
		Bank_B
	}

	public enum DFBValueFormat
	{
		Hex,
		q23Decimal,
		Int
	}

	public abstract class DFBModelBase
	{
		#region Private Members

		private Bank bankID;
		private int cycleActive;
		private int dataValueWidth;
		private int dataAddressWidth;
		private int pipelineDelay;
		private List<int> cycles;
		protected string name;
		protected List<string> instructions;
		protected List<Connection> connectionInputs;
		protected LabeledValue<long?> output;

		#endregion
		#region Public Members

		/// <summary>
		/// Name of device
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
		}

		/// <summary>
		/// Number of bytes to display for values
		/// </summary>
		public int DataValueWidth
		{
			get
			{
				return dataValueWidth;
			}
		}

		/// <summary>
		/// Number of bytes to display for addresses
		/// </summary>
		public int DataAddressWidth
		{
			get
			{
				return dataAddressWidth;
			}
		}

		/// <summary>
		/// Bank ID of this instance
		/// </summary>
		public LabeledValue<Bank> BankID
		{
			get
			{
				var r = new LabeledValue<Bank>("Bank:");
				r.Value = bankID;
				r.FormattedValue = bankID.ToString();
				return r;
			}
		}

		/// <summary>
		/// Number of instructions each command is delayed for this device due to pipelining
		/// </summary>
		public LabeledValue<int> PipelineDelay
		{
			get
			{
				var r = new LabeledValue<int>("Pipeline Delay:");
				r.Value = pipelineDelay;
				r.FormattedValue = pipelineDelay.ToString();
				return r;
			}
		}

		/// <summary>
		/// Returns a list of cycle headers, e.g. [now], [t-1], [t-2]
		/// </summary>
		public LabeledValue<List<string>> Pipeline
		{
			get
			{
				var rtnList = new LabeledValue<List<string>>("Pipeline:");
				rtnList.Value = new List<string>();
				for (int i = pipelineDelay; i >= 0; i--)
				{
					rtnList.Value.Add(FormatPipelineLable(-i));
				}
				return rtnList;
			}
		}

		/// <summary>
		/// Cycles in the relevant device pipeline. Last is current cycle.
		/// </summary>
		public List<LabeledValue<int?>> Cycles
		{
			get
			{
				var rtnList = new List<LabeledValue<int?>>();
				foreach (var cycle in cycles)
				{
					var r = new LabeledValue<int?>("Cycle:");
					r.Value = cycle >= 0 ? (int?)cycle : null;
					r.FormattedValue = r.Value.ToString();
					rtnList.Add(r);
				}
				return rtnList;
			}
		}

		/// <summary>
		/// Cycle that is active given the device's pipeline
		/// </summary>
		public LabeledValue<int> CycleActive
		{
			get
			{
				var lastCycle = cycles.Last();

				var rtn = new LabeledValue<int>("Cycle Active:");
				rtn.Value = lastCycle;
				rtn.FormattedValue = lastCycle.ToString();

				return rtn;
			}
		}

		/// <summary>
		/// Instructions applicable to this instance. Last item is active instruction based on pipeline delay.
		/// </summary>
		public List<LabeledValue<string>> Instructions
		{
			get
			{
				var rtnList = new List<LabeledValue<string>>();
				foreach (var instr in instructions)
				{
					var r = new LabeledValue<string>("Instr:");
					r.Value = instr;
					r.FormattedValue = instr;
					rtnList.Add(r);
				}
				return rtnList;
			}
		}

		/// <summary>
		/// Input connections to the device
		/// </summary>
		public List<Connection> ConnectionInputs
		{
			get
			{
				return connectionInputs;
			}
		}

		/// <summary>
		/// Output value of this instance
		/// </summary>
		public LabeledValue<long?> Output
		{
			get
			{
				return output;
			}
		}

		#endregion
		#region Constructor

		/// <summary>
		/// Create instance of this base class
		/// </summary>
		/// <param name="bankID">BankID of the device, where applicable</param>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <param name="dataValueWidth">Number of hex characters in data value</param>
		/// <param name="dataAddressWidth">Number of hex characters in address value</param>
		/// <param name="pipelineDelay">Number of instructions of pipeline delay for the device</param>
		public DFBModelBase(
			Bank bankID,
			int cycle,
			int dataValueWidth,
			int dataAddressWidth,
			int pipelineDelay)
		{
			this.bankID = bankID;
			this.cycleActive = cycle - pipelineDelay;
			this.dataValueWidth = dataValueWidth;
			this.dataAddressWidth = dataAddressWidth;
			this.pipelineDelay = pipelineDelay;

			// Add cycles from current cycle back [pipelineDelay] times
			this.cycles = new List<int>();

			for (int i = 0; i < pipelineDelay + 1; i++)
			{
				cycles.Add(cycle - i);
			}
			this.instructions = new List<string>();
			this.connectionInputs = new List<Connection>();
		}

		#endregion
		#region Methods

		/// <summary>
		/// Formats the cycle number for a label, e.g. "[now]", "[t-1]", "[t+1]"
		/// </summary>
		/// <param name="cycleOffset"></param>
		/// <returns></returns>
		private static string FormatPipelineLable(int cycleOffset)
		{
			if (cycleOffset == 0)
			{
				return "[now]:";
			}
			else if (cycleOffset < 0)
			{
				return string.Format("[t{0}]", cycleOffset);
			}
			else
			{
				return string.Format("[t+{0}]", cycleOffset);
			}
		}

		/// <summary>
		/// Returns a labeled value for a nullable type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="label"></param>
		/// <returns></returns>
		public static LabeledValue<T> NullLabeledValue<T>(string label)
		{
			var r = new LabeledValue<T>(label);
			r.Value = default(T);
			r.FormattedValue = "";
			return r;
		}

		/// <summary>
		/// Converts a flag field to a true/false value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string FormatFlag(object value)
		{
			return Convert.ToBoolean(value).ToString();
		}

		/// <summary>
		/// Converts an integer field to a string value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string FormatIntegral(long? value)
		{
			if (value.HasValue)
			{
				return value.Value.ToString("N0");
			}
			else
			{
				return "";
			}
		}

		/// <summary>
		/// Returns a formatted value string per the active DFBState Value Format
		/// </summary>
		/// <param name="byteCount"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string FormatValue(int byteCount, long? value)
		{
			return Formatter(byteCount, value, DFBState.ValueFormat);
		}

		/// <summary>
		/// Returns an address string in hex format
		/// </summary>
		/// <param name="byteCount"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string FormatHex(int byteCount, long? value)
		{
			return Formatter(byteCount, value, DFBValueFormat.Hex);
		}

		/// <summary>
		/// Returns a value in DFB Decimal format
		/// </summary>
		/// <param name="byteCount"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string FormatDfbDecimal(int byteCount, long? value)
		{
			return Formatter(byteCount, value, DFBValueFormat.q23Decimal);
		}

		/// <summary>
		/// Converts the value tot he specified representation
		/// </summary>
		/// <param name="numberOfPlaces">Number of characters to show in the output</param>
		/// <param name="value">Value to convert</param>
		/// <param name="valueFormat">Format to convert to</param>
		/// <returns></returns>
		private static string Formatter(int numberOfPlaces, long? value, DFBValueFormat valueFormat)
		{
			if (!value.HasValue) { return ""; }

			// Build string based on format identifier
			switch (valueFormat)
			{
				case DFBValueFormat.Hex:
					return SignedInt32_To_Hex((int)value.Value, numberOfPlaces);

				case DFBValueFormat.q23Decimal:
					return SignedInt32_To_DFB_Q23((int)value.Value).ToString("N7");

				default:
					throw new NotImplementedException(string.Format("DFBValueFormat: {0}", valueFormat));
			}
		}

		#endregion
		#region Integer, Hex, and Q.23 converters

		/*
			DFB Val            Dec       Hex
			--------------------------------
			+0.9999999 	 8,388,607	0x7FFFFF
			+0.0000001 	         1	0x000001
				0.0000000		   	 0	0x000000
			-0.0000001	16,777,215 	0xFFFFFF
			-1.0000000	 8,388,608 	0x800000
		*/

		/// <summary>
		/// Convert a signed integer to hex format, DFB min/max and rules applied
		/// </summary>
		/// <param name="intValue">Value to convert</param>
		/// <param name="numberOfPlaces">Number of characters to show in the output</param>
		/// <returns></returns>
		public static string SignedInt32_To_Hex(Int32 intValue, int numberOfPlaces)
		{
			var formatString = "{0:X" + numberOfPlaces.ToString() + "}";
			var outVal = string.Format(formatString, intValue);
			outVal = outVal.Substring(outVal.Length - numberOfPlaces, numberOfPlaces);
			return "0x" + outVal;
		}

		/// <summary>
		/// Convert a signed integer to DFB q.23 format, DFB min/max and rules applied
		/// </summary>
		/// <param name="intValue">Value to convert</param>
		/// <returns></returns>
		public static decimal SignedInt32_To_DFB_Q23(Int32 intValue)
		{
			const decimal root_two_23rd = 0.00000011920928955078100000m;
			const int round_places = 7;

			return Math.Round(intValue * root_two_23rd, round_places);
		}

		/// <summary>
		/// Convert a DFB q.23 value to hex format, DFB min/max and rules applied
		/// </summary>
		/// <param name="dfb_q23Value">Value to convert</param>
		/// <param name="numberOfPlaces">Number of characters to show in the output</param>
		/// <returns></returns>
		public static string DFB_Q23_To_Hex(decimal dfb_q23Value, int numberOfPlaces)
		{
			var intVal = DFB_Q23_To_SignedInt32(dfb_q23Value);

			return SignedInt32_To_Hex(intVal, numberOfPlaces);
		}

		/// <summary>
		/// Convert a DFB q.23 value to singed integer format, DFB min/max and rules applied
		/// </summary>
		/// <param name="dfb_q23Value">Value to convert</param>
		/// <returns></returns>
		public static Int32 DFB_Q23_To_SignedInt32(decimal dfb_q23Value)
		{
			const int multiplier = 8388608; // 2^23
			const int precision = 0;

			return (Int32)Math.Round(dfb_q23Value * multiplier, precision);
		}

		/// <summary>
		/// Converts 6-digit hex string (0x prefix can also be included) to signed Int32 with sign extension
		/// </summary>
		/// <param name="hexString">Value to convert</param>
		/// <returns></returns>
		public static Int32 Hex6_To_SignedInt32(string hexString)
		{
			// remove any prefix
			hexString = hexString.ToUpper().Replace("0X", "");
			var hexStringLen = hexString.Length;

			// sign extend since we will be converting to two's complement Int32 signed value
			if (hexStringLen < 32 / 4)
			{
				// negative last bit?
				int num = Int32.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
				var shift = hexStringLen * 4 - 1;
				if ((num >> shift) == 1)
				{
					hexString = hexString.PadLeft(8, 'F');
				}
			}

			return Int32.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
		}

		/// <summary>
		/// Convert a hex value to DFB q.23 format, DFB min/max and rules applied
		/// </summary>
		/// <param name="hexString">Value to convert</param>
		/// <returns></returns>
		public static decimal Hex_To_DFB_Q23(string hexString)
		{
			var precision = 8;
			var intVal = Hex6_To_SignedInt32(hexString);
			decimal step = (decimal)Math.Pow(10, precision);
			// ToDo: Review calc. It should only be ncessary to divide intVal by 
			// 0x7FFFFF (8388607m) for both cases but this doesn't match the datasheet pg 25
			if (intVal > 0)
			{
				return Math.Truncate(step * intVal / 8388607.5m) / step;
			}
			else
			{
				return Math.Truncate(step * intVal / 8388608m) / step;
			}
		}

		#endregion
	}
}
