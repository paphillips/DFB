using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	public class ViewPort
	{
		public double Width;
		public double Height;
		public double? Zoom;
		public double? Center_X_Points;
		public double? Center_Y_Points;
		public string Center_Node;

		public ViewPort(
			double width,
			double height,
			double? zoom,
			double? center_X_Points,
			double? center_Y_Points)
		{
			Width = width;
			Height = height;
			if (zoom.HasValue) { Zoom = zoom; }
			if (center_X_Points.HasValue) { Center_X_Points = center_X_Points; }
			if (center_Y_Points.HasValue) { Center_Y_Points = center_Y_Points; }
		}

		public ViewPort(
			double width,
			double height,
			double? zoom,
			string center_Node)
		{
			Width = width;
			Height = height;
			if (zoom.HasValue) { Zoom = zoom; }
			Center_Node = center_Node;
		}

		public override string ToString()
		{
			var vals = new List<string>();
			vals.Add(Width.ToString());
			vals.Add(Height.ToString());
			if(Zoom.HasValue) { vals.Add(Zoom.ToString()); }

			if (!string.IsNullOrEmpty(Center_Node))
			{
				vals.Add(Center_Node);
			}
			else if (Center_X_Points.HasValue && Center_Y_Points.HasValue)
			{
				vals.Add(Center_X_Points.Value.ToString());
				vals.Add(Center_Y_Points.Value.ToString());
			}

			return string.Join(",", vals);
		}
	}
}
