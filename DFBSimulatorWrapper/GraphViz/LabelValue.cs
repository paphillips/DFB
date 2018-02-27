using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	public abstract class LabelValue
	{
		public abstract void AppendNameValue(StringBuilder sb, string variableName);
	}
}
