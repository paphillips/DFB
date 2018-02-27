using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	public class NodeAttributes
	{
		public int Indentation;

		public escString URL;
		public double? area;
		public KnownColor? color;
		public string colorscheme;
		public string comment;
		public double? distortion;
		public KnownColor? fillcolor;
		public bool? fixedsize;
		public KnownColor? fontcolor;
		public string fontname;
		public double? fontsize;
		public int? gradientangle;
		public string group;
		public double? height;
		public escString href;
		public escString id;
		public string image;
		public string imagepos;
		public bool? imagescale;
		public lblString label;
		public string labelloc;
		public layerRange layer;
		public double? margin;
		public bool? nojustify;
		public string ordering;
		public double? orientation;
		public double? penwidth;
		public int? peripheries;
		public bool? pin;
		public Point pos;
		public Rect rects;
		public bool? regular;
		public string root;
		public int? samplepoints;
		public Shape? shape;
		public string shapefile;
		public int? showboxes;
		public int? sides;
		public double? skew;
		public int? sortv;
		public Style.Node? style;
		public escString target;
		public escString tooltip;
		public PointList vertices;
		public double? width;
		public lblString xlabel;
		public Point xlp;
		public double? z;

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append("[");

			URL.AppendNameValue(sb, nameof(URL));
			area.AppendNameValue(sb, nameof(area));
			color.AppendNameValue(sb, nameof(color));
			colorscheme.AppendNameValue(sb, nameof(colorscheme));
			comment.AppendNameValue(sb, nameof(comment));
			distortion.AppendNameValue(sb, nameof(distortion));
			fillcolor.AppendNameValue(sb, nameof(fillcolor));
			fixedsize.AppendNameValue(sb, nameof(fixedsize));
			fontcolor.AppendNameValue(sb, nameof(fontcolor));
			fontname.AppendNameValue(sb, nameof(fontname));
			fontsize.AppendNameValue(sb, nameof(fontsize));
			gradientangle.AppendNameValue(sb, nameof(gradientangle));
			group.AppendNameValue(sb, nameof(group));
			height.AppendNameValue(sb, nameof(height));
			href.AppendNameValue(sb, nameof(href));
			id.AppendNameValue(sb, nameof(id));
			image.AppendNameValue(sb, nameof(image));
			imagepos.AppendNameValue(sb, nameof(imagepos));
			imagescale.AppendNameValue(sb, nameof(imagescale));
			label.AppendNameValue(sb, nameof(label));
			labelloc.AppendNameValue(sb, nameof(labelloc));
			layer.AppendNameValue(sb, nameof(layer));
			margin.AppendNameValue(sb, nameof(margin));
			ordering.AppendNameValue(sb, nameof(ordering));
			orientation.AppendNameValue(sb, nameof(orientation));
			penwidth.AppendNameValue(sb, nameof(penwidth));
			peripheries.AppendNameValue(sb, nameof(peripheries));
			pos.AppendNameValue(sb, nameof(pos));
			rects.AppendNameValue(sb, nameof(rects));
			regular.AppendNameValue(sb, nameof(regular));
			root.AppendNameValue(sb, nameof(root));
			samplepoints.AppendNameValue(sb, nameof(samplepoints));
			shape.AppendNameValue(sb, nameof(shape));
			shapefile.AppendNameValue(sb, nameof(shapefile));
			showboxes.AppendNameValue(sb, nameof(showboxes));
			sides.AppendNameValue(sb, nameof(sides));
			skew.AppendNameValue(sb, nameof(skew));
			sortv.AppendNameValue(sb, nameof(sortv));
			style.AppendNameValue(sb, nameof(style));
			target.AppendNameValue(sb, nameof(target));
			tooltip.AppendNameValue(sb, nameof(tooltip));
			vertices.AppendNameValue(sb, nameof(vertices));
			width.AppendNameValue(sb, nameof(width));
			xlabel.AppendNameValue(sb, nameof(xlabel));
			xlp.AppendNameValue(sb, nameof(xlp));
			z.AppendNameValue(sb, nameof(z));
						
			sb.Append(" ]\n");

			return sb.ToString();
		}


	}
}
