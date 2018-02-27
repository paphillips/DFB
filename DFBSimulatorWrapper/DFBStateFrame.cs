using DFBSimulatorWrapper.DFBStateModel;
using DFBSimulatorWrapper.Diagram.StateDiagram;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DFBSimulatorWrapper
{
	/// <summary>
	/// Represent a single cycle's state snapshot
	/// </summary>
	public class DFBStateFrame
	{
		private string diagramSvg;

		public string DiagramSvg
		{
			get
			{
				return diagramSvg;
			}
		}

		public readonly int Cycle;

		public readonly CodeStateCycle CodeStateCycle;

		public readonly JumpConditionModel JumpConditions;

		public readonly BusInModel Stage_A;
		public readonly BusInModel Stage_B;

		public readonly Mux0Model Mux0;

		public readonly ACUModel ACU_A;
		public readonly ACUModel ACU_B;

		public readonly Mux1Model Mux_1A;
		public readonly Mux1Model Mux_1B;

		public readonly DataRamModel DataRam_A;
		public readonly DataRamModel DataRam_B;

		public readonly Mux2Model Mux_2A;
		public readonly Mux2Model Mux_2B;

		public readonly MACModel MAC;

		public readonly Mux3Model Mux_3A;
		public readonly Mux3Model Mux_3B;

		public readonly ALUModel ALU;

		public readonly ShifterModel Shifter;

		public readonly BusOutModel Hold_A;
		public readonly BusOutModel Hold_B;

		public readonly GlobalModel Global;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cycle">Zero-based cycle number</param>
		/// <param name="dfbSimulatorWrapper">Wrapper reference</param>
		/// <param name="graphViz">GraphViz reference</param>
		/// <param name="containerPx">Window for SVG size and scaling</param>
		/// <param name="dpiX">Current display DPI - x</param>
		/// <param name="dpiY">Current display DPI - y</param>
		public DFBStateFrame(
			int cycle,
			Wrapper dfbSimulatorWrapper,
			StateDiagram graphViz,
			Rectangle containerPx,
			float dpiX,
			float dpiY)
		{
			Cycle = cycle;

			CodeStateCycle = new CodeStateCycle(cycle);

			Stage_A = new BusInModel(Bank.Bank_A, cycle);
			Stage_B = new BusInModel(Bank.Bank_B, cycle);

			Mux0 = new Mux0Model(cycle);
			ACU_A = new ACUModel(Bank.Bank_A, cycle, dfbSimulatorWrapper.Parameters.Code);
			ACU_B = new ACUModel(Bank.Bank_B, cycle, dfbSimulatorWrapper.Parameters.Code);

			Mux_1A = new Mux1Model(Bank.Bank_A, cycle);
			Mux_1B = new Mux1Model(Bank.Bank_B, cycle);

			DataRam_A = new DataRamModel(Bank.Bank_A, cycle, dfbSimulatorWrapper.Parameters.Code);
			DataRam_B = new DataRamModel(Bank.Bank_B, cycle, dfbSimulatorWrapper.Parameters.Code);

			Mux_2A = new Mux2Model(Bank.Bank_A, cycle);
			Mux_2B = new Mux2Model(Bank.Bank_B, cycle);

			MAC = new MACModel(cycle);

			Mux_3A = new Mux3Model(Bank.Bank_A, cycle);
			Mux_3B = new Mux3Model(Bank.Bank_B, cycle);

			ALU = new ALUModel(cycle);

			Shifter = new ShifterModel(cycle);

			Hold_A = new BusOutModel(Bank.Bank_A, cycle);
			Hold_B = new BusOutModel(Bank.Bank_B, cycle);

			Global = new GlobalModel(cycle);

			JumpConditions = new JumpConditionModel(cycle);

			// Generate diagram
			if (graphViz == null) { graphViz = new StateDiagram(); }

			var bytes = graphViz.Generate(this);
			if (bytes == null || bytes.Length == 0)
			{
				throw new Exception("GraphVix render failed, check syntax");
			}

			diagramSvg = Encoding.UTF8.GetString(bytes);
			bytes = null;
			Resize(containerPx, dpiX, dpiY);
		}

		/// <summary>
		/// Resizes the SVG diagram to fit the current DPI and container dimensions
		/// </summary>
		/// <param name="containerPx"></param>
		/// <param name="dpiX"></param>
		/// <param name="dpiY"></param>
		public void Resize(
			Rectangle containerPx,
			float dpiX,
			float dpiY)
		{
			// Update the SVG document to reflect the new size in points
			var doc = XDocument.Parse(this.diagramSvg, LoadOptions.PreserveWhitespace);
			var node = doc.Descendants().FirstOrDefault();

			var scalePlug = 1.1;
			var widthPt = containerPx.Width / dpiX * 72;
			var heightPt = containerPx.Height / dpiX * 72;
			node.SetAttributeValue("width", string.Format("{0}pt", widthPt / scalePlug));
			node.SetAttributeValue("height", string.Format("{0}pt", heightPt / scalePlug));

			this.diagramSvg = node.ToString();
		}
	}
}
