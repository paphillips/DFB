using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	public class Subgraph
	{
		public readonly string ID;

		public object Tag;

		public readonly SubgraphAttributes SubgraphAttributes;

		public readonly NodeAttributes DefaultNodeAttributes;

		public readonly EdgeAttributes DefaultEdgeAttributes;

		public readonly List<Node> Nodes;

		public readonly List<Edge> Edges;

		public readonly List<Subgraph> Subgraphs;

		public int Indentation;

		public Subgraph(string id)
		{
			SubgraphAttributes = new SubgraphAttributes();
			DefaultNodeAttributes = new NodeAttributes();
			DefaultEdgeAttributes = new EdgeAttributes();
			Nodes = new List<Node>();
			Edges = new List<Edge>();
			Subgraphs = new List<Subgraph>();

			if (!id.ToUpperInvariant().StartsWith("CLUSTER_"))
			{
				ID = string.Format("cluster_{0}", id);
			}
			else
			{
				ID = id;
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendFormat("{0}subgraph {1} {{\n", new string('\t', Indentation), ID);
			
			var subgraphAttributes = SubgraphAttributes.ToString();
			var defaultNodeAttributes = DefaultNodeAttributes.ToString();
			var defaultEdgeAttributes = DefaultEdgeAttributes.ToString();

			if (subgraphAttributes.Length > 4) { sb.AppendFormat("{0}graph {1}", new String('\t', Indentation + 1), subgraphAttributes); }
			if (defaultNodeAttributes.Length > 4) { sb.AppendFormat("{0}node {1}", new String('\t', Indentation + 1), defaultNodeAttributes); }
			if (defaultEdgeAttributes.Length > 4) { sb.AppendFormat("{0}edge {1}", new String('\t', Indentation + 1), defaultEdgeAttributes); }

			foreach (var node in Nodes)
			{
				node.Indentation = Indentation + 1;
				sb.Append(node.ToString());
			}
			foreach (var edge in Edges)
			{
				edge.Indentation = Indentation + 1;
				sb.Append(edge.ToString());
			}
			foreach (var subgraph in Subgraphs)
			{
				subgraph.Indentation = Indentation + 1;
				sb.Append(subgraph.ToString());
			}

			sb.AppendFormat("{0}}}\n", new string('\t', Indentation));

			return sb.ToString();
		}
	}
}
