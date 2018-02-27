using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using escString = System.String;

namespace DFBSimulatorWrapper.GraphViz
{
	public class EdgeAttributes
	{
		public int Indentation;

		public List<string> Connections;

		public escString URL;
		public ArrowType? arrowhead;
		public double? arrowsize;
		public ArrowType? arrowtail;
		public KnownColor? color;
		public string colorscheme;
		public string comment;
		public bool? constraint;
		public bool? decorate;
		public DirType? dir;
		public escString edgeURL;
		public escString edgehref;
		public escString edgetarget;
		public escString edgetooltip;
		public KnownColor? fontcolor;
		public string fontname;
		public double? fontsize;
		public escString headURL;
		public GraphViz.Point head_lp;
		public bool? headclip;
		public escString headhref;
		public escString headlabel;
		public PortPos? headport;
		public escString headtarget;
		public escString headtooltip;
		public escString label;
		public escString labelURL;
		public double? labelangle;
		public double? labeldistance;
		public bool? labelfloat;
		public KnownColor? labelfontcolor;
		public string labelfontname;
		public double? labelfontsize;
		public escString labelhref;
		public escString labeltarget;
		public escString labeltooltip;
		public string layer;
		public double? len;
		public string lhead;
		public GraphViz.Point lp;
		public string ltail;
		public int? minlen;
		public double? penwidth;
		public GraphViz.Point pos;
		public string samehead;
		public string sametail;
		public int? showboxes;
		public Style.Edge? style;
		public escString tailURL;
		public GraphViz.Point tail_lp;
		public bool? tailclip;
		public escString tailhref;
		public string taillabel;
		public PortPos? tailport;
		public escString tailtarget;
		public escString tailtooltip;
		public escString target;
		public double? weight;
		public string xlabel;

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append("[");

			URL.AppendNameValue(sb, nameof(URL));
			arrowhead.AppendNameValue(sb, nameof(arrowhead));
			arrowsize.AppendNameValue(sb, nameof(arrowsize));
			arrowtail.AppendNameValue(sb, nameof(arrowtail));
			color.AppendNameValue(sb, nameof(color));
			colorscheme.AppendNameValue(sb, nameof(colorscheme));
			comment.AppendNameValue(sb, nameof(comment));
			constraint.AppendNameValue(sb, nameof(constraint));
			decorate.AppendNameValue(sb, nameof(decorate));
			dir.AppendNameValue(sb, nameof(dir));
			edgeURL.AppendNameValue(sb, nameof(edgeURL));
			edgehref.AppendNameValue(sb, nameof(edgehref));
			edgetarget.AppendNameValue(sb, nameof(edgetarget));
			edgetooltip.AppendNameValue(sb, nameof(edgetooltip));
			fontcolor.AppendNameValue(sb, nameof(fontcolor));
			fontname.AppendNameValue(sb, nameof(fontname));
			fontsize.AppendNameValue(sb, nameof(fontsize));
			headURL.AppendNameValue(sb, nameof(headURL));
			head_lp.AppendNameValue(sb, nameof(head_lp));
			headclip.AppendNameValue(sb, nameof(headclip));
			headhref.AppendNameValue(sb, nameof(headhref));
			headlabel.AppendNameValue(sb, nameof(headlabel));
			headport.AppendNameValue(sb, nameof(headport));
			headtarget.AppendNameValue(sb, nameof(headtarget));
			headtooltip.AppendNameValue(sb, nameof(headtooltip));
			label.AppendNameValue(sb, nameof(label));
			labelURL.AppendNameValue(sb, nameof(labelURL));
			labelangle.AppendNameValue(sb, nameof(labelangle));
			labeldistance.AppendNameValue(sb, nameof(labeldistance));
			labelfloat.AppendNameValue(sb, nameof(labelfloat));
			labelfontcolor.AppendNameValue(sb, nameof(labelfontcolor));
			labelfontname.AppendNameValue(sb, nameof(labelfontname));
			labelfontsize.AppendNameValue(sb, nameof(labelfontsize));
			labelhref.AppendNameValue(sb, nameof(labelhref));
			labeltarget.AppendNameValue(sb, nameof(labeltarget));
			labeltooltip.AppendNameValue(sb, nameof(labeltooltip));
			layer.AppendNameValue(sb, nameof(layer));
			len.AppendNameValue(sb, nameof(len));
			lhead.AppendNameValue(sb, nameof(lhead));
			lp.AppendNameValue(sb, nameof(lp));
			ltail.AppendNameValue(sb, nameof(ltail));
			minlen.AppendNameValue(sb, nameof(minlen));
			penwidth.AppendNameValue(sb, nameof(penwidth));
			pos.AppendNameValue(sb, nameof(pos));
			samehead.AppendNameValue(sb, nameof(samehead));
			sametail.AppendNameValue(sb, nameof(sametail));
			showboxes.AppendNameValue(sb, nameof(showboxes));
			style.AppendNameValue(sb, nameof(style));
			tailURL.AppendNameValue(sb, nameof(tailURL));
			tail_lp.AppendNameValue(sb, nameof(tail_lp));
			tailclip.AppendNameValue(sb, nameof(tailclip));
			tailhref.AppendNameValue(sb, nameof(tailhref));
			taillabel.AppendNameValue(sb, nameof(taillabel));
			tailport.AppendNameValue(sb, nameof(tailport));
			tailtarget.AppendNameValue(sb, nameof(tailtarget));
			tailtooltip.AppendNameValue(sb, nameof(tailtooltip));
			target.AppendNameValue(sb, nameof(target));
			weight.AppendNameValue(sb, nameof(weight));
			xlabel.AppendNameValue(sb, nameof(xlabel));

			sb.Append(" ]\n");

			return sb.ToString();
		}
	}
}
