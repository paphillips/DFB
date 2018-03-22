using DFB_v1_40.Simulator;
using DFBSimulatorWrapper.DFBStateModel;
using DFBSimulatorWrapper.Diagram.CallDiagram;
using DFBSimulatorWrapper.Diagram.StateDiagram;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DFBSimulatorWrapper
{
	/// <summary>
	/// Manages the presentation of the Digital Filter Block simulator state frames
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class DFBState : IDisposable
	{
		#region Members

		private List<CodeState> codeStates;
		private static CodeStoreModel codeStore;
		private string callDiagramSvg;
		private static Wrapper dfbSimulatorWrapper;
		private List<DFBStateFrame> stateFrames;

		public List<CodeState> CodeStates => codeStates;

		/// <summary>
		/// Current value format
		/// </summary>
		public static DFBValueFormat ValueFormat;

		/// <summary>
		/// List of state frames for each cycle
		/// </summary>
		public List<DFBStateFrame> StateFrames => stateFrames;

		/// <summary>
		/// Call diagram
		/// </summary>
		public string CallDiagramSvg => callDiagramSvg;

		#endregion
		#region Constructor

		public DFBState(
			Wrapper dfbSimulatorWrapper,
			DFBValueFormat valueFormat)
		{
			DFBState.dfbSimulatorWrapper = dfbSimulatorWrapper;
			ValueFormat = valueFormat;
			stateFrames = new List<DFBStateFrame>();
			codeStore = new CodeStoreModel(DFBState.dfbSimulatorWrapper);

			// Build list of states and instructions
			codeStates = new List<CodeState>();
			codeStates.AddRange(CodeStoreModel.InstructionStateList.Select(x => new CodeState(x)));
		}

		#endregion
		#region Methods

		/// <summary>
		/// Resizes the SVG diagrams to fit the current DPI and container dimensions
		/// </summary>
		/// <param name="containerPx"></param>
		/// <param name="dpiX"></param>
		/// <param name="dpiY"></param>
		public void Resize(
				Rectangle containerPx,
				float dpiX,
				float dpiY)
		{
			foreach (var frame in stateFrames)
			{
				frame.Resize(containerPx, dpiX, dpiY);
			}
		}

		/// <summary>
		/// Map the current cycle to the Bus1Out line
		/// </summary>
		/// <returns></returns>
		public Dictionary<int, int> Bus1CycleToOutputline(bool showAllCycles)
		{
			var rtnDict = new Dictionary<int, int>();
			var outValLineCount = 0;
			for (int i = 0; i < stateFrames.Count; i++)
			{
				if (showAllCycles)
				{
					rtnDict.Add(i, i);
				}
				else
				{
					if (stateFrames[i].Hold_A.Output.Value.HasValue)
					{
						rtnDict.Add(i, outValLineCount);
						outValLineCount++;
					}
				}
			}
			return rtnDict;
		}

		/// <summary>
		/// Returns a dictionary representing the cycles and the bus output
		/// </summary>
		/// <param name="format">Format for output</param>
		/// <returns></returns>
		public string Bus1Out(DFBValueFormat format, bool showAllCycles)
		{
			var sb = new StringBuilder();

			for (int i = 0; i < stateFrames.Count; i++)
			{
				string value = null;
				switch (format)
				{
					case DFBValueFormat.Hex:
						value = stateFrames[i].Hold_A.OutputHex.FormattedValue;
						break;

					case DFBValueFormat.Int:
						value = stateFrames[i].Hold_A.OutputInt.FormattedValue;
						break;

					case DFBValueFormat.q23Decimal:
						value = stateFrames[i].Hold_A.OutputDFBDec.FormattedValue;
						break;
				}
				if (!showAllCycles && string.IsNullOrEmpty(value)) { continue; }

				sb.AppendFormat("{0:6}\t{1}\n", i.ToString().PadLeft(4), value);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Map the current cycle to the Bus1Out line
		/// </summary>
		/// <returns></returns>
		public Dictionary<int, int> Bus2CycleToOutputline(bool showAllCycles)
		{
			var rtnDict = new Dictionary<int, int>();
			var outValLineCount = 0;
			for (int i = 0; i < stateFrames.Count; i++)
			{
				if (showAllCycles)
				{
					rtnDict.Add(i, i);
				}
				else
				{
					if (stateFrames[i].Hold_B.Output.Value.HasValue)
					{
						rtnDict.Add(i, outValLineCount);
						outValLineCount++;
					}
				}
			}
			return rtnDict;

		}

		/// <summary>
		/// Returns a string representing the cycles and the bus output
		/// </summary>
		/// <param name="format">Format for output</param>
		/// <returns></returns>
		public string Bus2Out(DFBValueFormat format, bool showAllCycles)
		{
			var sb = new StringBuilder();

			for (int i = 0; i < stateFrames.Count; i++)
			{
				string value = null;
				switch (format)
				{
					case DFBValueFormat.Hex:
						value = stateFrames[i].Hold_B.OutputHex.FormattedValue;
						break;

					case DFBValueFormat.Int:
						value = stateFrames[i].Hold_B.OutputInt.FormattedValue;
						break;

					case DFBValueFormat.q23Decimal:
						value = stateFrames[i].Hold_B.OutputDFBDec.FormattedValue;
						break;
				}
				if (!showAllCycles && string.IsNullOrEmpty(value)) { continue; }

				sb.AppendFormat("{0:6}\t{1}\n", i.ToString().PadLeft(4), value);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Generates a single state frame
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		public static DFBStateFrame GenerateFrame(
			Wrapper dfbSimulatorWrapper,
			int cycle,
			Rectangle containerPx,
			float dpiX,
			float dpiY)
		{
			var gv = new StateDiagram();
			return new DFBStateFrame(cycle, DFBState.dfbSimulatorWrapper, gv, containerPx, dpiX, dpiY);
		}

		/// <summary>
		/// Generates the call diagram
		/// </summary>
		public void GenerateCallDiagram()
		{
			var diagram = new CallDiagram();

			var bytes = diagram.Generate(this);
			if (bytes == null || bytes.Length == 0)
			{
				throw new Exception("GraphVix render failed, check syntax");
			}

			var svgDoc = Encoding.UTF8.GetString(bytes);

			// strip the xml header
			var doc = XDocument.Parse(svgDoc, LoadOptions.PreserveWhitespace);
			var node = doc.Descendants().FirstOrDefault();
			callDiagramSvg = node.ToString();

			diagram = null;
			bytes = null;
		}

		/// <summary>
		/// Retrieves the private field value from the current simulator object
		/// </summary>
		/// <param name="cycle">Simulator cycle number for which to retrieve value
		/// <param name="memberName">Private field to retrieve</param>
		/// <returns></returns>
		public static T GetCySimulatorPrivateField<T>(int cycle, string memberName)
		{
			var simulator = dfbSimulatorWrapper.GetSimulatorAtCycle(cycle);
			if (simulator == null) { return default(T); }

			var val = PrivateValueAccessor.GetPrivateFieldValue(typeof(CySimulator), memberName, simulator);
			if (val == null) { return default(T); }

			return (T)val;
		}

		/// <summary>
		/// Retrieves the simulator object for a given cycle back from the current cycle
		/// </summary>
		/// <param name="cycleOffset">Number of simulator cycles backward or forward from current cycle. 0 = current cycle, -1 = prior cycle, +1 = next cycle, etc.</param>
		/// <returns></returns>
		public static CySimulator GetCySimulator(int cycle)
		{
			var simulator = dfbSimulatorWrapper.GetSimulatorAtCycle(cycle);
			if (simulator == null)
			{
				return null;
			}
			else
			{
				return simulator;
			}
		}

		/// <summary>
		/// Returns the prior (calling) state (label) for this cycle
		/// </summary>
		/// <param name="cycle"></param>
		/// <returns></returns>
		public DFBStateFrame StatePrior(int cycle)
		{
			if (cycle == 0) { return null; }

			// Search backward until the prior state is found
			return StateFrames
				.OrderByDescending(x => x.Cycle)
				.Where(x => x.Cycle < cycle
					&& (x.CodeStateCycle.Label != StateFrames[cycle].CodeStateCycle.Label
						|| x.CodeStateCycle.CodeState.State.Loop == 1)
					&& x.CodeStateCycle.IsJumpInstruction == true)
				.FirstOrDefault();
		}

		/// <summary>
		/// Returns the prior call of the current state label
		/// </summary>
		/// <param name="cycle"></param>
		/// <returns></returns>
		public DFBStateFrame StatePriorSameLabel(int cycle)
		{
			if (cycle == 0) { return null; }

			// Search backward until the prior state is found
			return StateFrames
				.OrderByDescending(x => x.Cycle)
				.Where(x => x.Cycle < cycle
					&& x.CodeStateCycle.Label == StateFrames[cycle].CodeStateCycle.Label
					&& x.CodeStateCycle.IsJumpInstruction == true)
				.FirstOrDefault();
		}

		/// <summary>
		/// Returns the next state after this cycle
		/// </summary>
		/// <param name="cycle"></param>
		/// <returns></returns>
		public DFBStateFrame StateNext(int cycle)
		{
			try
			{
				if(cycle == StateFrames.Count - 1) { return null; }

				// Search forward until the next state is found
				return StateFrames
					.OrderBy(x => x.Cycle)
					.Where(x => x.Cycle > cycle
						&& (x.CodeStateCycle.Label != StateFrames[cycle].CodeStateCycle.Label
							|| x.CodeStateCycle.CodeState.State.Loop == 1)
						&& x.CodeStateCycle.IsJumpInstruction == true)
					.FirstOrDefault();
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		/// <summary>
		/// Returns the next call of the current state label
		/// </summary>
		/// <param name="cycle"></param>
		/// <returns></returns>
		public DFBStateFrame StateNextSameLabel(int cycle)
		{
			// Search forward until the next state is found with the same label
			return StateFrames
				.OrderBy(x => x.Cycle)
				.Where(x => x.Cycle > cycle
					&& x.CodeStateCycle.Label == StateFrames[cycle].CodeStateCycle.Label
					&& x.CodeStateCycle.IsJumpInstruction == true)
				.FirstOrDefault();
		}

		#region IDisposable Support

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					dfbSimulatorWrapper.Dispose();
					for (int i = 0; i < stateFrames.Count; i++)
					{
						stateFrames[i] = null;
					}
					stateFrames = null;
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~DFBState() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}

		#endregion

		#endregion
	}
}
