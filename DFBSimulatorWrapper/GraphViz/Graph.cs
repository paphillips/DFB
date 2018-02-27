using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	/// <summary>
	/// Supports digraph only
	/// </summary>
	public class Graph
	{
		public readonly string ID;

		public object Tag;

		public readonly GraphAttributes GraphAttributes;

		public readonly NodeAttributes DefaultNodeAttributes;

		public readonly EdgeAttributes DefaultEdgeAttributes;

		public readonly List<Node> Nodes;

		public readonly List<Edge> Edges;

		public readonly List<Subgraph> Subgraphs;

		public Graph(string id)
		{
			ID = id;
			GraphAttributes = new GraphAttributes();
			DefaultNodeAttributes = new NodeAttributes();
			DefaultEdgeAttributes = new EdgeAttributes();

			Nodes = new List<Node>();
			Edges = new List<Edge>();
			Subgraphs = new List<Subgraph>();
		}

		public override string ToString()
		{
			var Indentation = 0;
			var sb = new StringBuilder();
			sb.AppendFormat("digraph {0}\n", ID);
			sb.AppendLine("{");

			var graphAttributes = GraphAttributes.ToString();
			var defaultNodeAttributes = DefaultNodeAttributes.ToString();
			var defaultEdgeAttributes = DefaultEdgeAttributes.ToString();

			if (graphAttributes.Length > 4) { sb.AppendFormat("{0}graph {1}", new String('\t', Indentation + 1), graphAttributes); }
			if (defaultNodeAttributes.Length > 4) { sb.AppendFormat("{0}node {1}", new String('\t', Indentation + 1), defaultNodeAttributes); }
			if (defaultEdgeAttributes.Length > 4) { sb.AppendFormat("{0}edge {1}", new String('\t', Indentation + 1), defaultEdgeAttributes); }
			sb.AppendLine("");

			foreach(var node in Nodes)
			{
				node.Indentation = 1;
				sb.Append(node.ToString());
			}
			if (Nodes.Count > 0) { sb.AppendLine(); }

			foreach (var subgraph in Subgraphs)
			{
				subgraph.Indentation = 1;
				sb.Append(subgraph.ToString());
			}
			if (Subgraphs.Count > 0) { sb.AppendLine(); }

			foreach (var edge in Edges)
			{
				edge.Indentation = 1;
				sb.Append(edge.ToString());
			}

			sb.AppendLine("}");

			return sb.ToString();
		}
	}
}
