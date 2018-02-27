using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using escString = System.String;
using lblString = System.String;
using layerList = System.String;
using layerRange = System.String;
using System.Drawing;

namespace DFBSimulatorWrapper.GraphViz
{
	public class SubgraphAttributes
	{
		public int Indentation;

		public double? K;
		public escString URL;
		public double? area;
		public KnownColor? bgcolor;
		public KnownColor? color;
		public string colorscheme;
		public KnownColor? fillcolor;
		public KnownColor? fontcolor;
		public string fontname;
		public double? fontsize;
		public int? gradientangle;
		public escString href;
		public escString id;
		public lblString label;
		public string labeljust;
		public string labelloc;
		public layerRange layer;
		public double? lheight;
		public Point lp;
		public double? lwidth;
		public double? margin;
		public bool? nojustify;
		public KnownColor? pencolor;
		public double? penwidth;
		public int? peripheries;
		public int? sortv;
		public Style.Cluster? style;
		public escString target;
		public escString tooltip;

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendFormat("{0}[", new String('\t', Indentation));

			K.AppendNameValue(sb, nameof(K));
			URL.AppendNameValue(sb, nameof(URL));
			area.AppendNameValue(sb, nameof(area));
			bgcolor.AppendNameValue(sb, nameof(bgcolor));
			color.AppendNameValue(sb, nameof(color));
			colorscheme.AppendNameValue(sb, nameof(colorscheme));
			fillcolor.AppendNameValue(sb, nameof(fillcolor));
			fontcolor.AppendNameValue(sb, nameof(fontcolor));
			fontname.AppendNameValue(sb, nameof(fontname));
			fontsize.AppendNameValue(sb, nameof(fontsize));
			gradientangle.AppendNameValue(sb, nameof(gradientangle));
			href.AppendNameValue(sb, nameof(href));
			id.AppendNameValue(sb, nameof(id));
			label.AppendNameValue(sb, nameof(label));
			labeljust.AppendNameValue(sb, nameof(labeljust));
			labelloc.AppendNameValue(sb, nameof(labelloc));
			layer.AppendNameValue(sb, nameof(layer));
			lheight.AppendNameValue(sb, nameof(lheight));
			lp.AppendNameValue(sb, nameof(lp));
			lwidth.AppendNameValue(sb, nameof(lwidth));
			margin.AppendNameValue(sb, nameof(margin));
			nojustify.AppendNameValue(sb, nameof(nojustify));
			pencolor.AppendNameValue(sb, nameof(pencolor));
			penwidth.AppendNameValue(sb, nameof(penwidth));
			peripheries.AppendNameValue(sb, nameof(peripheries));
			sortv.AppendNameValue(sb, nameof(sortv));
			style.AppendNameValue(sb, nameof(style));
			target.AppendNameValue(sb, nameof(target));
			tooltip.AppendNameValue(sb, nameof(tooltip));

			sb.Append("]\n");

			return sb.ToString();
		}
	}
}
