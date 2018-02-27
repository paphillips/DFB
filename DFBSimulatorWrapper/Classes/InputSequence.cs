using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper
{
	public class InputSequence
	{
		public int Cycle { get; set; }
		public bool GlobalInput1 { get; set; }
		public bool GlobalInput2 { get; set; }
		public bool Semaphore0 { get; set; }
		public bool Semaphore1 { get; set; }
		public bool Semaphore2 { get; set; }
	}
}
