using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using DFBSimulatorWrapper;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.IO;
using System.Drawing;
using DFBSimulatorWrapper.DFBStateModel;
using static DFBSimulatorWrapper.DFBState;
using System.Net;
using DFBSimulatorWrapper.GraphViz;

namespace DFBSimulatorWrapper.Diagram.CallDiagram
{
	/// <summary>
	/// Class for generating a GraphViz state diagram from the device models
	/// </summary>
	public class CallDiagram
	{
		#region Constants

		private string styleHeaderBGColor = Enum.GetName(typeof(KnownColor), KnownColor.LemonChiffon);
		private string styleInactiveCellBGColor = Enum.GetName(typeof(KnownColor), KnownColor.WhiteSmoke);
		private string styleKeyCellBGColor = Enum.GetName(typeof(KnownColor), KnownColor.AliceBlue);

		#endregion
		#region Members

		private GraphGeneration wrapper;

		#endregion
		#region Constructor

		public CallDiagram()
		{
			var getStartProcessQuery = new GetStartProcessQuery();
			var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
			var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

			wrapper = new GraphGeneration(getStartProcessQuery,
				getProcessStartInfoQuery,
				registerLayoutPluginCommand);
		}

		#endregion
		#region Methods

		/// <summary>
		/// Swimlane style - vertical lane per label, ACU A/B values with label
		/// </summary>
		/// <param name="dfbState"></param>
		/// <returns></returns>
		public byte[] Generate(
			DFBState dfbState)
		{
			// Graph
			GraphViz.Graph graph = CreateGraph("dfb_call");

			const double labelColWidth = 1.75f;
			const double rowHeight = 1.5f;
			double currCol_x = 0;
			double currRow_y = 0;

			// Subgraphs
			var labels = dfbState.StateFrames
				.Where(x => x.CodeStateCycle.IsJumpInstruction == true)
				.Select(x => x.CodeStateCycle.Label)
				.Distinct()
				.ToList();
			labels.ForEach(x => CreateSubgraph(graph, x));

			// Cycle through all of the jump instructions and build call sequence
			var jumpStateFrames = dfbState.StateFrames
				.Where(x => x.CodeStateCycle.IsJumpInstruction == true)
				.OrderBy(x => x.Cycle);

			foreach (var stateFrame in jumpStateFrames)
			{
				// Place all nodes for same label in one column
				var xPos = (currCol_x + (labelColWidth * stateFrame.CodeStateCycle.CodeState.State.StateNumber));

				// Place every node in an exclusive row
				var yPos = currRow_y;
				currRow_y -= rowHeight;

				AddSubgraphNode(
					graph,
					dfbState,
					stateFrame,
					xPos,
					yPos);
			}

			var text = graph.ToString();
			wrapper.RenderingEngine = Enums.RenderingEngine.Neato;
			//wrapper.RenderingEngine = Enums.RenderingEngine.Dot;            // doesn't do absolute pos

			return wrapper.GenerateGraph(text, Enums.GraphReturnType.Svg);
		}

		/// <summary>
		/// Creates a graph object with default settings
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		private GraphViz.Graph CreateGraph(
			string id)
		{
			var graph = new GraphViz.Graph(id);

			graph.GraphAttributes.splines = GraphViz.Splines.line;
			graph.GraphAttributes.fontname = "Arial";
			graph.GraphAttributes.fontsize = 8;
			graph.GraphAttributes.rankdir = GraphViz.Rankdir.TB;

			graph.DefaultNodeAttributes.fontname = "Arial";
			graph.DefaultNodeAttributes.fontsize = 8;
			graph.DefaultNodeAttributes.shape = GraphViz.Shape.none;

			graph.DefaultEdgeAttributes.fontname = "Arial";
			graph.DefaultEdgeAttributes.fontsize = 8;

			return graph;
		}

		/// <summary>
		/// Creates a Subgraph object with default settings
		/// </summary>
		/// <param name="graph"></param>
		/// <param name="label"></param>
		private static void CreateSubgraph(
			GraphViz.Graph
			graph,
			string label)
		{
			var subgraph = new GraphViz.Subgraph(label);

			subgraph.Tag = label;
			subgraph.SubgraphAttributes.label = new GraphViz.lblString(label);
			subgraph.SubgraphAttributes.color = KnownColor.SteelBlue;
			subgraph.SubgraphAttributes.style = GraphViz.Style.Cluster.filled;
			subgraph.SubgraphAttributes.fillcolor = KnownColor.WhiteSmoke;

			graph.Subgraphs.Add(subgraph);
		}

		/// <summary>
		/// Generates a node for the state label
		/// </summary>
		/// <param name="label"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public void AddSubgraphNode(
			GraphViz.Graph graph,
			DFBState dfbState,
			DFBStateFrame dfbStateFrame,
			double? pos_X,
			double? pos_Y)
		{
			// State prior to current frame gives the beginning values for ACU when label was entered
			DFBStateFrame dfbStateFrameBegValues = dfbState.StatePrior(dfbStateFrame.Cycle);

			var shapeName = dfbStateFrame.CodeStateCycle.UniqueName;

			var node = CreateNode(dfbStateFrame, pos_X, pos_Y, dfbStateFrameBegValues, shapeName);

			graph.Subgraphs
				.Where(x => x.Tag.ToString() == dfbStateFrame.CodeStateCycle.Label)
				.FirstOrDefault()
				.Nodes.Add(node);

			// Add edge for ACU change from one label call to the next
			AddEdges(graph, dfbState, dfbStateFrame, dfbStateFrameBegValues, shapeName);
		}

		/// <summary>
		/// Creates a node for the label call
		/// </summary>
		/// <param name="dfbStateFrame"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <param name="dfbStateFrameBegValues"></param>
		/// <param name="shapeName"></param>
		/// <returns></returns>
		private GraphViz.Node CreateNode(
			DFBStateFrame dfbStateFrame,
			double? pos_X, double?
			pos_Y,
			DFBStateFrame dfbStateFrameBegValues,
			string shapeName)
		{
			const int COL1MINWIDTH = 30;
			const int COL2MINWIDTH = 30;
			const int COL3MINWIDTH = 30;

			// Add Node
			var node = new GraphViz.Node(shapeName)
			{
				Tag = dfbStateFrame
			};

			node.NodeAttributes.shape = GraphViz.Shape.none;

			// tootip doesn't work in webbrowser control - it bases tooltips off of title attribute
			//node.NodeAttributes.tooltip = new GraphViz.escString(dfbStateFrame.CodeStateCycle.Instruction.InstructionText);

			if (pos_X.HasValue && pos_Y.HasValue)
			{
				node.NodeAttributes.pos = new GraphViz.Point
				{
					InputOnly = true,
					X = pos_X.Value,
					Y = pos_Y.Value
				};
			}

			var table = new HtmlTable
			{
				Border = 0
			};
			table.Attributes.Add("CELLBORDER", "1");
			table.CellSpacing = 0;

			// Header
			var row = new HtmlTableRow();
			AddHeader1Cell(row, null, 3, dfbStateFrame.CodeStateCycle.GroupName);
			table.Rows.Add(row);

			// Details
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, "Cycle:", false, true);
			AddDetailCell(row, COL2MINWIDTH, null, 2, dfbStateFrame.Cycle.ToString());
			table.Rows.Add(row);

			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, "ACU", false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, "Beg");
			AddDetailCell(row, COL3MINWIDTH, null, null, "End");
			table.Rows.Add(row);

			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, "A.reg", false, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, dfbStateFrameBegValues != null ? dfbStateFrameBegValues.ACU_A.Reg_reg.FormattedValue : "");
			AddDetailCell(row, COL2MINWIDTH, null, null, dfbStateFrame.ACU_A.Reg_reg.FormattedValue);
			table.Rows.Add(row);

			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, "B.reg", false, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, dfbStateFrameBegValues != null ? dfbStateFrameBegValues.ACU_B.Reg_reg.FormattedValue : "");
			AddDetailCell(row, COL2MINWIDTH, null, null, dfbStateFrame.ACU_B.Reg_reg.FormattedValue);
			table.Rows.Add(row);

			// Add table as label
			node.NodeAttributes.label = table.ToGraphvizLabelFormat();

			return node;
		}

		/// <summary>
		/// Adds edge connections between labels, and for the change in ACU address between calls to the same label
		/// </summary>
		/// <param name="graph"></param>
		/// <param name="dfbState"></param>
		/// <param name="dfbStateFrame"></param>
		/// <param name="dfbStateFrameBegValues"></param>
		/// <param name="shapeName"></param>
		private void AddEdges(
			GraphViz.Graph graph,
			DFBState dfbState,
			DFBStateFrame dfbStateFrame,
			DFBStateFrame dfbStateFrameBegValues,
			string shapeName)
		{
			// State prior to this label's last execution gives the prior beginning values for ACU when label was entered
			DFBStateFrame dfbStateFramePriorBegValues = null;
			var statePriorSameLabel = dfbState.StatePriorSameLabel(dfbStateFrame.Cycle);
			if (statePriorSameLabel != null)
			{
				var sameLabelPriorCycleNbr = statePriorSameLabel.Cycle;
				dfbStateFramePriorBegValues = dfbState.StatePrior(sameLabelPriorCycleNbr);
			}

			// State following current frame gives connection target
			var dfbStateFrameNext = dfbState.StateNext(dfbStateFrame.Cycle);

			if (dfbStateFrameNext == null) { return; }

			var shapeNameNext = dfbStateFrameNext.CodeStateCycle.UniqueName;

			// label
			var labels = new List<string>();
			var state = dfbStateFrame.CodeStateCycle.CodeState.State;
			var fallThrough = true;

			if (state.AcuAEQ == 1 && dfbStateFrame.JumpConditions.AcuAEQ)
			{
				labels.Add("AcuAEQ");
				fallThrough = false;
			}

			if (state.AcuBEQ == 1 && dfbStateFrame.JumpConditions.AcuBEQ)
			{
				labels.Add("AcuBEQ");
				fallThrough = false;
			}

			if (state.DpEQ == 1 && dfbStateFrame.JumpConditions.DpEQ)
			{
				labels.Add("DpEQ");
				fallThrough = false;
			}

			if (state.DpSign == 1 && dfbStateFrame.JumpConditions.DpSign)
			{
				labels.Add("DpSign");
				fallThrough = false;
			}

			if (state.DpThresh == 1 && dfbStateFrame.JumpConditions.DpThresh)
			{
				labels.Add("DpThresh");
				fallThrough = false;
			}
			if (state.In1 == 1 && dfbStateFrame.JumpConditions.In1)
			{
				labels.Add("In1");
				fallThrough = false;
			}

			if (state.In2 == 1 && dfbStateFrame.JumpConditions.In2)
			{
				labels.Add("In2");
				fallThrough = false;
			}
			if (state.Sat == 1 && dfbStateFrame.JumpConditions.Sat)
			{
				labels.Add("Sat");
				fallThrough = false;
			}

			if(fallThrough)
			{
				labels.Add("false");
			}

			var edge = new GraphViz.Edge();
			graph.Edges.Add(edge);
			edge.Connections.AddRange(new string[]
				{
					shapeName,
					shapeNameNext
				});

			edge.EdgeAttributes.style = GraphViz.Style.Edge.solid;
			edge.EdgeAttributes.color = KnownColor.ForestGreen;
			edge.EdgeAttributes.arrowhead = GraphViz.ArrowType.normal;
			edge.EdgeAttributes.arrowtail = GraphViz.ArrowType.inv;
			edge.EdgeAttributes.label = new GraphViz.escString(String.Join(" ", labels));

			// Add connection for ACU delta from prior call of same label
			if (dfbStateFramePriorBegValues != null && statePriorSameLabel != null)
			{
				// Determine ACU reg address changes from last time this label was executed
				var shapeNamePriorCall = statePriorSameLabel.CodeStateCycle.UniqueName;

				Func<int, int, string> sign = delegate (int curr, int prior)
				{
					if (prior < curr) { return "+"; }
					else { return ""; }
				};

				var sb = new StringBuilder();
				var acuABeg = (int)dfbStateFrameBegValues.ACU_A.Reg_reg.Value;
				var acuABegPrior = (int)dfbStateFramePriorBegValues.ACU_A.Reg_reg.Value;
				var acuABegChange = acuABeg - acuABegPrior;

				if (acuABegChange != 0)
				{
					sb.AppendFormat("\\nACU_A Chg: {0}{1}",
						sign(acuABeg, acuABegPrior),
						acuABegChange.ToString());
				}

				var acuBBeg = (int)dfbStateFrameBegValues.ACU_B.Reg_reg.Value;
				var acuBBegPrior = (int)dfbStateFramePriorBegValues.ACU_B.Reg_reg.Value;
				var acuBBegChange = acuBBeg - acuBBegPrior;

				if (acuBBegChange != 0)
				{
					if (acuABegChange != 0) { sb.AppendLine(""); }

					sb.AppendFormat("\\nACU_B Chg: {0}{1}",
						sign(acuBBeg, acuBBegPrior),
						acuBBegChange.ToString());
				}

				edge = new GraphViz.Edge();
				graph.Edges.Add(edge);
				edge.Connections.AddRange(new string[]
				{
					shapeNamePriorCall,
					shapeName
				});
				edge.EdgeAttributes.color = KnownColor.Gray;
				edge.EdgeAttributes.weight = 0.5f;
				edge.EdgeAttributes.style = GraphViz.Style.Edge.dashed;
				edge.EdgeAttributes.arrowhead = GraphViz.ArrowType.none;
				edge.EdgeAttributes.arrowtail = GraphViz.ArrowType.none;
				edge.EdgeAttributes.headlabel = new GraphViz.escString(sb.ToString());
				edge.EdgeAttributes.labeldistance = 0.6f;
			}
		}

		#endregion
		#region Node Helpers

		/// <summary>
		/// Create a detail cell in the provided row
		/// </summary>
		/// <param name="row"></param>
		/// <param name="width"></param>
		/// <param name="portName"></param>
		/// <param name="columnSpan"></param>
		/// <param name="text"></param>
		/// <param name="isInactive"></param>
		/// <param name="keyStyle"></param>
		private void AddDetailCell(HtmlTableRow row,
			int width,
			string portName,
			int? columnSpan,
			string text,
			bool isInactive = false,
			bool keyStyle = false)
		{
			var newCell = new HtmlTableCell
			{
				Width = width.ToString(),
				Align = "left"
			};
			newCell.Controls.Add(new LiteralControl(text));
			if (!string.IsNullOrEmpty(portName))
			{
				newCell.Attributes.Add("PORT", portName);
			}
			if (columnSpan.HasValue && columnSpan.Value > 0)
			{
				newCell.ColSpan = columnSpan.Value;
			}
			if (isInactive)
			{
				newCell.BgColor = styleInactiveCellBGColor;
			}
			if (keyStyle)
			{
				newCell.BgColor = styleKeyCellBGColor;

			}
			row.Cells.Add(newCell);
		}

		/// <summary>
		/// Add a header cell to the row with large font size
		/// </summary>
		/// <param name="row"></param>
		/// <param name="portName"></param>
		/// <param name="columnSpan"></param>
		/// <param name="text"></param>
		private void AddHeader1Cell(
			HtmlTableRow row,
			string portName,
			int? columnSpan,
			string text)
		{
			var newCell = new HtmlTableCell
			{
				Align = "middle"
			};
			var fontElem = new HtmlGenericControl("font");
			fontElem.Attributes.Add("point-size", "10");
			fontElem.InnerText = text;
			var boldElem = new HtmlGenericControl("b");
			boldElem.Controls.Add(fontElem);
			newCell.Controls.Add(boldElem);

			if (!string.IsNullOrEmpty(portName))
			{
				newCell.Attributes.Add("PORT", portName);
			}
			if (columnSpan.HasValue && columnSpan.Value > 0)
			{
				newCell.ColSpan = columnSpan.Value;
			}
			if (columnSpan.HasValue && columnSpan.Value > 0)
			{
				newCell.ColSpan = columnSpan.Value;
			}
			newCell.BgColor = styleHeaderBGColor;
			//newCell.Style.Add("font-weight", "bold");
			row.Cells.Add(newCell);
		}

		/// <summary>
		/// Add a header cell to the row with normal font size
		/// </summary>
		/// <param name="row"></param>
		/// <param name="portName"></param>
		/// <param name="columnSpan"></param>
		/// <param name="text"></param>
		private void AddHeader2Cell(
			HtmlTableRow row,
			string portName,
			int? columnSpan,
			string text)
		{
			var newCell = new HtmlTableCell
			{
				Align = "middle"
			};

			//var boldElem = new HtmlGenericControl("b");
			//boldElem.InnerText = text;
			//newCell.Controls.Add(boldElem);
			newCell.Controls.Add(new LiteralControl(text));

			if (!string.IsNullOrEmpty(portName))
			{
				newCell.Attributes.Add("PORT", portName);
			}
			if (columnSpan.HasValue && columnSpan.Value > 0)
			{
				newCell.ColSpan = columnSpan.Value;
			}
			if (columnSpan.HasValue && columnSpan.Value > 0)
			{
				newCell.ColSpan = columnSpan.Value;
			}
			newCell.BgColor = styleHeaderBGColor;

			newCell.Style.Add("font-weight", "bold");
			row.Cells.Add(newCell);
		}

		#endregion
	}
}
