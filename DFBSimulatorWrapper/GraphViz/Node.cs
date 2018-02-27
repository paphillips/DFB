using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	public class Node
	{
		public readonly string ID;

		public object Tag;

		public readonly NodeAttributes NodeAttributes;

		public int Indentation;

		public Node(string id)
		{
			NodeAttributes = new NodeAttributes();
			ID = id;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendFormat("{0}{1} {2}", new string('\t', Indentation), ID, NodeAttributes.ToString());
			return sb.ToString();
		}
	}
}
