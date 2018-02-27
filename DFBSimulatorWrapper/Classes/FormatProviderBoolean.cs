using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper
{
	public class FormatProviderBoolean : IFormatProvider, ICustomFormatter
	{
		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			arg = arg ?? false;
			bool value = (bool)arg;
			format = (format == null ? null : format.Trim().ToLower());

			switch (format.ToLower())
			{
				case "yesno":
					return value ? "Yes" : "No";
				case "yes":
					return value ? "Yes" : "";
				case "yn":
					return value ? "Y" : "N";
				case "y":
					return value ? "Y" : "";
				case "truefalse":
					return value ? "True" : "False";
				case "true":
					return value ? "True" : "";
				case "tf":
					return value ? "T" : "F";
				case "t":
					return value ? "T" : "";
				case "x":
					return value ? "X" : "";
				default:
					if (arg is IFormattable)
						return ((IFormattable)arg).ToString(format, formatProvider);
					else
						return arg.ToString();
			}
		}

		public object GetFormat(Type formatType)
		{
			if (formatType == typeof(ICustomFormatter))
				return this;
			else
				return null;
		}
	}
}
