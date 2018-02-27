using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	public class Adddouble
	{
		public double Value;

		public bool HasPlusPrefix;

		public Adddouble(double value, bool hasPlusPrefix)
		{
			Value = value;
			HasPlusPrefix = hasPlusPrefix;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}", HasPlusPrefix ? "+" : "", Value);
		}
	}
}
