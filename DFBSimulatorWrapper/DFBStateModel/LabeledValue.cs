using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.DFBStateModel
{
	public class LabeledValue<T>
	{
		/// <summary>
		/// Label for the value
		/// </summary>
		public readonly string Label;

		/// <summary>
		/// Value
		/// </summary>
		public T Value;

		// Formatted value
		public string FormattedValue;

		public LabeledValue(string label)
		{
			Label = label;
		}
	}
}
