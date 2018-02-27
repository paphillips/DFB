using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	public class Point
	{
		public double X;
		public double Y;
		public double? Z;
		public bool? InputOnly;

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(X.ToString());
			sb.Append(",");
			sb.Append(Y.ToString());
			if(Z.HasValue)
			{
				sb.Append(",");
				sb.Append(Z.ToString());
			}
			if(InputOnly.HasValue && InputOnly.Value)
			{
				sb.Append("!");
			}
			return sb.ToString();
		}
	}
}
