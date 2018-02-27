using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper
{
	public class SimProgressReport
	{
		public int CurrentStep { get; set; }
		public int TotalSteps { get; set; }
		public bool Cancelled { get; set; }
	}
}
