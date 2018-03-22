// defined: use a single thread
// not defined: thread count = (# logical processors on system - 1)
//#define useSingleThread

using AutoMapper;
using DFB_v1_40;
using DFB_v1_40.Asm;
using DFB_v1_40.Simulator;
using DFBSimulatorWrapper.DFBStateModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DFBProject;

namespace DFBSimulatorWrapper
{
	public class Wrapper : IDisposable
	{
		#region Constants


		#endregion
		#region Members

		private static bool mapperInitialized;
		private CyCodeTab cyCodeTab;
		private CySimulator cySimulator;
		private static CyDfbAsm cyDfbAsm;
		private CyParameters cyParameters;
		private List<InputSequence> inputSequence { get; set; }
		private int stepsRequested;
		private int stepsExecuted;
		private bool simExecuted;
		private bool simStepExecuted;
		private List<CySimulator> simulatorHistory;
		int batchSize;

		/// <summary>
		/// DFB assembler instance
		/// </summary>
		public static CyDfbAsm DfbAsm
		{
			get
			{
				return cyDfbAsm;
			}
		}

		/// <summary>
		/// DFM simulator parameters
		/// </summary>
		public CyParameters Parameters
		{
			get
			{
				return cyParameters;
			}
		}

		/// <summary>
		/// Number of cycles captured in this simulator
		/// </summary>
		public int SimulatorCyclesCaptured
		{
			get
			{
				return simulatorHistory.Count;
			}
		}

		#endregion
		#region Constructor

		/// <summary>
		/// Initialize the wrapper
		/// </summary>
		public Wrapper()
		{
			if (!mapperInitialized)
			{
				// Automapper to clone CySimulator object
				Mapper.Initialize(i =>
				{
					//i.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly || p.GetMethod.IsPrivate;
					i.ShouldMapProperty = p => false;
					i.ShouldMapField = p => true;
					i.CreateMap<CySimulator, CySimulator>();
					i.CreateMap<CyControlWord, CyControlWord>();
				});

				mapperInitialized = true;
			}

			simulatorHistory = new List<CySimulator>();

#if useSingleThread
			batchSize = 1;
#else
			batchSize = Environment.ProcessorCount > 1 ? Environment.ProcessorCount - 1 : 1;
#endif
		}

		#endregion
		#region Methods

		/// <summary>
		/// Run the simulations asynchronously
		/// </summary>
		/// <param name="parameters">Simulation parameters</param>
		/// <param name="numCycles">Number of cycles to simulate</param>
		/// <param name="progress">Progress report update object</param>
		/// <param name="cancellationToken">Cancellation token for UI</param>
		/// <param name="inputSequence">List of globals to set at specifiec cycle numbers</param>
		/// <param name="cyCodeTab">CyCodeTab instance</param>
		/// <param name="containerPx">Bounding box of diagram container</param>
		/// <param name="dpiX">DPI of system - x</param>
		/// <param name="dpiY">DPI of system - y</param>
		/// <returns></returns>
		public Task<DFBState> RunDFBSimulations(
			CyParameters parameters,
			int numCycles,
			IProgress<SimProgressReport> progress,
			CancellationToken cancellationToken,
			List<InputSequence> inputSequence,
			CyCodeTab cyCodeTab,
			Rectangle containerPx,
			float dpiX,
			float dpiY)
		{
			var progressReport = new SimProgressReport();
			this.inputSequence = inputSequence;
			stepsRequested = numCycles;
			cyParameters = parameters;

			// CyCodeTab must be instantiated on the WInForms side, with references to
			// UIFramework.Product4.WinForms and UIFramework.Product6.WinForms
			this.cyCodeTab = cyCodeTab;

			Assemble();

			// Create state view manager
			var dfbState = new DFBState(this, DFBValueFormat.q23Decimal);

			// Return on error
			var errors = cyDfbAsm.Errors.GetFullList();
			if (errors.Where(x => x.Type == CyMessageType.Error).Count() > 0)
			{
				return Task.Run(() => { return dfbState; });
			}

			// Check pending async cancel
			if (cancellationToken.IsCancellationRequested == true)
			{
				return Task.Run(() => { return dfbState; });
			}

			// Execute the simulation steps with status reports
			return Task.Run(() =>
			{
				// Execute DFB simulator for all cycles
				while (true)
				{
					bool sim;

					sim = SimulateStep();
					if (sim && stepsExecuted < numCycles)
					{
						stepsExecuted++;
					}
					else
					{
						break;
					}
				}

				progressReport.TotalSteps = simulatorHistory.Count;

				// Generate state frames in batches
				if (batchSize > simulatorHistory.Count) { batchSize = simulatorHistory.Count; }
				var batchStartIdx = 0;
				var batchEndIdx = batchStartIdx + batchSize;

				var framesTmp = new Dictionary<int, DFBStateFrame>();

				while (true)
				{
					// Check pending async cancel
					if (cancellationToken.IsCancellationRequested == true)
					{
						progressReport.Cancelled = true;
						progress.Report(progressReport);
						return dfbState;
					}

					// Generate a batch of frames
					framesTmp.Clear();
					var curBatch = batchStartIdx;
					Parallel.For(batchStartIdx, batchEndIdx, b =>
					{
						framesTmp.Add(b, DFBState.GenerateFrame(this, b, containerPx, dpiX, dpiY));
						curBatch++;
						progressReport.CurrentStep = curBatch;
						progress.Report(progressReport);
					});

					dfbState.StateFrames.AddRange(framesTmp
						.OrderBy(x => x.Key)
						.Select(x => x.Value));

					batchStartIdx = batchStartIdx + batchSize;
					if (batchStartIdx > simulatorHistory.Count - 1)
					{
						// Last batch finished all frames
						break;
					}

					batchEndIdx = batchEndIdx + batchSize;
					// Note that batchEndIdx is exclusive in Parallel.For()
					if (batchEndIdx > simulatorHistory.Count)
					{
						batchEndIdx = simulatorHistory.Count;
					}
				}

				return dfbState;
			});
		}

		/// <summary>
		/// Return the simulator object at the specified step
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns>CySimulator object if cycle exists, otherwise null</returns>
		public CySimulator GetSimulatorAtCycle(int cycle)
		{
			if (cycle < 0 || cycle > simulatorHistory.Count - 1) { return null; }

			return simulatorHistory[cycle];
		}

		/// <summary>
		/// Simulate a step
		/// </summary>
		/// <returns></returns>
		private bool SimulateStep()
		{
			bool simulationContinues = true;
			bool flagInitializationCycle = false;

			if (!this.simStepExecuted)
			{
				this.simStepExecuted = true;
				if (this.cySimulator == null)
				{
					this.Simulate(null, null);  // init
					flagInitializationCycle = true;
				}

				// Set global properties if user entered a value for this cycle
				if (inputSequence.Where(x => x.Cycle == stepsExecuted - 1).Any())
				{
					var globals = inputSequence.Where(x => x.Cycle == stepsExecuted - 1).First();
					cySimulator.GlobalInput1 = Convert.ToByte(globals.GlobalInput1);
					cySimulator.GlobalInput2 = Convert.ToByte(globals.GlobalInput2);
					cySimulator.Semaphore0 = Convert.ToByte(globals.Semaphore0);
					cySimulator.Semaphore1 = Convert.ToByte(globals.Semaphore1);
					cySimulator.Semaphore2 = Convert.ToByte(globals.Semaphore2);
				}

				if (this.cySimulator != null)
				{
					// MakeStep() is short-circuited on first execution
					if (flagInitializationCycle || this.cySimulator.MakeStep())
					{
						//this.propertyGridDebug.Refresh();
					}
					else
					{
						this.TryEndSimulation();
						simulationContinues = false;
					}
					this.DisplaySimulatorDebugInfo();
					if (!flagInitializationCycle && simulationContinues)
					{
						CaptureHistory();
					}
				}
				else
				{
					this.TryEndSimulation();
					simulationContinues = false;
				}
				this.simStepExecuted = false;
			}

			return simulationContinues;
		}

		/// <summary>
		/// Runs simulation
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="s"></param>
		private void Simulate(object sender, EventArgs s)
		{
			if (!this.simExecuted)
			{
				this.simExecuted = true;
				if (this.cySimulator == null)
				{
					if (cyDfbAsm != null /*&& this.ErrorsCount() <= 0*/)
					{
						int[] values = GetBusInValues(cyParameters.Bus1_data);
						int[] values2 = GetBusInValues(cyParameters.Bus2_data);
						this.cySimulator = new CySimulator();
						bool flag = this.cySimulator.InitializeSimulator(values, values2, cyDfbAsm.GetRamStringArrays());

						if (sender != null && flag)
						{
							this.cySimulator.SimulateAllSteps(this.TryEndSimulation);
						}
						this.simExecuted = false;
						return;
					}

					this.simExecuted = false;
					return;
				}
				this.TryEndSimulation();
				this.simExecuted = false;
				return;
			}
			return;
		}

		/// <summary>
		/// Runs the assembler
		/// </summary>
		private void Assemble()
		{
			cyDfbAsm = new CyDfbAsm();
			cyDfbAsm.Compile(cyParameters.Code, cyParameters.OptimizeAssembly);
		}

		/// <summary>
		/// Reads the bus in values
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private int[] GetBusInValues(string text)
		{
			List<int> list = new List<int>();
			StringReader stringReader = new StringReader(text);

			string text2 = stringReader.ReadLine();
			while (!string.IsNullOrEmpty(text2))
			{
				uint uIntValue = CyParameters.GetUIntValue(text2);
				if (uIntValue > 16777216)
				{
					throw new ArgumentOutOfRangeException(string.Format(
						"Out of range input value detected: {0} ({1} exceeds maximum value of {2}",
						text2,
						uIntValue,
						16777216));
				}
				list.Add((int)uIntValue);
				text2 = stringReader.ReadLine();
			}

			return list.ToArray();
		}

		/// <summary>
		/// Try to end the simulation
		/// </summary>
		private void TryEndSimulation()
		{
			// Not implemented for this use case
			// this is required for UI updates on the forms side but not here
		}

		/// <summary>
		/// Display debug info
		/// </summary>
		private void DisplaySimulatorDebugInfo()
		{
			// Not implemented for this use case
		}

		/// <summary>
		/// Clones the simulator object and saves it to simulatorHistory
		/// </summary>
		private void CaptureHistory()
		{
			// Capture values for next step
			var simulatorHistoryClone = (CySimulator)Mapper.Map(cySimulator, typeof(CySimulator), typeof(CySimulator));
			simulatorHistory.Add(simulatorHistoryClone);
		}

		/// <summary>
		/// Retrieves the private field value from the current simulator object
		/// </summary>
		/// <param name="memberName"></param>
		/// <returns></returns>
		public static object GetCyDfbAsmPrivateFieldCurr(string memberName)
		{
			if (cyDfbAsm == null) { return null; }
			else
			{
				return PrivateValueAccessor.GetPrivateFieldValue(typeof(CyDfbAsm), memberName, Wrapper.DfbAsm);
			}
		}

		#region IDisposable Support

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					for (int i = 0; i < simulatorHistory.Count; i++)
					{
						this.simulatorHistory[i] = null;
					}
					this.cySimulator = null;
					this.cyCodeTab = null;
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~Wrapper() {
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
