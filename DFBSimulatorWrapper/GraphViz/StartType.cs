using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	public class StartType
	{
		public enum StyleEnum
		{
			regular,
			self,
			random
		}

		public StyleEnum? Style;
		public string Seed;

		public StartType(StyleEnum? style, string seed)
		{
			if(style.HasValue) { Style = style; }
			Seed = seed;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}",
				Style.HasValue ? Enum.GetName(typeof(StyleEnum), Style) : "",
				Seed);
		}
	}
}
