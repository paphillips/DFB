using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	public class Rect
	{
		public double X_LowerLeft;
		public double Y_LowerLeft;
		public double X_UpperRight;
		public double Y_UpperRight;

		public Rect(double x_LowerLeft, double y_LowerLeft, double x_UpperRight, double y_UpperRight)
		{
			X_LowerLeft = x_LowerLeft;
			Y_LowerLeft = y_LowerLeft;
			X_UpperRight = x_UpperRight;
			Y_UpperRight = y_UpperRight;
		}

		public override string ToString()
		{
			return string.Format("{0}{1}{2}{3}", X_LowerLeft, Y_LowerLeft, X_UpperRight, Y_UpperRight);
		}
	}
}
