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

namespace DFBSimulatorWrapper.Diagram.StateDiagram
{
	/// <summary>
	/// Class for generating a GraphViz state diagram from the device models
	/// </summary>
	public class StateDiagram_old
	{
		#region Classes

		/// <summary>
		/// Represents a node on the graph
		/// </summary>
		public class RecordNode
		{
			public readonly string Id;
			public readonly HtmlTable Table;
			//public readonly bool FixedSize;
			//public readonly int Width;
			//public readonly int Height;
			public decimal? Pos_X;
			public decimal? Pos_Y;
			public string fontcolor;

			public RecordNode(string id)
			{
				this.Id = id;
				Table = new HtmlTable();
				//FixedSize = fixedSize;
				//if (width > 0) { Width = width; };
				//if (height > 0) { Height = height; };

			}

			public override string ToString()
			{
				var sb = new StringBuilder();
				var tw = new StringWriter(sb);
				var hw = new HtmlTextWriter(tw);
				// Node start
				sb.AppendFormat("\"{0}\" [\n", Id);

				// fontcolor
				if (!string.IsNullOrEmpty(fontcolor))
				{
					sb.AppendFormat("fontcolor = \"{0}\"\n", fontcolor);
				}

				// Position attributes (neato layout only)
				if (Pos_X.HasValue && Pos_Y.HasValue)
				{
					sb.AppendFormat("pos = \"{0},{1}!\"\n", Pos_X.Value, Pos_Y.Value);
				}

				// Size attributes
				//sb.AppendFormat("fixedsize = {0}\n", FixedSize.ToString());
				//if (Width != 0) { sb.AppendFormat("Width = {0}\n", Width); }
				//if (Height != 0) { sb.AppendFormat("Height = {0}\n", Height); }

				// Table content
				sb.AppendLine("label=<");
				Table.RenderControl(hw);

				// Node close
				sb.AppendLine(">];");
				return sb.ToString();
			}
		}

		/// <summary>
		/// Represents a hidden node on the graph, used to force the edge 
		/// routing in cases where the layout is not optimal
		/// </summary>
		public class HiddenNode
		{
			public readonly string Id;
			public decimal? Pos_X;
			public decimal? Pos_Y;
			public string ConnectedToId;
			public decimal? Width;
			public decimal? Height;

			public HiddenNode(string id)
			{
				this.Id = id;
			}

			public override string ToString()
			{
				var sb = new StringBuilder();

				// Node start
				sb.AppendFormat("\"{0}\" [\n", Id);

				// Position attributes (neato layout only)
				if (Pos_X.HasValue && Pos_Y.HasValue)
				{
					sb.AppendFormat("pos = \"{0},{1}!\"\n", Pos_X.Value, Pos_Y.Value);
				}

				sb.AppendFormat(" label = \"{0}\"", Id);
				sb.AppendFormat(" shape = \"{0}\"", "box");
				sb.AppendFormat(" style = \"{0}\"", "invis");

				if (Width.HasValue)
				{
					sb.AppendFormat("width = \"{0}\"\n", Width);
				}
				if (Height.HasValue)
				{
					sb.AppendFormat("height = \"{0}\"\n", Height);
				}
				if (Width.HasValue || Height.HasValue)
				{
					sb.AppendFormat("fixedsize = \"{0}\"\n", "true");
				}

				// Node close
				sb.AppendLine("];");

				return sb.ToString();
			}

			public string Connection()
			{
				return string.Format("{0} -> {1} [style = \"invis\"]\n", this.Id, this.ConnectedToId);
			}
		}

		#endregion
		#region Constants

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

		public StateDiagram_old()
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

			var sb = new StringBuilder();
			Graph(sb, "LR");
			sb.AppendLine("node [shape=plaintext]");

			// Nodes
			decimal positionMultiplier = 1;
			decimal posY_center = 24 * positionMultiplier;
			decimal posXWidth = 3.5m * positionMultiplier;

			decimal legend_X = 3.5m;
			decimal legend_Y = 8.0m;

			decimal rowOffset1 = 0.0m;
			decimal rowOffset2 = rowOffset1 + 3.25m;
			decimal rowOffset3 = rowOffset2 + 3.0m;

			decimal rowOffset_Stage = rowOffset2;
			decimal rowOffset_Mux0 = rowOffset1;
			decimal rowOffset_Mux1 = rowOffset2;
			decimal rowOffset_ACU = rowOffset3 + 0.5m;
			decimal rowOffset_DataRam = rowOffset3;
			decimal rowOffset_Mux2 = rowOffset2;
			decimal rowOffset_Mux3 = rowOffset2;
			decimal rowOffset_ALU = rowOffset1;
			decimal rowOffset_Shifter = rowOffset1;
			decimal rowOffset_Hold = rowOffset2;
			decimal rowOffset_Global = rowOffset3 + .45m;

			decimal colpos_Stage_Mux0 = 3.5m;
			decimal colpos_Mux1 = colpos_Stage_Mux0 + 3.5m;
			decimal colpos_ACU = colpos_Mux1 + 1.6m;
			decimal colpos_DataRamMux2 = colpos_ACU + 2.85m;
			decimal colpos_MAC = colpos_DataRamMux2 + 2.5m;
			decimal colpos_Mux3 = colpos_MAC + 2.5m;
			decimal colpos_ALU = colpos_Mux3 + 3.25m;
			decimal colpos_Global = colpos_ALU;
			decimal colpos_Shifter = colpos_ALU + 5.25m;
			decimal colpos_Hold = colpos_Shifter + 0.75m;

			// Hidden spacers to force routing of shifter to mux2 edges
			decimal shifterABuffer_width = 1.5m;
			decimal shifterABuffer_height = 4.25m;
			decimal rowOffset_ShifterA_buffer = rowOffset_Shifter + 1.0m + shifterABuffer_height / 2;
			decimal colpos_ShifterA_buffer = colpos_Shifter - 1.8m;

			decimal shifterBBuffer_width = 1.5m;
			decimal shifterBBuffer_height = 4.25m;
			decimal rowOffset_ShifterB_buffer = rowOffset_Shifter + 1.0m + shifterBBuffer_height / 2;
			decimal colpos_ShifterB_buffer = colpos_Shifter - 1.8m;

			sb.Append(Legend(legend_X, posY_center + legend_Y));

			sb.Append(StageBusIn(stateFrame.Stage_A, colpos_Stage_Mux0, posY_center + rowOffset_Stage).ToString());
			sb.Append(Mux_0(stateFrame.Mux0, colpos_Stage_Mux0, posY_center + rowOffset_Mux0));
			sb.Append(StageBusIn(stateFrame.Stage_B, colpos_Stage_Mux0, posY_center - rowOffset_Stage).ToString());

			sb.Append(Mux_1(stateFrame.Mux_1A, colpos_Mux1, posY_center + rowOffset_Mux1));
			sb.Append(Mux_1(stateFrame.Mux_1B, colpos_Mux1, posY_center - rowOffset_Mux1));

			sb.Append(ACU(stateFrame.ACU_A, colpos_ACU, posY_center + rowOffset_ACU));
			sb.Append(ACU(stateFrame.ACU_B, colpos_ACU, posY_center - rowOffset_ACU));

			sb.Append(DataRam(stateFrame.DataRam_A, colpos_DataRamMux2, posY_center + rowOffset_DataRam));
			sb.Append(Mux_2(stateFrame.Mux_2A, colpos_DataRamMux2, posY_center + rowOffset_Mux2));
			sb.Append(Mux_2(stateFrame.Mux_2B, colpos_DataRamMux2, posY_center - rowOffset_Mux2));
			sb.Append(DataRam(stateFrame.DataRam_B, colpos_DataRamMux2, posY_center - rowOffset_DataRam));

			sb.Append(MAC(stateFrame.MAC, colpos_MAC, posY_center));

			sb.Append(Mux_3(stateFrame.Mux_3A, colpos_Mux3, posY_center + rowOffset_Mux3));
			sb.Append(Mux_3(stateFrame.Mux_3B, colpos_Mux3, posY_center - rowOffset_Mux3));

			// Hidden nodes to push connectors from shifter up/down so they don't overlap
			var shifter_Abuffer = MakeHiddenNode("shifter_Abuffer", colpos_ShifterA_buffer, posY_center + rowOffset_ShifterA_buffer, shifterABuffer_width, shifterABuffer_height);
			shifter_Abuffer.ConnectedToId = DevicePort.Mux_3A.ToString();
			sb.Append(shifter_Abuffer);

			var shifter_Bbuffer = MakeHiddenNode("shifter_Bbuffer", colpos_ShifterB_buffer, posY_center - rowOffset_ShifterB_buffer, shifterBBuffer_width, shifterBBuffer_height);
			shifter_Bbuffer.ConnectedToId = DevicePort.Mux_3B.ToString();
			sb.Append(shifter_Bbuffer);

			sb.Append(ALU(stateFrame.ALU, colpos_ALU, posY_center + rowOffset_ALU));

			sb.Append(Shifter(stateFrame.Shifter, colpos_Shifter, posY_center + rowOffset_Shifter));

			sb.Append(HoldBusOut(stateFrame.Hold_A, colpos_Hold, posY_center + rowOffset_Hold));
			sb.Append(HoldBusOut(stateFrame.Hold_B, colpos_Hold, posY_center - rowOffset_Hold));

			sb.Append(Global(stateFrame.Global, colpos_Global, posY_center - rowOffset_Global));

			// Connection position overrides
			ConnectionOverrides(stateFrame);

			// Connections
			stateFrame.Stage_A.ConnectionInputs.ForEach(x => AddConnection(sb, x));
			stateFrame.Stage_B.ConnectionInputs.ForEach(x => AddConnection(sb, x));

			stateFrame.Mux0.ConnectionInputs.ForEach(x => AddConnection(sb, x));

			stateFrame.ACU_A.ConnectionInputs.ForEach(x => AddConnection(sb, x));
			stateFrame.ACU_B.ConnectionInputs.ForEach(x => AddConnection(sb, x));

			stateFrame.Mux_1A.ConnectionInputs.ForEach(x => AddConnection(sb, x));
			stateFrame.Mux_1B.ConnectionInputs.ForEach(x => AddConnection(sb, x));

			stateFrame.DataRam_A.ConnectionInputs.ForEach(x => AddConnection(sb, x));
			stateFrame.DataRam_B.ConnectionInputs.ForEach(x => AddConnection(sb, x));

			stateFrame.Mux_2A.ConnectionInputs.ForEach(x => AddConnection(sb, x));
			stateFrame.Mux_2B.ConnectionInputs.ForEach(x => AddConnection(sb, x));

			stateFrame.MAC.ConnectionInputs.ForEach(x => AddConnection(sb, x));

			stateFrame.Mux_3A.ConnectionInputs.ForEach(x => AddConnection(sb, x));
			stateFrame.Mux_3B.ConnectionInputs.ForEach(x => AddConnection(sb, x));

			sb.AppendLine(shifter_Abuffer.Connection());
			sb.AppendLine(shifter_Bbuffer.Connection());

			stateFrame.ALU.ConnectionInputs.ForEach(x => AddConnection(sb, x));

			stateFrame.Shifter.ConnectionInputs.ForEach(x => AddConnection(sb, x));

			stateFrame.Hold_A.ConnectionInputs.ForEach(x => AddConnection(sb, x));
			stateFrame.Hold_B.ConnectionInputs.ForEach(x => AddConnection(sb, x));

			stateFrame.Global.ConnectionInputs.ForEach(x => AddConnection(sb, x));

			sb.AppendLine("}");
			var text = sb.ToString();

			//wrapper.RenderingEngine = Enums.RenderingEngine.Fdp;          // Ok, portPos doesn't seem to work
			wrapper.RenderingEngine = Enums.RenderingEngine.Neato;          // Ortho connectors sometimes dont' work
			//wrapper.RenderingEngine = Enums.RenderingEngine.Dot;			// doesn't do absolute pos
			//wrapper.RenderingEngine = Enums.RenderingEngine.Circo;		// bad layout for this use case
			//wrapper.RenderingEngine = Enums.RenderingEngine.Osage;		// doesn't do absolute pos
			//wrapper.RenderingEngine = Enums.RenderingEngine.Patchwork;	// doesn't work for this use case
			//wrapper.RenderingEngine = Enums.RenderingEngine.Sfdp;			// doesn't do absolute pos, nodes overlap
			//wrapper.RenderingEngine = Enums.RenderingEngine.Twopi;		// doesn't do absolute pos, nodes overlap

			byte[] output = wrapper.GenerateGraph(text, Enums.GraphReturnType.Svg);

			return output;
		}

		/// <summary>
		/// Graph defaults
		/// </summary>
		/// <param name="n"></param>
		/// <param name="rankDir"></param>
		private static void Graph(StringBuilder n, string rankDir)
		{
			n.AppendLine("digraph dfb {");
			n.AppendLine("splines = \"ortho\"\n");

			n.AppendFormat("graph[");
			n.AppendFormat("fontname = \"{0}\"\n", "Arial");
			n.AppendFormat("rankdir = \"{0}\"\n", rankDir);
			n.AppendLine("];");

			n.AppendFormat("node[");
			n.AppendFormat("fontname = \"{0}\",", "Arial");
			n.AppendLine("];");

			n.AppendFormat("edge[");
			n.AppendFormat("fontname = \"{0}\",", "Arial");
			n.AppendLine("];");
		}

		/// <summary>
		/// Create any connection overrides here
		/// </summary>
		/// <param name="stateFrame"></param>
		public void ConnectionOverrides(DFBStateFrame stateFrame)
		{
			//// Mux0 inputs - Stage A/B on the east side
			//stateFrame.Mux0.ConnectionInputs
			//	.Where(x => x.From == DevicePort.Stage_A)
			//	.FirstOrDefault()
			//	.FromPortPosition = new PortPosition(PortPosition.Position.east);
			//stateFrame.Mux0.ConnectionInputs
			//	.Where(x => x.From == DevicePort.Stage_B)
			//	.FirstOrDefault()
			//	.FromPortPosition = new PortPosition(PortPosition.Position.east);
		}

		#endregion
		#region Device Node Generators

		/// <summary>
		/// Generate a legend node
		/// </summary>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public RecordNode Legend(decimal? pos_X, decimal? pos_Y)
		{
			//n.AppendLine("1[label = \"Hello World!\" fontname = \"Arial\"];");
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 100;

			var shapeName = "Legend";
			var node = new RecordNode(shapeName);
			node.Pos_X = pos_X;
			node.Pos_Y = pos_Y;

			node.Table.Border = 0;
			node.Table.Attributes.Add("CELLBORDER", "1");
			node.Table.CellSpacing = 0;

			var row = new HtmlTableRow();

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

			node.Table.Rows.Add(row);

			return node;
		}

		/// <summary>
		/// Generate a  node
		/// </summary>
		/// <param name="busInModel"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public RecordNode StageBusIn(BusInModel busInModel, decimal? pos_X, decimal? pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 100;

			var shapeName = busInModel.Name;

			var node = new RecordNode(shapeName);
			if (pos_X.HasValue && pos_Y.HasValue)
			{
				node.Pos_X = pos_X;
				node.Pos_Y = pos_Y;
			}
			node.Table.Border = 0;
			node.Table.Attributes.Add("CELLBORDER", "1");
			node.Table.CellSpacing = 0;

			var row = new HtmlTableRow();
			AddHeader1Cell(row, shapeName, 2, shapeName);
			node.Table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busInModel.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busInModel.Cycles[0].FormattedValue);
			node.Table.Rows.Add(row);

			// Output hex
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busInModel.OutputHex.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busInModel.OutputHex.FormattedValue);
			node.Table.Rows.Add(row);

			// Output int
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busInModel.OutputInt.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busInModel.OutputInt.FormattedValue);
			node.Table.Rows.Add(row);

			// Output Dfb decimal
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busInModel.OutputDFBDec.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busInModel.OutputDFBDec.FormattedValue);
			node.Table.Rows.Add(row);

			return node;
		}

		/// <summary>
		/// Generate a Mux0 node
		/// </summary>
		/// <param name="mux0"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public RecordNode Mux_0(Mux0Model mux0, decimal? pos_X, decimal? pos_Y)
		{
			const int COL1MINWIDTH = 60;
			const int COL2MINWIDTH = 80;
			const int COL3MINWIDTH = 90;

			var shapeName = mux0.Name;

			var node = new RecordNode(shapeName);
			if (pos_X.HasValue && pos_Y.HasValue)
			{
				node.Pos_X = pos_X;
				node.Pos_Y = pos_Y;
			}
			node.Table.Border = 0;
			node.Table.Attributes.Add("CELLBORDER", "1");
			node.Table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, shapeName, 3, shapeName);
			node.Table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, mux0.Pipeline.Label);
			AddHeader2Cell(row, null, null, mux0.Pipeline.Value[0]);
			AddHeader2Cell(row, null, null, mux0.Pipeline.Value[1]);
			node.Table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux0.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux0.Cycles[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux0.Cycles[1].FormattedValue);
			node.Table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux0.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux0.Instructions[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux0.Instructions[1].FormattedValue);
			node.Table.Rows.Add(row);

			// Addr
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux0.Addresses[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux0.Addresses[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux0.Addresses[1].FormattedValue);
			node.Table.Rows.Add(row);

			// Output
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux0.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux0.Output.FormattedValue);
			node.Table.Rows.Add(row);

			return node;
		}

		/// <summary>
		/// Generate an ACU node
		/// </summary>
		/// <param name="acu"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public RecordNode ACU(ACUModel acu, decimal? pos_X, decimal? pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 100;

			var shapeName = acu.Name;

			var node = new RecordNode(shapeName);
			{
				node.Pos_X = pos_X;
				node.Pos_Y = pos_Y;
			}
			node.Table.Border = 0;
			node.Table.Attributes.Add("CELLBORDER", "1");
			node.Table.CellSpacing = 0;

			var row = new HtmlTableRow();
			AddHeader1Cell(row, shapeName, 2, shapeName);
			node.Table.Rows.Add(row);

			// reg
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, acu.Reg_reg.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, acu.Reg_reg.FormattedValue);
			node.Table.Rows.Add(row);

			// freg
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, acu.Reg_freg.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, acu.Reg_freg.FormattedValue);
			node.Table.Rows.Add(row);

			// lreg
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, acu.Reg_lreg.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, acu.Reg_lreg.FormattedValue);
			node.Table.Rows.Add(row);

			// mreg
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, acu.Reg_mreg.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, acu.Reg_mreg.FormattedValue);
			node.Table.Rows.Add(row);

			// flag_mod
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, acu.Reg_flag_mod.Label, false, true);
			AddDetailCell(row, COL1MINWIDTH, null, null, acu.Reg_flag_mod.FormattedValue);
			node.Table.Rows.Add(row);

			return node;
		}

		/// <summary>
		/// Generate a Mux 1 node
		/// </summary>
		/// <param name="mux1"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public RecordNode Mux_1(Mux1Model mux1, decimal? pos_X, decimal? pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 70;
			const int COL3MINWIDTH = 84;

			var shapeName = mux1.Name;

			var node = new RecordNode(shapeName);
			{
				node.Pos_X = pos_X;
				node.Pos_Y = pos_Y;
			}
			node.Table.Border = 0;
			node.Table.Attributes.Add("CELLBORDER", "1");
			node.Table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, shapeName, 3, shapeName);
			node.Table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, mux1.Pipeline.Label);
			AddHeader2Cell(row, null, null, mux1.Pipeline.Value[0]);
			AddHeader2Cell(row, null, null, mux1.Pipeline.Value[1]);
			node.Table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux1.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux1.Cycles[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux1.Cycles[1].FormattedValue);
			node.Table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux1.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux1.Instructions[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux1.Instructions[1].FormattedValue);
			node.Table.Rows.Add(row);

			// Output
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux1.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux1.Output.FormattedValue);
			node.Table.Rows.Add(row);

			return node;
		}

		/// <summary>
		/// Generate a DataRam node
		/// </summary>
		/// <param name="dataRam"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public RecordNode DataRam(DataRamModel dataRam, decimal? pos_X, decimal? pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 80;

			var shapeName = dataRam.Name;

			var node = new RecordNode(shapeName);
			{
				node.Pos_X = pos_X;
				node.Pos_Y = pos_Y;
			}
			node.Table.Border = 0;
			node.Table.Attributes.Add("CELLBORDER", "1");
			node.Table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, shapeName, 2, shapeName);
			node.Table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, dataRam.Pipeline.Label);
			AddHeader2Cell(row, null, null, dataRam.Pipeline.Value[0]);
			node.Table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, dataRam.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, dataRam.Cycles[0].FormattedValue);
			node.Table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, dataRam.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, dataRam.Instructions[0].FormattedValue);
			node.Table.Rows.Add(row);

			// ValueWrite
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, dataRam.ValueWrite.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, dataRam.ValueWrite.FormattedValue);
			node.Table.Rows.Add(row);

			// Address
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, dataRam.Address.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, dataRam.Address.FormattedValue);
			node.Table.Rows.Add(row);

			// Value
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, dataRam.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, dataRam.Output.FormattedValue);
			node.Table.Rows.Add(row);

			return node;
		}

		/// <summary>
		/// Generate a Mux 2 node
		/// </summary>
		/// <param name="mux2"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public RecordNode Mux_2(Mux2Model mux2, decimal? pos_X, decimal? pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 86;
			const int COL3MINWIDTH = 86;

			var shapeName = mux2.Name;

			var node = new RecordNode(shapeName);
			{
				node.Pos_X = pos_X;
				node.Pos_Y = pos_Y;
			}
			node.Table.Border = 0;
			node.Table.Attributes.Add("CELLBORDER", "1");
			node.Table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, shapeName, 3, shapeName);
			node.Table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, mux2.Pipeline.Label);
			AddHeader2Cell(row, null, null, mux2.Pipeline.Value[0]);
			AddHeader2Cell(row, null, null, mux2.Pipeline.Value[1]);
			node.Table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux2.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux2.Cycles[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux2.Cycles[1].FormattedValue);
			node.Table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux2.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux2.Instructions[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux2.Instructions[1].FormattedValue);
			node.Table.Rows.Add(row);

			// Input
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux2.Input0.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, WebUtility.HtmlEncode(mux2.Input0.FormattedValue));
			AddDetailCell(row, COL3MINWIDTH, null, null, WebUtility.HtmlEncode(mux2.Input1.FormattedValue));
			node.Table.Rows.Add(row);

			// Input Source
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux2.Input0Src.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, WebUtility.HtmlEncode(mux2.Input0Src.FormattedValue));
			AddDetailCell(row, COL3MINWIDTH, null, null, WebUtility.HtmlEncode(mux2.Input1Src.FormattedValue));
			node.Table.Rows.Add(row);

			// Output
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux2.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux2.Output.FormattedValue);
			node.Table.Rows.Add(row);

			return node;
		}

		/// <summary>
		/// Generate a MAC node
		/// </summary>
		/// <param name="mac"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public RecordNode MAC(MACModel mac, decimal? pos_X, decimal? pos_Y)
		{
			const int COL1MINWIDTH = 80;
			const int COL2MINWIDTH = 80;
			const int COL3MINWIDTH = 110;
			const int COL4MINWIDTH = 120;

			var shapeName = mac.Name;

			var node = new RecordNode(shapeName);
			{
				node.Pos_X = pos_X;
				node.Pos_Y = pos_Y;
			}
			node.Table.Border = 0;
			node.Table.Attributes.Add("CELLBORDER", "1");
			node.Table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, shapeName, 4, shapeName);
			node.Table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, mac.Pipeline.Label);
			AddHeader2Cell(row, null, null, mac.Pipeline.Value[0]);
			AddHeader2Cell(row, null, null, mac.Pipeline.Value[1]);
			AddHeader2Cell(row, null, null, mac.Pipeline.Value[2]);
			node.Table.Rows.Add(row);

			// Cycle row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mac.Cycles[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mac.Cycles[1].FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.Cycles[2].FormattedValue);
			node.Table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mac.Instructions[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mac.Instructions[1].FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.Instructions[2].FormattedValue);
			node.Table.Rows.Add(row);

			// a term row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.PipelineItems[0].A.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, mac.PipelineItems[1].A.FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.PipelineItems[2].A.FormattedValue);
			node.Table.Rows.Add(row);

			// b term row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.PipelineItems[0].B.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, mac.PipelineItems[1].B.FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.PipelineItems[2].B.FormattedValue);
			node.Table.Rows.Add(row);

			// Accumulator
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.PipelineItems[0].Accumulator.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.PipelineItems[2].Accumulator.FormattedValue);
			node.Table.Rows.Add(row);

			// ALU row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.PipelineItems[0].AluValue.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, mac.PipelineItems[1].AluValue.FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.PipelineItems[2].AluValue.FormattedValue);
			node.Table.Rows.Add(row);

			// Output Formula op row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.PipelineItems[0].OutputFormula.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.PipelineItems[2].OutputFormula.FormattedValue);
			node.Table.Rows.Add(row);

			// Output row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mac.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL4MINWIDTH, null, null, mac.Output.FormattedValue);
			node.Table.Rows.Add(row);

			return node;
		}

		/// <summary>
		/// Generate a Mux 3 node
		/// </summary>
		/// <param name="mux3"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public RecordNode Mux_3(Mux3Model mux3, decimal? pos_X, decimal? pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 70;
			const int COL3MINWIDTH = 90;

			var shapeName = mux3.Name;

			var node = new RecordNode(shapeName);
			{
				node.Pos_X = pos_X;
				node.Pos_Y = pos_Y;
			}
			node.Table.Border = 0;
			node.Table.Attributes.Add("CELLBORDER", "1");
			node.Table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, shapeName, 3, shapeName);
			node.Table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, mux3.Pipeline.Label);
			AddHeader2Cell(row, null, null, mux3.Pipeline.Value[0]);
			AddHeader2Cell(row, null, null, mux3.Pipeline.Value[1]);
			node.Table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux3.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux3.Cycles[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux3.Cycles[1].FormattedValue);
			node.Table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux3.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, mux3.Instructions[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux3.Instructions[1].FormattedValue);
			node.Table.Rows.Add(row);

			// Output
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, mux3.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, mux3.Output.FormattedValue);
			node.Table.Rows.Add(row);

			return node;
		}

		/// <summary>
		/// Generate an ALU node
		/// </summary>
		/// <param name="alu"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public RecordNode ALU(ALUModel alu, decimal? pos_X, decimal? pos_Y)
		{
			const int COL1MINWIDTH = 100;
			const int COL2MINWIDTH = 90;
			const int COL3MINWIDTH = 90;
			const int COL4MINWIDTH = 90;

			var shapeName = alu.Name;

			var node = new RecordNode(shapeName);
			{
				node.Pos_X = pos_X;
				node.Pos_Y = pos_Y;
			}
			node.Table.Border = 0;
			node.Table.Attributes.Add("CELLBORDER", "1");
			node.Table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, shapeName, 4, shapeName);
			node.Table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, alu.Pipeline.Label);
			AddHeader2Cell(row, null, null, alu.Pipeline.Value[0]);
			AddHeader2Cell(row, null, null, alu.Pipeline.Value[1]);
			AddHeader2Cell(row, null, null, alu.Pipeline.Value[2]);
			node.Table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, alu.Cycles[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, alu.Cycles[1].FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, alu.Cycles[2].FormattedValue);
			node.Table.Rows.Add(row);

			// Instr row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, WebUtility.HtmlEncode(alu.Instructions[0].FormattedValue));
			AddDetailCell(row, COL3MINWIDTH, null, null, WebUtility.HtmlEncode(alu.Instructions[1].FormattedValue));
			AddDetailCell(row, COL4MINWIDTH, null, null, WebUtility.HtmlEncode(alu.Instructions[2].FormattedValue));
			node.Table.Rows.Add(row);

			// Input A
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.PipelineItems[0].InputA.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, alu.PipelineItems[1].InputA.FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, alu.PipelineItems[2].InputA.FormattedValue);
			node.Table.Rows.Add(row);

			// Input A Source
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.PipelineItems[0].InputASource.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, alu.PipelineItems[1].InputASource.FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, alu.PipelineItems[2].InputASource.FormattedValue);
			node.Table.Rows.Add(row);

			// Input B
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.PipelineItems[0].InputB.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, alu.PipelineItems[1].InputB.FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, alu.PipelineItems[2].InputB.FormattedValue);
			node.Table.Rows.Add(row);

			// Input B Source
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.PipelineItems[0].InputBSource.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, alu.PipelineItems[1].InputBSource.FormattedValue);
			AddDetailCell(row, COL4MINWIDTH, null, null, alu.PipelineItems[2].InputBSource.FormattedValue);
			node.Table.Rows.Add(row);

			// Output formula
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.PipelineItems[0].OutputFormula.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, "", true);
			AddDetailCell(row, COL3MINWIDTH, null, null, WebUtility.HtmlEncode(alu.PipelineItems[1].OutputFormula.FormattedValue));
			AddDetailCell(row, COL4MINWIDTH, null, null, WebUtility.HtmlEncode(alu.PipelineItems[2].OutputFormula.FormattedValue));
			node.Table.Rows.Add(row);

			// Output row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, alu.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, "", true);
			AddDetailCell(row, COL3MINWIDTH, null, null, "", true);
			AddDetailCell(row, COL4MINWIDTH, null, null, alu.Output.FormattedValue);
			node.Table.Rows.Add(row);

			return node;
		}

		/// <summary>
		/// Generate a Shifter node
		/// </summary>
		/// <param name="shifter"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public RecordNode Shifter(ShifterModel shifter, decimal? pos_X, decimal? pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 80;
			const int COL3MINWIDTH = 90;

			var shapeName = shifter.Name;

			var node = new RecordNode(shapeName);
			{
				node.Pos_X = pos_X;
				node.Pos_Y = pos_Y;
			}
			node.Table.Border = 0;
			node.Table.Attributes.Add("CELLBORDER", "1");
			node.Table.CellSpacing = 0;

			var row = new HtmlTableRow();
			AddHeader1Cell(row, shapeName, 3, shapeName);
			node.Table.Rows.Add(row);

			// Header 2
			row = new HtmlTableRow();
			AddHeader2Cell(row, null, null, shifter.Pipeline.Label);
			AddHeader2Cell(row, null, null, shifter.Pipeline.Value[0]);
			AddHeader2Cell(row, null, null, shifter.Pipeline.Value[1]);
			node.Table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, shifter.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, shifter.Cycles[0].FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, shifter.Cycles[1].FormattedValue);
			node.Table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, shifter.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, WebUtility.HtmlEncode(shifter.Instructions[0].FormattedValue));
			AddDetailCell(row, COL3MINWIDTH, null, null, WebUtility.HtmlEncode(shifter.Instructions[1].FormattedValue));
			node.Table.Rows.Add(row);

			// Input
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, shifter.Input.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, shifter.Input.FormattedValue);
			node.Table.Rows.Add(row);

			// Output
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, shifter.Output.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, null, true);
			AddDetailCell(row, COL3MINWIDTH, null, null, shifter.Output.FormattedValue);
			node.Table.Rows.Add(row);

			return node;
		}

		/// <summary>
		/// Generate a Bus Out node
		/// </summary>
		/// <param name="busOut"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public RecordNode HoldBusOut(BusOutModel busOut, decimal? pos_X, decimal? pos_Y)
		{
			const int COL1MINWIDTH = 50;
			const int COL2MINWIDTH = 80;

			var shapeName = busOut.Name;

			var node = new RecordNode(shapeName);
			if (pos_X.HasValue && pos_Y.HasValue)
			{
				node.Pos_X = pos_X;
				node.Pos_Y = pos_Y;
			}
			node.Table.Border = 0;
			node.Table.Attributes.Add("CELLBORDER", "1");
			node.Table.CellSpacing = 0;

			var row = new HtmlTableRow();
			AddHeader1Cell(row, shapeName, 2, shapeName);
			node.Table.Rows.Add(row);

			// Cycle
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busOut.Cycles[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busOut.Cycles[0].FormattedValue);
			node.Table.Rows.Add(row);

			// Instruction
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busOut.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busOut.Instructions[0].FormattedValue);
			node.Table.Rows.Add(row);

			// Output hex
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busOut.OutputHex.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busOut.OutputHex.FormattedValue);
			node.Table.Rows.Add(row);

			// Output int
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busOut.OutputInt.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busOut.OutputInt.FormattedValue);
			node.Table.Rows.Add(row);

			// Output Dfb decimal
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, busOut.OutputDFBDec.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, busOut.OutputDFBDec.FormattedValue);
			node.Table.Rows.Add(row);

			return node;
		}

		/// <summary>
		/// Generate a Global node
		/// </summary>
		/// <param name="global"></param>
		/// <param name="pos_X"></param>
		/// <param name="pos_Y"></param>
		/// <returns></returns>
		public RecordNode Global(GlobalModel global, decimal? pos_X, decimal? pos_Y)
		{
			const int COL1MINWIDTH = 100;
			const int COL2MINWIDTH = 80;
			const int COL3MINWIDTH = 100;
			const int COL4MINWIDTH = 80;

			var shapeName = global.Name;

			var node = new RecordNode(shapeName);
			{
				node.Pos_X = pos_X;
				node.Pos_Y = pos_Y;
			}
			node.Table.Border = 0;
			node.Table.Attributes.Add("CELLBORDER", "1");
			node.Table.CellSpacing = 0;

			// Header 1
			var row = new HtmlTableRow();
			AddHeader1Cell(row, shapeName, 4, shapeName);
			node.Table.Rows.Add(row);

			// Instr row
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, global.Instructions[0].Label, false, true);
			AddDetailCell(row, COL2MINWIDTH + COL3MINWIDTH + COL4MINWIDTH, null, 3, global.Instructions[0].FormattedValue);
			node.Table.Rows.Add(row);

			// Global_en0
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, global.Global_en0.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, global.Global_en0.FormattedValue);
			// rflag
			AddDetailCell(row, COL3MINWIDTH, null, null, global.Rflag.Label, false, true);
			AddDetailCell(row, COL4MINWIDTH, null, null, global.Rflag.FormattedValue);
			node.Table.Rows.Add(row);

			// Global_en1
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, global.Global_en1.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, global.Global_en1.FormattedValue);

			// tflag
			AddDetailCell(row, COL3MINWIDTH, null, null, global.Tflag.Label, false, true);
			AddDetailCell(row, COL4MINWIDTH, null, null, global.Tflag.FormattedValue);
			node.Table.Rows.Add(row);

			// Global_en2
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, global.Global_en2.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, global.Global_en2.FormattedValue);
			// Tsign
			AddDetailCell(row, COL3MINWIDTH, null, null, global.Tsign.Label, false, true);
			AddDetailCell(row, COL4MINWIDTH, null, null, global.Tsign.FormattedValue);
			node.Table.Rows.Add(row);

			// SatEn
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, global.SatEn.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, global.SatEn.FormattedValue);
			// Sem_en0
			AddDetailCell(row, COL3MINWIDTH, null, null, global.Sem_en0.Label, false, true);
			AddDetailCell(row, COL4MINWIDTH, null, null, global.Sem_en0.FormattedValue);
			node.Table.Rows.Add(row);

			// Sqcnt
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, global.Sqcnt.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, global.Sqcnt.FormattedValue);
			// Sem_en1
			AddDetailCell(row, COL3MINWIDTH, null, null, global.Sem_en1.Label, false, true);
			AddDetailCell(row, COL4MINWIDTH, null, null, global.Sem_en1.FormattedValue);
			node.Table.Rows.Add(row);

			// Sqcval
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, global.Sqcval.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, global.Sqcval.FormattedValue);
			// Sem_en2
			AddDetailCell(row, COL3MINWIDTH, null, null, global.Sem_en2.Label, false, true);
			AddDetailCell(row, COL4MINWIDTH, null, null, global.Sem_en2.FormattedValue);
			node.Table.Rows.Add(row);

			// Satflag
			row = new HtmlTableRow();
			AddDetailCell(row, COL1MINWIDTH, null, null, global.Satflag.Label, false, true);
			AddDetailCell(row, COL2MINWIDTH, null, null, global.Satflag.FormattedValue);
			AddDetailCell(row, COL3MINWIDTH, null, null, null, false, true);
			AddDetailCell(row, COL4MINWIDTH, null, null, null);
			node.Table.Rows.Add(row);

			return node;
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
		public HiddenNode MakeHiddenNode(string id, decimal? pos_X, decimal? pos_Y, decimal? width, decimal? height)
		{
			var node = new HiddenNode(id);
			node.Pos_X = pos_X;
			node.Pos_Y = pos_Y;
			node.Width = width;
			node.Height = height;
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
		private void AddConnection(StringBuilder sb, Connection conn)
		{
			// Edge definition
			string fromPortPos = "";
			string toPortPos = "";
			
			sb.AppendFormat("{0}{1} -> {2}{3}", conn.From.ToString(), fromPortPos, conn.To.ToString(), toPortPos);

			// Styling
			sb.Append(" [");

			// label
			if (conn.FromPortActive == PortStatus.Active && !string.IsNullOrEmpty(conn.FromPortActiveText))
			{
				sb.AppendFormat(" xlabel = \"{0}\"", conn.FromPortActiveText);
				//sb.AppendFormat(" xlabeldistance = \"{0}\"", "0.75");

			}
			else if (conn.FromPortActive == PortStatus.Inactive && !string.IsNullOrEmpty(conn.FromPortInactiveText))
			{
				sb.AppendFormat(" label = \"{0}\"", conn.FromPortInactiveText);
				//sb.AppendFormat(" labeldistance = \"{0}\"", "0.75");
			}
						
				// default
				sb.AppendFormat(" dir = \"{0}\"", DirType.forward.ToString());

			// color
			
				// default
				if (conn.FromPortActive == PortStatus.Active
					&& conn.ToPortActive == PortStatus.Active)
				{
					// source Active, target = Active:
					switch (conn.BusType)
					{
						case BusType.Address:
							sb.AppendFormat(" color = \"{0}\"", styleColor_AddressBus_Active);
							break;
						case BusType.Data:
							sb.AppendFormat(" color = \"{0}\"", styleColor_DataBus_Active);
							break;
					}
					sb.AppendFormat(" penwidth = \"{0}\"", stylePenWidth_Bus_Active);
					//sb.AppendFormat(" arrowsize = \"{0}\"", 2.0);
					//sb.AppendFormat(" arrowhead = \"{0}\"", "normal");
				}
				else if (conn.FromPortActive == PortStatus.Active
					&& conn.ToPortActive == PortStatus.Inactive)
				{
					// source Active, target = Inactive:
					switch (conn.BusType)
					{
						case BusType.Address:
							sb.AppendFormat(" color = \"{0}\"", styleColor_AddressBus_Inactive);
							break;
						case BusType.Data:
							sb.AppendFormat(" color = \"{0}\"", styleColor_DataBus_Inactive);
							break;
					}
					//sb.AppendFormat(" arrowhead = \"{0}\"", "tee");
				}
				else
				{
					// source Inactive, target = Inactive:
					sb.AppendFormat(" color = \"{0}\"", styleColor_Bus_Inactive);
					sb.AppendFormat(" labelfontcolor = \"{0}\"", styleColor_Bus_Inactive);
					//sb.AppendFormat(" arrowhead = \"{0}\"", "tee");
				}
			

			//// weight
			//if (conn.Weight.HasValue)
			//{
			//	// specified
			//	sb.AppendFormat(" weight = \"{0}\"", conn.Weight.Value);
			//}
			//else
			//{
			//	// default
			//}

			//if (conn.Length.HasValue)
			//{
			//	// specified
			//	sb.AppendFormat(" len = \"{0}\"", conn.Length.Value);
			//}

			//// pos
			//if (conn.EdgePos_X != null && conn.EdgePos_Y != null)
			//{
			//	// specified
			//	sb.AppendFormat("pos = \"{0},{1}!\"\n", conn.EdgePos_X.Value, conn.EdgePos_Y.Value);
			//}
			//else
			//{
			//	// defaults
			//}

			sb.AppendLine("];");
		}

#endregion
	}
}
