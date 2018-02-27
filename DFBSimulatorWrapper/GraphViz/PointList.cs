using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	public class PointList
	{
		public List<Point> Points;

		public PointList()
		{
			Points = new List<Point>();
		}

		public override string ToString()
		{
			return string.Join(" ", Points);
		}
	}
}
