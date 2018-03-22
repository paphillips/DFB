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

namespace DFBSimulatorWrapper.Diagram.StateDiagram
{
	/// <summary>
	/// Class for generating a GraphViz state diagram from the device models
	/// </summary>
	public class StateDiagram
	{
		#region Constants

		private const bool debugShowHiddenNodes = false;

		private string styleHeaderBGColor = Enum.GetName(typeof(KnownColor), KnownColor.LemonChiffon);
		private string styleInactiveCellBGColor = Enum.GetName(typeof(KnownColor), KnownColor.WhiteSmoke);
		private string styleKeyCellBGColor = Enum.GetName(typeof(KnownColor), KnownColor.AliceBlue);

		private KnownColor styleColor_DataBus_Active = KnownColor.Blue;
		private KnownColor styleColor_AddressBus_Active = KnownColor.DarkGreen;
		private KnownColor styleColor_DataBus_Inactive = KnownColor.Blue;
		private KnownColor styleColor_AddressBus_Inactive = KnownColor.DarkGreen;

		private KnownColor styleColor_Bus_Inactive = KnownColor.Gray;
		private double stylePenWidth_Bus_Active = 3.0;

		#endregion
		#region Members

		private GraphGeneration wrapper;

		#endregion
		#region Constructor

		public StateDiagram()
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
		/// Generate a state frame diagram svg node
		/// </summary>
		/// <param name="stateFrame"></param>
		/// <returns>Byte array of the state frame diagram svg node</returns>
		public byte[] Generate(DFBStateFrame stateFrame)
		{
			// fonts: http://wingraphviz.sourceforge.net/wingraphviz/language/fontname.htm

			var graph = CreateGraph("dfb");

			var sb = new StringBuilder();

			// Nodes
			double positionMultiplier = 1d;
			double posY_center = 24d * positionMultiplier;
			double posXWidth = 3.5d * positionMultiplier;

			double legend_X = 3.5d;
			double legend_Y = 8.0d;

			double rowOffset1 = 0.0d;
			double rowOffset2 = rowOffset1 + 3.25d;
			double rowOffset3 = rowOffset2 + 3.0d;

			double rowOffset_Stage = rowOffset2;
			double rowOffset_Mux0 = rowOffset1;
			double rowOffset_Mux1 = rowOffset2;
			double rowOffset_ACU = rowOffset3 + 0.5d;
			double rowOffset_DataRam = rowOffset3;
			double rowOffset_Mux2 = rowOffset2;
			double rowOffset_Mux3 = rowOffset2;
			double rowOffset_ALU = rowOffset1;
			double rowOffset_Shifter = rowOffset1;
			double rowOffset_Hold = rowOffset2;
			double rowOffset_Global = rowOffset3 + .60d;

			double colpos_Stage_Mux0 = 3.5d;
			double colpos_Mux1 = colpos_Stage_Mux0 + 3.5d;
			double colpos_ACU = colpos_Mux1 + 1.6d;
			double colpos_DataRamMux2 = colpos_ACU + 2.85d;
			double colpos_MAC = colpos_DataRamMux2 + 2.5d;
			double colpos_Mux3 = colpos_MAC + 2.5d;
			double colpos_ALU = colpos_Mux3 + 3.25d;
			double colpos_Global = colpos_ALU;
			double colpos_Shifter = colpos_ALU + 5.25d;
			double colpos_Hold = colpos_Shifter + 0.75d;

			// Hidden spacers to force routing of shifter to mux2 edges
			double shifterABuffer_width = 1.0d;
			double shifterABuffer_height = 4.25d;
			double rowOffset_ShifterA_buffer = rowOffset_Shifter + 1.0d + shifterABuffer_height / 2d;
			double colpos_ShifterA_buffer = colpos_Shifter - 1.9d;

			double shifterBBuffer_width = 1.0d;
			double shifterBBuffer_height = 4.25d;
			double rowOffset_ShifterB_buffer = rowOffset_Shifter + 1.0d + shifterBBuffer_height / 2d;
			double colpos_ShifterB_buffer = colpos_Shifter - 1.9d;

			Legend(graph, legend_X, posY_center + legend_Y);

			StageBusIn(graph, stateFrame.Stage_A, colpos_Stage_Mux0, posY_center + rowOffset_Stage);
			Mux_0(graph, stateFrame.Mux0, colpos_Stage_Mux0, posY_center + rowOffset_Mux0);
			StageBusIn(graph, stateFrame.Stage_B, colpos_Stage_Mux0, posY_center - rowOffset_Stage);

			Mux_1(graph, stateFrame.Mux_1A, colpos_Mux1, posY_center + rowOffset_Mux1);
			Mux_1(graph, stateFrame.Mux_1B, colpos_Mux1, posY_center - rowOffset_Mux1);

			ACU(graph, stateFrame.ACU_A, colpos_ACU, posY_center + rowOffset_ACU);
			ACU(graph, stateFrame.ACU_B, colpos_ACU, posY_center - rowOffset_ACU);

			DataRam(graph, stateFrame.DataRam_A, colpos_DataRamMux2, posY_center + rowOffset_DataRam);
			Mux_2(graph, stateFrame.Mux_2A, colpos_DataRamMux2, posY_center + rowOffset_Mux2);
			Mux_2(graph, stateFrame.Mux_2B, colpos_DataRamMux2, posY_center - rowOffset_Mux2);
			DataRam(graph, stateFrame.DataRam_B, colpos_DataRamMux2, posY_center - rowOffset_DataRam);

			MAC(graph, stateFrame.MAC, colpos_MAC, posY_center);

			Mux_3(graph, stateFrame.Mux_3A, colpos_Mux3, posY_center + rowOffset_Mux3);
			Mux_3(graph, stateFrame.Mux_3B, colpos_Mux3, posY_center - rowOffset_Mux3);

			// Hidden nodes to push connectors from shifter up/down so they don't overlap
			var shifterAHiddenNode = HiddenNode("shftAbuffer", colpos_ShifterA_buffer, posY_center + rowOffset_ShifterA_buffer, shifterABuffer_width, shifterABuffer_height);
			var shifterAHiddenEdge = new Edge();
			shifterAHiddenEdge.Connections.Add(shifterAHiddenNode.ID);
			shifterAHiddenEdge.Connections.Add(DevicePort.Mux_3A.ToString());
			shifterAHiddenEdge.EdgeAttributes.style = debugShowHiddenNodes ? Style.Edge.solid : Style.Edge.invis;
			graph.Nodes.Add(shifterAHiddenNode);
			graph.Edges.Add(shifterAHiddenEdge);

			var shifterBHiddenNode = HiddenNode("shftBbuffer", colpos_ShifterB_buffer, posY_center - rowOffset_ShifterB_buffer, shifterBBuffer_width, shifterBBuffer_height);
			var shifterBHiddenEdge = new Edge();
			shifterBHiddenEdge.Connections.Add(shifterBHiddenNode.ID);
			shifterBHiddenEdge.Connections.Add(DevicePort.Mux_3B.ToString());
			shifterBHiddenEdge.EdgeAttributes.style = debugShowHiddenNodes ? Style.Edge.solid : Style.Edge.invis;
			graph.Nodes.Add(shifterBHiddenNode);
			graph.Edges.Add(shifterBHiddenEdge);

			ALU(graph, stateFrame.ALU, colpos_ALU, posY_center + rowOffset_ALU);

			Shifter(graph, stateFrame.Shifter, colpos_Shifter, posY_center + rowOffset_Shifter);

			HoldBusOut(graph, stateFrame.Hold_A, colpos_Hold, posY_center + rowOffset_Hold);
			HoldBusOut(graph, stateFrame.Hold_B, colpos_Hold, posY_center - rowOffset_Hold);

			Global(graph, stateFrame.Global, colpos_Global, posY_center - rowOffset_Global);

			// Connections
			stateFrame.Stage_A.ConnectionInputs.ForEach(x => AddConnection(graph, x));
			stateFrame.Stage_B.ConnectionInputs.ForEach(x => AddConnection(graph, x));

			stateFrame.Mux0.ConnectionInputs.ForEach(x => AddConnection(graph, x));

			stateFrame.ACU_A.ConnectionInputs.ForEach(x => AddConnection(graph, x));
			stateFrame.ACU_B.ConnectionInputs.ForEach(x => AddConnection(graph, x));

			stateFrame.Mux_1A.ConnectionInputs.ForEach(x => AddConnection(graph, x));
			stateFrame.Mux_1B.ConnectionInputs.ForEach(x => AddConnection(graph, x));

			stateFrame.DataRam_A.ConnectionInputs.ForEach(x => AddConnection(graph, x));
			stateFrame.DataRam_B.ConnectionInputs.ForEach(x => AddConnection(graph, x));

			stateFrame.Mux_2A.ConnectionInputs.ForEach(x => AddConnection(graph, x));
			stateFrame.Mux_2B.ConnectionInputs.ForEach(x => AddConnection(graph, x));

			stateFrame.MAC.ConnectionInputs.ForEach(x => AddConnection(graph, x));

			stateFrame.Mux_3A.ConnectionInputs.ForEach(x => AddConnection(graph, x));
			stateFrame.Mux_3B.ConnectionInputs.ForEach(x => AddConnection(graph, x));

			stateFrame.ALU.ConnectionInputs.ForEach(x => AddConnection(graph, x));

			stateFrame.Shifter.ConnectionInputs.ForEach(x => AddConnection(graph, x));

			stateFrame.Hold_A.ConnectionInputs.ForEach(x => AddConnection(graph, x));
			stateFrame.Hold_B.ConnectionInputs.ForEach(x => AddConnection(graph, x));

			stateFrame.Global.ConnectionInputs.ForEach(x => AddConnection(graph, x));

			wrapper.RenderingEngine = Enums.RenderingEngine.Neato;
			/*																
			wrapper.RenderingEngine = Enums.RenderingEngine.Fdp;        // Ok, portPos doesn't seem to work
			wrapper.RenderingEngine = Enums.RenderingEngine.Dot;		// doesn't do absolute pos
			wrapper.RenderingEngine = Enums.RenderingEngine.Circo;		// bad layout for this use case
			wrapper.RenderingEngine = Enums.RenderingEngine.Osage;		// doesn't do absolute pos
			wrapper.RenderingEngine = Enums.RenderingEngine.Patchwork;	// doesn't work for this use case
			wrapper.RenderingEngine = Enums.RenderingEngine.Sfdp;		// doesn't do absolute pos, nodes overlap
			wrapper.RenderingEngine = Enums.RenderingEngine.Twopi;		// doesn't do absolute pos, nodes overlap
			*/

			byte[] output = wrapper.GenerateGraph(graph.ToString(), Enums.GraphReturnType.Svg);

			return output;
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

			graph.GraphAttributes.splines = GraphViz.Splines.ortho;
			graph.GraphAttributes.fontname = "Arial";
			graph.GraphAttributes.rankdir = GraphViz.Rankdir.LR;

			graph.DefaultNodeAttributes.fontname = "Arial";
			graph.DefaultNodeAttributes.shape = Shape.none;
			graph.DefaultEdgeAttributes.fontname = "Arial";

			return graph;
		}

		#endregion
		#region Device Node Generators

		/// <summary>
		/// Generate a legend node
		/// </summary>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public void Legend(GraphViz.Graph graph, double pos_X, double pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 100;

			var shapeName = "Legend";
			var node = new GraphViz.Node(shapeName);
			node.NodeAttributes.pos = new GraphViz.Point()
			{
				X = pos_X,
				Y = pos_Y,
				InputOnly = true
			};

			var table = new HtmlTable();

			table.Border = 0;
			table.Attributes.Add("CELLBORDER", "1");
			table.CellSpacing = 0;

			var row = new HtmlTableRow();
			table.Rows.Add(row);

			var newCell = new HtmlTableCell();
			newCell.Align = "left";
			newCell.Width = COL1MINWIDTH.ToString();
			var fontElem = new HtmlGenericControl("font");
			fontElem.Attributes.Add("point-size", "18");
			fontElem.Attributes.Add("color", styleColor_DataBus_Active.ToString());
			fontElem.InnerText = "Data";
			newCell.Controls.Add(fontElem);
			row.Cells.Add(newCell);

			newCell = new HtmlTableCell();
			newCell.Align = "left";
			newCell.Width = COL2MINWIDTH.ToString();
			fontElem = new HtmlGenericControl("font");
			fontElem.Attributes.Add("point-size", "18");
			fontElem.Attributes.Add("color", styleColor_AddressBus_Active.ToString());
			fontElem.InnerText = "Address";
			newCell.Controls.Add(fontElem);
			row.Cells.Add(newCell);

			node.NodeAttributes.label = table.ToGraphvizLabelFormat();

			graph.Nodes.Add(node);
		}

		/// <summary>
		/// Generate a  node
		/// </summary>
		/// <param name="busInModel"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public void StageBusIn(GraphViz.Graph graph, BusInModel busInModel, double pos_X, double pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 100;

			var shapeName = busInModel.Name;

			var node = new GraphViz.Node(shapeName);

			node.NodeAttributes.pos = new GraphViz.Point()
			{
				X = pos_X,
				Y = pos_Y,
				InputOnly = true
			};

			var table = new HtmlTable();

			table.Border = 0;
			table.Attributes.Add("CELLBORDER", "1");
			table.CellSpacing = 0;

			var row = new HtmlTableRow();
			AddHeader1Cell(row, null, 2, shapeName);
			table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busInModel.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busInModel.Cycles[0].FormattedValue);
			table.Rows.Add(row);

			// Output hex
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busInModel.OutputHex.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busInModel.OutputHex.FormattedValue);
			table.Rows.Add(row);

			// Output int
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busInModel.OutputInt.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busInModel.OutputInt.FormattedValue);
			table.Rows.Add(row);

			// Output Dfb decimal
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busInModel.OutputDFBDec.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busInModel.OutputDFBDec.FormattedValue);
			table.Rows.Add(row);

			node.NodeAttributes.label = table.ToGraphvizLabelFormat();

			graph.Nodes.Add(node);
		}

		/// <summary>
		/// Generate a Mux0 node
		/// </summary>
		/// <param name="mux0"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public void Mux_0(GraphViz.Graph graph, Mux0Model mux0, double pos_X, double pos_Y)
		{
			const int COL1MINWIDTH = 60;
			const int COL2MINWIDTH = 80;
			const int COL3MINWIDTH = 90;

			var shapeName = mux0.Name;

			var node = new GraphViz.Node(shapeName);
			node.NodeAttributes.pos = new GraphViz.Point()
			{
				X = pos_X,
				Y = pos_Y,
				InputOnly = true
			};

			var table = new HtmlTable();

			table.Border = 0;
			table.Attributes.Add("CELLBORDER", "1");
			table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, null, 3, shapeName);
			table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, mux0.Pipeline.Label);
			AddHeader2Cell(row, null, null, mux0.Pipeline.Value[0]);
			AddHeader2Cell(row, null, null, mux0.Pipeline.Value[1]);
			table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux0.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux0.Cycles[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux0.Cycles[1].FormattedValue);
			table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux0.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux0.Instructions[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux0.Instructions[1].FormattedValue);
			table.Rows.Add(row);

			// Addr
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux0.Addresses[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux0.Addresses[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux0.Addresses[1].FormattedValue);
			table.Rows.Add(row);

			// Output
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux0.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux0.Output.FormattedValue);
			table.Rows.Add(row);

			node.NodeAttributes.label = table.ToGraphvizLabelFormat();

			graph.Nodes.Add(node);
		}

		/// <summary>
		/// Generate an ACU node
		/// </summary>
		/// <param name="acu"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public void ACU(GraphViz.Graph graph, ACUModel acu, double pos_X, double pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 100;

			var shapeName = acu.Name;

			var node = new GraphViz.Node(shapeName);
			node.NodeAttributes.pos = new GraphViz.Point()
			{
				X = pos_X,
				Y = pos_Y,
				InputOnly = true
			};

			var table = new HtmlTable();

			table.Border = 0;
			table.Attributes.Add("CELLBORDER", "1");
			table.CellSpacing = 0;

			var row = new HtmlTableRow();
			AddHeader1Cell(row, null, 2, shapeName);
			table.Rows.Add(row);

			// reg
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, acu.Reg_reg.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, acu.Reg_reg.FormattedValue);
			table.Rows.Add(row);

			// freg
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, acu.Reg_freg.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, acu.Reg_freg.FormattedValue);
			table.Rows.Add(row);

			// lreg
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, acu.Reg_lreg.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, acu.Reg_lreg.FormattedValue);
			table.Rows.Add(row);

			// mreg
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, acu.Reg_mreg.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, acu.Reg_mreg.FormattedValue);
			table.Rows.Add(row);

			// flag_mod
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, acu.Reg_flag_mod.Label, false, true);
			AddDetailCell(row, COL1MINWIDTH, null, null, acu.Reg_flag_mod.FormattedValue);
			table.Rows.Add(row);

			node.NodeAttributes.label = table.ToGraphvizLabelFormat();

			graph.Nodes.Add(node);
		}

		/// <summary>
		/// Generate a Mux 1 node
		/// </summary>
		/// <param name="mux1"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public void Mux_1(GraphViz.Graph graph, Mux1Model mux1, double pos_X, double pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 70;
			const int COL3MINWIDTH = 84;

			var shapeName = mux1.Name;

			var node = new GraphViz.Node(shapeName);
			node.NodeAttributes.pos = new GraphViz.Point()
			{
				X = pos_X,
				Y = pos_Y,
				InputOnly = true
			};

			var table = new HtmlTable();

			table.Border = 0;
			table.Attributes.Add("CELLBORDER", "1");
			table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, null, 3, shapeName);
			table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, mux1.Pipeline.Label);
			AddHeader2Cell(row, null, null, mux1.Pipeline.Value[0]);
			AddHeader2Cell(row, null, null, mux1.Pipeline.Value[1]);
			table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux1.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux1.Cycles[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux1.Cycles[1].FormattedValue);
			table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux1.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux1.Instructions[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux1.Instructions[1].FormattedValue);
			table.Rows.Add(row);

			// Output
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux1.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux1.Output.FormattedValue);
			table.Rows.Add(row);

			node.NodeAttributes.label = table.ToGraphvizLabelFormat();

			graph.Nodes.Add(node);
		}

		/// <summary>
		/// Generate a DataRam node
		/// </summary>
		/// <param name="dataRam"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public void DataRam(GraphViz.Graph graph, DataRamModel dataRam, double pos_X, double pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 80;

			var shapeName = dataRam.Name;

			var node = new GraphViz.Node(shapeName);
			node.NodeAttributes.pos = new GraphViz.Point()
			{
				X = pos_X,
				Y = pos_Y,
				InputOnly = true
			};

			var table = new HtmlTable();

			table.Border = 0;
			table.Attributes.Add("CELLBORDER", "1");
			table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, null, 2, shapeName);
			table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, dataRam.Pipeline.Label);
			AddHeader2Cell(row, null, null, dataRam.Pipeline.Value[0]);
			table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, dataRam.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, dataRam.Cycles[0].FormattedValue);
			table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, dataRam.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, dataRam.Instructions[0].FormattedValue);
			table.Rows.Add(row);

			// ValueWrite
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, dataRam.ValueWrite.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, dataRam.ValueWrite.FormattedValue);
			table.Rows.Add(row);

			// Address
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, dataRam.Address.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, dataRam.Address.FormattedValue);
			table.Rows.Add(row);

			// Value
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, dataRam.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, dataRam.Output.FormattedValue);
			table.Rows.Add(row);

			node.NodeAttributes.label = table.ToGraphvizLabelFormat();

			graph.Nodes.Add(node);
		}

		/// <summary>
		/// Generate a Mux 2 node
		/// </summary>
		/// <param name="mux2"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public void Mux_2(GraphViz.Graph graph, Mux2Model mux2, double pos_X, double pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 86;
			const int COL3MINWIDTH = 86;

			var shapeName = mux2.Name;

			var node = new GraphViz.Node(shapeName);
			node.NodeAttributes.pos = new GraphViz.Point()
			{
				X = pos_X,
				Y = pos_Y,
				InputOnly = true
			};

			var table = new HtmlTable();

			table.Border = 0;
			table.Attributes.Add("CELLBORDER", "1");
			table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, null, 3, shapeName);
			table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, mux2.Pipeline.Label);
			AddHeader2Cell(row, null, null, mux2.Pipeline.Value[0]);
			AddHeader2Cell(row, null, null, mux2.Pipeline.Value[1]);
			table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux2.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux2.Cycles[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux2.Cycles[1].FormattedValue);
			table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux2.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux2.Instructions[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux2.Instructions[1].FormattedValue);
			table.Rows.Add(row);

			// Input
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux2.Input0.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, WebUtility.HtmlEncode(mux2.Input0.FormattedValue));
			AddDetailCell(row, COL3MINWIDTH, null, null, WebUtility.HtmlEncode(mux2.Input1.FormattedValue));
			table.Rows.Add(row);

			// Input Source
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux2.Input0Src.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, WebUtility.HtmlEncode(mux2.Input0Src.FormattedValue));
			AddDetailCell(row, COL3MINWIDTH, null, null, WebUtility.HtmlEncode(mux2.Input1Src.FormattedValue));
			table.Rows.Add(row);

			// Output
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux2.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux2.Output.FormattedValue);
			table.Rows.Add(row);

			node.NodeAttributes.label = table.ToGraphvizLabelFormat();

			graph.Nodes.Add(node);
		}

		/// <summary>
		/// Generate a MAC node
		/// </summary>
		/// <param name="mac"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public void MAC(GraphViz.Graph graph, MACModel mac, double pos_X, double pos_Y)
		{
			const int COL1MINWIDTH = 80;
			const int COL2MINWIDTH = 80;
			const int COL3MINWIDTH = 110;
			const int COL4MINWIDTH = 120;

			var shapeName = mac.Name;

			var node = new GraphViz.Node(shapeName);
			node.NodeAttributes.pos = new GraphViz.Point()
			{
				X = pos_X,
				Y = pos_Y,
				InputOnly = true
			};

			var table = new HtmlTable();

			table.Border = 0;
			table.Attributes.Add("CELLBORDER", "1");
			table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, null, 4, shapeName);
			table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, mac.Pipeline.Label);
			AddHeader2Cell(row, null, null, mac.Pipeline.Value[0]);
			AddHeader2Cell(row, null, null, mac.Pipeline.Value[1]);
			AddHeader2Cell(row, null, null, mac.Pipeline.Value[2]);
			table.Rows.Add(row);

			// Cycle row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mac.Cycles[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mac.Cycles[1].FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.Cycles[2].FormattedValue);
			table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mac.Instructions[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mac.Instructions[1].FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.Instructions[2].FormattedValue);
			table.Rows.Add(row);

			// a term row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.PipelineItems[0].A.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, mac.PipelineItems[1].A.FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.PipelineItems[2].A.FormattedValue);
			table.Rows.Add(row);

			// b term row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.PipelineItems[0].B.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, mac.PipelineItems[1].B.FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.PipelineItems[2].B.FormattedValue);
			table.Rows.Add(row);

			// Accumulator
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.PipelineItems[0].Accumulator.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.PipelineItems[2].Accumulator.FormattedValue);
			table.Rows.Add(row);

			// ALU row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.PipelineItems[0].AluValue.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, mac.PipelineItems[1].AluValue.FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.PipelineItems[2].AluValue.FormattedValue);
			table.Rows.Add(row);

			// Output Formula op row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.PipelineItems[0].OutputFormula.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.PipelineItems[2].OutputFormula.FormattedValue);
			table.Rows.Add(row);

			// Output row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.Output.FormattedValue);
			table.Rows.Add(row);

			node.NodeAttributes.label = table.ToGraphvizLabelFormat();

			graph.Nodes.Add(node);
		}

		/// <summary>
		/// Generate a Mux 3 node
		/// </summary>
		/// <param name="mux3"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public void Mux_3(GraphViz.Graph graph, Mux3Model mux3, double pos_X, double pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 70;
			const int COL3MINWIDTH = 90;

			var shapeName = mux3.Name;

			var node = new GraphViz.Node(shapeName);
			node.NodeAttributes.pos = new GraphViz.Point()
			{
				X = pos_X,
				Y = pos_Y,
				InputOnly = true
			};

			var table = new HtmlTable();

			table.Border = 0;
			table.Attributes.Add("CELLBORDER", "1");
			table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, null, 3, shapeName);
			table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, mux3.Pipeline.Label);
			AddHeader2Cell(row, null, null, mux3.Pipeline.Value[0]);
			AddHeader2Cell(row, null, null, mux3.Pipeline.Value[1]);
			table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux3.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux3.Cycles[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux3.Cycles[1].FormattedValue);
			table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux3.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux3.Instructions[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux3.Instructions[1].FormattedValue);
			table.Rows.Add(row);

			// Output
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux3.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux3.Output.FormattedValue);
			table.Rows.Add(row);

			node.NodeAttributes.label = table.ToGraphvizLabelFormat();

			graph.Nodes.Add(node);
		}

		/// <summary>
		/// Generate an ALU node
		/// </summary>
		/// <param name="alu"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public void ALU(GraphViz.Graph graph, ALUModel alu, double pos_X, double pos_Y)
		{
			const int COL1MINWIDTH = 100;
			const int COL2MINWIDTH = 90;
			const int COL3MINWIDTH = 90;
			const int COL4MINWIDTH = 90;

			var shapeName = alu.Name;

			var node = new GraphViz.Node(shapeName);
			node.NodeAttributes.pos = new GraphViz.Point()
			{
				X = pos_X,
				Y = pos_Y,
				InputOnly = true
			};

			var table = new HtmlTable();

			table.Border = 0;
			table.Attributes.Add("CELLBORDER", "1");
			table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, null, 4, shapeName);
			table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, alu.Pipeline.Label);
			AddHeader2Cell(row, null, null, alu.Pipeline.Value[0]);
			AddHeader2Cell(row, null, null, alu.Pipeline.Value[1]);
			AddHeader2Cell(row, null, null, alu.Pipeline.Value[2]);
			table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, alu.Cycles[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, alu.Cycles[1].FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, alu.Cycles[2].FormattedValue);
			table.Rows.Add(row);

			// Instr row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, WebUtility.HtmlEncode(alu.Instructions[0].FormattedValue));
			AddDetailCell(row, COL3MINWIDTH, null, null, WebUtility.HtmlEncode(alu.Instructions[1].FormattedValue));
			AddDetailCell(row, COL4MINWIDTH, null, null, WebUtility.HtmlEncode(alu.Instructions[2].FormattedValue));
			table.Rows.Add(row);

			// Input A
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.PipelineItems[0].InputA.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, alu.PipelineItems[1].InputA.FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, alu.PipelineItems[2].InputA.FormattedValue);
			table.Rows.Add(row);

			// Input A Source
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.PipelineItems[0].InputASource.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, alu.PipelineItems[1].InputASource.FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, alu.PipelineItems[2].InputASource.FormattedValue);
			table.Rows.Add(row);

			// Input B
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.PipelineItems[0].InputB.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, alu.PipelineItems[1].InputB.FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, alu.PipelineItems[2].InputB.FormattedValue);
			table.Rows.Add(row);

			// Input B Source
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.PipelineItems[0].InputBSource.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, alu.PipelineItems[1].InputBSource.FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, alu.PipelineItems[2].InputBSource.FormattedValue);
			table.Rows.Add(row);

			// Output formula
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.PipelineItems[0].OutputFormula.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, "", true);
			AddDetailCell(row, COL3MINWIDTH, null, null, WebUtility.HtmlEncode(alu.PipelineItems[1].OutputFormula.FormattedValue));
			AddDetailCell(row, COL4MINWIDTH, null, null, WebUtility.HtmlEncode(alu.PipelineItems[2].OutputFormula.FormattedValue));
			table.Rows.Add(row);

			// Output row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, "", true);
			AddDetailCell(row, COL3MINWIDTH, null, null, "", true);
			AddDetailCell(row, COL4MINWIDTH, null, null, alu.Output.FormattedValue);
			table.Rows.Add(row);

			node.NodeAttributes.label = table.ToGraphvizLabelFormat();

			graph.Nodes.Add(node);
		}

		/// <summary>
		/// Generate a Shifter node
		/// </summary>
		/// <param name="shifter"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public void Shifter(GraphViz.Graph graph, ShifterModel shifter, double pos_X, double pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 80;
			const int COL3MINWIDTH = 90;

			var shapeName = shifter.Name;

			var node = new GraphViz.Node(shapeName);
			node.NodeAttributes.pos = new GraphViz.Point()
			{
				X = pos_X,
				Y = pos_Y,
				InputOnly = true
			};

			var table = new HtmlTable();

			table.Border = 0;
			table.Attributes.Add("CELLBORDER", "1");
			table.CellSpacing = 0;

			var row = new HtmlTableRow();
			AddHeader1Cell(row, null, 3, shapeName);
			table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, shifter.Pipeline.Label);
			AddHeader2Cell(row, null, null, shifter.Pipeline.Value[0]);
			AddHeader2Cell(row, null, null, shifter.Pipeline.Value[1]);
			table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, shifter.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, shifter.Cycles[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, shifter.Cycles[1].FormattedValue);
			table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, shifter.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, WebUtility.HtmlEncode(shifter.Instructions[0].FormattedValue));
			AddDetailCell(row, COL3MINWIDTH, null, null, WebUtility.HtmlEncode(shifter.Instructions[1].FormattedValue));
			table.Rows.Add(row);

			// Input
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, shifter.Input.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, shifter.Input.FormattedValue);
			table.Rows.Add(row);

			// Output
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, shifter.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, shifter.Output.FormattedValue);
			table.Rows.Add(row);

			node.NodeAttributes.label = table.ToGraphvizLabelFormat();

			graph.Nodes.Add(node);
		}

		/// <summary>
		/// Generate a Bus Out node
		/// </summary>
		/// <param name="busOut"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public void HoldBusOut(GraphViz.Graph graph, BusOutModel busOut, double pos_X, double pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 80;

			var shapeName = busOut.Name;

			var node = new GraphViz.Node(shapeName);
			node.NodeAttributes.pos = new GraphViz.Point()
			{
				X = pos_X,
				Y = pos_Y,
				InputOnly = true
			};

			var table = new HtmlTable();

			table.Border = 0;
			table.Attributes.Add("CELLBORDER", "1");
			table.CellSpacing = 0;

			var row = new HtmlTableRow();
			AddHeader1Cell(row, null, 2, shapeName);
			table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busOut.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busOut.Cycles[0].FormattedValue);
			table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busOut.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busOut.Instructions[0].FormattedValue);
			table.Rows.Add(row);

			// Output hex
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busOut.OutputHex.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busOut.OutputHex.FormattedValue);
			table.Rows.Add(row);

			// Output int
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busOut.OutputInt.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busOut.OutputInt.FormattedValue);
			table.Rows.Add(row);

			// Output Dfb decimal
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busOut.OutputDFBDec.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busOut.OutputDFBDec.FormattedValue);
			table.Rows.Add(row);

			node.NodeAttributes.label = table.ToGraphvizLabelFormat();

			graph.Nodes.Add(node);
		}

		/// <summary>
		/// Generate a Global node
		/// </summary>
		/// <param name="global"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public void Global(GraphViz.Graph graph, GlobalModel global, double pos_X, double pos_Y)
		{
			const int COL1MINWIDTH = 100;
			const int COL2MINWIDTH = 80;
			const int COL3MINWIDTH = 100;
			const int COL4MINWIDTH = 80;

			var shapeName = global.Name;

			var node = new GraphViz.Node(shapeName);
			node.NodeAttributes.pos = new GraphViz.Point()
			{
				X = pos_X,
				Y = pos_Y,
				InputOnly = true
			};

			var table = new HtmlTable();

			table.Border = 0;
			table.Attributes.Add("CELLBORDER", "1");
			table.CellSpacing = 0;

			table.Rows.Add(new HtmlTableRow());
			table.Rows.Add(new HtmlTableRow());
			table.Rows.Add(new HtmlTableRow());
			table.Rows.Add(new HtmlTableRow());
			table.Rows.Add(new HtmlTableRow());
			table.Rows.Add(new HtmlTableRow());
			table.Rows.Add(new HtmlTableRow());
			table.Rows.Add(new HtmlTableRow());
			table.Rows.Add(new HtmlTableRow());
			table.Rows.Add(new HtmlTableRow());

			// Header 1
			AddHeader1Cell(table.Rows[0], null, 4, shapeName);

			// Instr row
			AddDetailCell(table.Rows[1], COL1MINWIDTH, null, null, global.Instructions[0].Label, false, true);
			AddDetailCell(table.Rows[1], COL2MINWIDTH + COL3MINWIDTH + COL4MINWIDTH, null, 3, global.Instructions[0].FormattedValue);
			
			// Global_en0
			AddDetailCell(table.Rows[2], COL1MINWIDTH, null, null, global.Global_en0.Label, false, true);
			AddDetailCell(table.Rows[2], COL2MINWIDTH, null, null, global.Global_en0.FormattedValue);
			// Sem_en0
			AddDetailCell(table.Rows[2], COL3MINWIDTH, null, null, global.Sem_en0.Label, false, true);
			AddDetailCell(table.Rows[2], COL4MINWIDTH, null, null, global.Sem_en0.FormattedValue);
					
			// Global_en1
			AddDetailCell(table.Rows[3], COL1MINWIDTH, null, null, global.Global_en1.Label, false, true);
			AddDetailCell(table.Rows[3], COL2MINWIDTH, null, null, global.Global_en1.FormattedValue);
			// Sem_en1
			AddDetailCell(table.Rows[3], COL3MINWIDTH, null, null, global.Sem_en1.Label, false, true);
			AddDetailCell(table.Rows[3], COL4MINWIDTH, null, null, global.Sem_en1.FormattedValue);

			// Global_en2
			AddDetailCell(table.Rows[4], COL1MINWIDTH, null, null, global.Global_en2.Label, false, true);
			AddDetailCell(table.Rows[4], COL2MINWIDTH, null, null, global.Global_en2.FormattedValue);
			// Sem_en2
			AddDetailCell(table.Rows[4], COL3MINWIDTH, null, null, global.Sem_en2.Label, false, true);
			AddDetailCell(table.Rows[4], COL4MINWIDTH, null, null, global.Sem_en2.FormattedValue);

			// Sqcnt
			AddDetailCell(table.Rows[5], COL1MINWIDTH, null, null, global.Sqcnt.Label, false, true);
			AddDetailCell(table.Rows[5], COL2MINWIDTH, null, null, global.Sqcnt.FormattedValue);
			// rflag
			AddDetailCell(table.Rows[5], COL3MINWIDTH, null, null, global.Rflag.Label, false, true);
			AddDetailCell(table.Rows[5], COL4MINWIDTH, null, null, global.Rflag.FormattedValue);

			// Sqcval
			AddDetailCell(table.Rows[6], COL1MINWIDTH, null, null, global.Sqcval.Label, false, true);
			AddDetailCell(table.Rows[6], COL2MINWIDTH, null, null, global.Sqcval.FormattedValue);
			// DP Sign
			AddDetailCell(table.Rows[6], COL3MINWIDTH, null, null, global.DPsign.Label, false, true);
			AddDetailCell(table.Rows[6], COL4MINWIDTH, null, null, global.DPsign.FormattedValue);

			// SatEn
			AddDetailCell(table.Rows[7], COL1MINWIDTH, null, null, global.SatEn.Label, false, true);
			AddDetailCell(table.Rows[7], COL2MINWIDTH, null, null, global.SatEn.FormattedValue);
			// tflag
			AddDetailCell(table.Rows[7], COL3MINWIDTH, null, null, global.Tflag.Label, false, true);
			AddDetailCell(table.Rows[7], COL4MINWIDTH, null, null, global.Tflag.FormattedValue);

			// Satflag
			AddDetailCell(table.Rows[8], COL1MINWIDTH, null, null, global.Satflag.Label, false, true);
			AddDetailCell(table.Rows[8], COL2MINWIDTH, null, null, global.Satflag.FormattedValue);
			// Tsign
			AddDetailCell(table.Rows[8], COL3MINWIDTH, null, null, global.Tsign.Label, false, true);
			AddDetailCell(table.Rows[8], COL4MINWIDTH, null, null, global.Tsign.FormattedValue);

			// Satflag
			AddDetailCell(table.Rows[9], COL1MINWIDTH, null, 2, null, true);
			// Tsign
			AddDetailCell(table.Rows[9], COL3MINWIDTH, null, null, global.DPeq.Label, false, true);
			AddDetailCell(table.Rows[9], COL4MINWIDTH, null, null, global.DPeq.FormattedValue);

			node.NodeAttributes.label = table.ToGraphvizLabelFormat();

			graph.Nodes.Add(node);
		}

		#endregion
		#region Node Helpers

		/// <summary>
		/// Create a hidden node, used to force edge routing
		/// </summary>
		/// <param name="id"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public Node HiddenNode(string id, double pos_X, double pos_Y, double width, double height)
		{
			var node = new Node(id);
			node.NodeAttributes.pos = new GraphViz.Point()
			{
				X = pos_X,
				Y = pos_Y,
				InputOnly = true
			};

			node.NodeAttributes.width = width;
			node.NodeAttributes.height = height;
			node.NodeAttributes.shape = Shape.box;
			node.NodeAttributes.style = debugShowHiddenNodes ? Style.Node.solid : Style.Node.invis;

			return node;
		}

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
			var newCell = new HtmlTableCell();
			newCell.Width = width.ToString();
			newCell.Align = "left";
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
		private void AddHeader1Cell(HtmlTableRow row, string portName, int? columnSpan, string text)
		{
			var newCell = new HtmlTableCell();

			newCell.Align = "middle";
			var fontElem = new HtmlGenericControl("font");
			fontElem.Attributes.Add("point-size", "18");
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
		private void AddHeader2Cell(HtmlTableRow row, string portName, int? columnSpan, string text)
		{
			var newCell = new HtmlTableCell();
			newCell.Align = "middle";

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

		/// <summary>
		/// Create a connection string
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="conn"></param>
		private void AddConnection(Graph graph, Connection conn)
		{
			var edge = new Edge();

			edge.Connections.Add(conn.From.ToString());
			edge.Connections.Add(conn.To.ToString());

			// label
			if (conn.FromPortActive == PortStatus.Active && !string.IsNullOrEmpty(conn.FromPortActiveText))
			{
				edge.EdgeAttributes.xlabel = conn.FromPortActiveText;
			}
			else if (conn.FromPortActive == PortStatus.Inactive && !string.IsNullOrEmpty(conn.FromPortInactiveText))
			{
				edge.EdgeAttributes.xlabel = conn.FromPortInactiveText;
			}

			// default
			edge.EdgeAttributes.dir = GraphViz.DirType.forward;

			// Colors
			if (conn.FromPortActive == PortStatus.Active
				&& conn.ToPortActive == PortStatus.Active)
			{
				// source Active, target = Active:
				switch (conn.BusType)
				{
					case BusType.Address:
						edge.EdgeAttributes.color = styleColor_AddressBus_Active;
						break;
					case BusType.Data:
						edge.EdgeAttributes.color = styleColor_DataBus_Active;
						break;
				}
				edge.EdgeAttributes.penwidth = stylePenWidth_Bus_Active;
			}
			else if (conn.FromPortActive == PortStatus.Active
				&& conn.ToPortActive == PortStatus.Inactive)
			{
				// source Active, target = Inactive:
				switch (conn.BusType)
				{
					case BusType.Address:
						edge.EdgeAttributes.color = styleColor_AddressBus_Inactive;
						break;
					case BusType.Data:
						edge.EdgeAttributes.color = styleColor_DataBus_Inactive;
						break;
				}
			}
			else
			{
				// source Inactive, target = Inactive:
				edge.EdgeAttributes.color = styleColor_Bus_Inactive;
				edge.EdgeAttributes.labelfontcolor = styleColor_Bus_Inactive;
			}

			graph.Edges.Add(edge);
		}

		#endregion
	}
}
