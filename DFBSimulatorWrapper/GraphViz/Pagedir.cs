using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	/// <summary>
	/// Major-minor order for traversing a rectangular array: B = bottom, T = Top, L = Left, R = Right
	/// </summary>
	public enum Pagedir
	{
		BL,
		BR,
		TL,
		TR,
		RB,
		RT,
		LB,
		LT
	}
}
