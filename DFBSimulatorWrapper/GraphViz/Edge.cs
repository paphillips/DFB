using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	public class Edge
	{
		public object Tag;

		public readonly List<string> Connections;

		public readonly EdgeAttributes EdgeAttributes;

		public int Indentation;

		public Edge()
		{
			Connections = new List<string>();
			EdgeAttributes = new EdgeAttributes();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendFormat("{0}{1} {2}", new string('\t', Indentation), string.Join(" -> ", Connections), EdgeAttributes.ToString());
			return sb.ToString();
		}
	}
}
