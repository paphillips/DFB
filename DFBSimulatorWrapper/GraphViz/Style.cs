using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	public class Style
	{
		public enum Node
		{
			dashed,
			dotted,
			solid,
			invis,
			bold,
			rounded,
			diagonals,
			filled,
			striped,
			wedged,
			radial,
		}

		public enum Edge
		{
			dashed,
			dotted,
			solid,
			invis,
			bold,
			tapered,
		}

		public enum Cluster
		{
			solid,
			dashed,
			dotted,
			bold,
			rounded,
			filled,
			striped,
			radial,
		}
	}
}
