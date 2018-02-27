using DFB_v1_40;
using DFB_v1_40.Asm;
using DFBSimulatorWrapper;
using DFBSimulatorWrapper.DFBStateModel;
using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DFBUtility
{
	public partial class Form1 : Form
	{
		#region Members

		const int dfbMinInt = -8388608;
		const int dfbMaxInt = 8388607;
		const decimal dfbMinDec = -1.0000000m;
		const decimal dfbMaxDec = 0.9999999m;

		private float dpiX;
		private float dpiY;
		public DFBProject project;
		Wrapper sim;
		bool simRunning = false;
		CyParameters m_parameters;
		CancellationTokenSource tokenSource;
		CancellationToken cancelToken;
		public DFBState state;
		bool fctb_Code_IsDirty;
		bool fctb_BusIn1_IsDirty;
		bool fctb_BusIn2_IsDirty;
		bool dgvGlobalInputs_IsDirty;
		bool tbNbrCycles_IsDirty;
		int previousCycleNbr;
		CheckBox cbBusOut1ShowAll;
		CheckBox cbBusOut2ShowAll;

		string defaultPath;
		string[] args;
		
		#endregion
		#region Form

		public Form1(string[] args)
		{
			this.args = args;
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			Update();
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				defaultPath = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), 
					"DFB Utility");

				Directory.CreateDirectory(defaultPath);

				using (Graphics g = this.CreateGraphics())
				{
					dpiX = g.DpiX;
					dpiY = g.DpiY;
				}

				webBrowser1.Navigate("about:blank");
				webBrowser2.Navigate("about:blank");

				// layouts and default states
				tbCurrCycle.Text = "";
				lblCycleEnd.Text = "";
				splitMain.SplitterDistance = (int)(0.30 * splitMain.Width);
				splitRamACU.SplitterDistance = (int)(splitRamACU.Width / 2);
				splitRamData.SplitterDistance = (int)(splitRamData.Width / 2);
				//splitMain_RH.SplitterDistance = (int)(splitMain_RH.Width / 2);

				cbBusOut1Format.SelectedIndex = 0;
				cbBusOut2Format.SelectedIndex = 0;

				// Cy setup
				var inst = new CyInstEdit_v1();
				m_parameters = new CyParameters(inst);
				CyParameters.MsgPrinted += this.TryMsgPrint;
				CyParameters.ErrorAdded += this.TryAddError;
				m_parameters.m_edit = inst;

				tokenSource = new CancellationTokenSource();
				cancelToken = tokenSource.Token;

				fctb_Code.SelectionStyle = selectionStyle;
				fctb_Code.BookmarkColor = bookmarkColor;

				fctbRamAcuA.SelectionStyle = selectionStyle;
				fctbRamAcuA.BookmarkColor = bookmarkColor;
				fctbRamAcuB.SelectionStyle = selectionStyle;
				fctbRamAcuB.BookmarkColor = bookmarkColor;

				fctbRamDataA.SelectionStyle = selectionStyle;
				fctbRamDataA.BookmarkColor = bookmarkColor;
				fctbRamDataB.SelectionStyle = selectionStyle;
				fctbRamDataB.BookmarkColor = bookmarkColor;

				fctbBusOut1.SelectionStyle = selectionStyle;
				fctbBusOut1.BookmarkColor = bookmarkColor;
				fctbBusOut2.SelectionStyle = selectionStyle;
				fctbBusOut2.BookmarkColor = bookmarkColor;

				AddToolstripCheckbox_HoldA();
				AddToolstripCheckbox_HoldB();

				project = new DFBProject();
				saveFileDialog1.AddExtension = true;
				bsInputSequence.DataSource = project.InputSequence;

				fctb_Code_IsDirty = false;
				fctb_BusIn1_IsDirty = false;
				fctb_BusIn2_IsDirty = false;
				dgvGlobalInputs_IsDirty = false;
				tbNbrCycles_IsDirty = false;

				UpdateFormDirty();

				fctbValConv_Hex.TextChanged += fctbValConv_Hex_TextChanged;
				fctbValConv_DFB.TextChanged += fctbValConv_DFB_TextChanged;
				fctbValConv_Int.TextChanged += fctbValConv_Int_TextChanged;
				fctbRamAcuA.TextChanged += fctbRamAcu_TextChanged;
				fctbRamAcuB.TextChanged += fctbRamAcu_TextChanged;
				fctbRamDataA.TextChanged += fctbRamAcu_TextChanged;
				fctbRamDataB.TextChanged += fctbRamAcu_TextChanged;

				var sb = new StringBuilder();
				sb.AppendLine("PageUp = Step Back, PageDown = Step Forward");
				sb.AppendLine("Zoom using Ctrl +, Ctrl -, or Ctrl Scrollwheel");
				ToolTip toolTip1 = new ToolTip();
				toolTip1.AutoPopDelay = 5000;
				toolTip1.InitialDelay = 1000;
				toolTip1.ReshowDelay = 500;
				toolTip1.ShowAlways = true;
				toolTip1.SetToolTip(trackCycle, sb.ToString());

				// Prevent browser from stealing KeyDown events
				webBrowser1.WebBrowserShortcutsEnabled = false;
				webBrowser2.WebBrowserShortcutsEnabled = false;

				CheckGraphviz();

				openFileDialog1.InitialDirectory = defaultPath;

				if(args != null && args.Length > 0 && File.Exists(args[0]))
				{
					OpenProject(args[0]);
				}


			}
			catch (Exception ex) { LogException(ex); }
		}
		
		private async void RunSimulations()
		{
			try
			{
				if (fctb_Code.Text.Length == 0) { return; }

				tbCurrCycle.Text = "";
				lblCycleEnd.Text = "";

				bool nbrCyclesValid;
				nbrCyclesValid = int.TryParse(tbNbrCycles.Text, out int nbrCycles);
				if (!nbrCyclesValid || nbrCycles < 1)
				{
					TryMsgPrint("Number of cycles must be a positive integer.", Color.Red);
					return;
				}

				m_parameters.m_globalEditMode = true;
				m_parameters.Code = fctb_Code.Text;

				// Set bus data from files, if present
				m_parameters.Bus1_data = fctb_BusIn1.Text;
				m_parameters.Bus2_data = fctb_BusIn2.Text;

				// Not sure what these parameters do, seem not to apply to simulator
				//m_parameters.ShowDMA1 = true;
				//m_parameters.ShowDMA2 = false;

				// Connections
				m_parameters.ShowIn1 = false;       // Enable input terminal 1
				m_parameters.ShowIn2 = false;       // Enable input terminal 2
				m_parameters.ShowOut1 = true;       // Enable output terminal 1
				m_parameters.ShowOut2 = false;      // Enable output terminal 2

				// Output 1 source (radio button)
				bool rbO1DFB_RUN_Checked = false;   // Run Bit
				bool rbO1SEM0_Checked = false;      // Semaphore 0
				bool rbO1SEM1_Checked = true;       // Semaphore 1
				bool rbO1DFB_INTR_Checked = false;  // Interrupt

				// Output 2 source (radio button)
				bool rbO2SEM2_Checked = true;       // Semaphore 2
				bool rbO2DPSIGN_Checked = false;    // Datapath sign
				bool rbO2DPTHRESH_Checked = false;  // Datapath threshold crossed
				bool rbO2DPEQ_Checked = false;      // Datapath ALU equals 0

				// Interrupt Generation Sources
				bool intSem0CB_Checked = false;     // Semaphore 0
				bool intSem1CB_Checked = false;     // Semaphore 1
				bool intSem2CB_Checked = false;     // Semaphore 2
				bool intRegACB_Checked = false;     // Data in holding register A
				bool intRegBCB_Checked = false;     // Data in holding register B

				// DMA Request A Source
				bool dmar1NoneRB_Checked = false;   // None
				bool dmar1HoldRB_Checked = true;    // Data in Holding Register A
				bool dmar1Sem0RB_Checked = false;   // Semaphore 0
				bool dmar1Sem1RB_Checked = false;   // Semaphore 1

				// DMA Request B Source
				bool dmar2NoneRB_Checked = true;    // None
				bool dmar2HoldRB_Checked = false;   // Data in Holding Register B
				bool dmar2Sem0RB_Checked = false;   // Semaphore 0
				bool dmar2Sem1RB_Checked = false;   // Semaphore 1

				if (rbO1DFB_INTR_Checked) { m_parameters.InitialOutput1Source = CyOut1SourceOptions.DFB_INTR; }
				else if (rbO1DFB_RUN_Checked) { m_parameters.InitialOutput1Source = CyOut1SourceOptions.DFB_RUN; }
				else if (rbO1SEM0_Checked) { m_parameters.InitialOutput1Source = CyOut1SourceOptions.SEM0; }
				else if (rbO1SEM1_Checked) { m_parameters.InitialOutput1Source = CyOut1SourceOptions.SEM1; }
				m_parameters.InitialOutput1Source = CyOut1SourceOptions.SEM1;

				if (rbO2DPEQ_Checked) { m_parameters.InitialOutput2Source = CyOut2SourceOptions.DPEQ; }
				else if (rbO2DPSIGN_Checked) { m_parameters.InitialOutput2Source = CyOut2SourceOptions.DPSIGN; }
				else if (rbO2DPTHRESH_Checked) { m_parameters.InitialOutput2Source = CyOut2SourceOptions.DPTHRESH; }
				else if (rbO2SEM2_Checked) { m_parameters.InitialOutput2Source = CyOut2SourceOptions.SEM2; }
				m_parameters.InitialOutput2Source = CyOut2SourceOptions.SEM2;


				byte b = 0;
				if (intSem2CB_Checked) { b = (byte)(b + 16); }
				if (intSem1CB_Checked) { b = (byte)(b + 8); }
				if (intSem0CB_Checked) { b = (byte)(b + 4); }
				if (intRegBCB_Checked) { b = (byte)(b + 2); }
				if (intRegACB_Checked) { b = (byte)(b + 1); }
				m_parameters.InitialInterruptMode = b;

				b = 0;
				if (dmar1NoneRB_Checked) { /*b = b;*/ }
				else if (dmar1Sem0RB_Checked) { b = (byte)(b + 2); }
				else if (dmar1Sem1RB_Checked) { b = (byte)(b + 3); }
				else if (dmar1HoldRB_Checked) { b = (byte)(b + 1); }

				if (dmar2NoneRB_Checked) { /*b = b;*/ }
				else if (dmar2Sem1RB_Checked) { b = (byte)(b + 8); }
				else if (dmar2Sem0RB_Checked) { b = (byte)(b + 12); }
				else if (dmar2HoldRB_Checked) { b = (byte)(b + 4); }
				m_parameters.InitialDMAMode = b;

				m_parameters.CustomizerLayout = "";
				m_parameters.OptimizeAssembly = true;  // use compactor or not

				// Execute simulation for n steps
				statlblBottom.Text = string.Format("Running {0} simulation cycles...", nbrCycles);

				var wasCancelled = false;
				var progress = new Progress<SimProgressReport>();
				progress.ProgressChanged += (o, report) =>
				{
					progressBottom.Maximum = report.TotalSteps;
					progressBottom.Value = report.CurrentStep;
					statlblBottom.Text = string.Format("Completed {0} of {1} cycles.", report.CurrentStep, report.TotalSteps);
					wasCancelled = report.Cancelled;
				};

				// Cleanup prior
				if (state != null)
				{
					state.Dispose();
					state = null;
				}
				if (sim != null)
				{
					sim.Dispose();
					sim = null;
				}
				GC.Collect();

				// Execute
				sim = new DFBSimulatorWrapper.Wrapper();
				var cyCodeTab = new CyCodeTab(m_parameters);
				state = await sim.RunDFBSimulations(m_parameters, nbrCycles, progress, cancelToken, project.InputSequence, cyCodeTab, webBrowser1.DisplayRectangle, dpiX, dpiY);
				progressBottom.Value = 0;

				if (state.StateFrames.Count == 0)
				{
					statlblBottom.Text = "No frames captured - review the log.";
					SetSimState(false);
					return;
				}

				state.GenerateCallDiagram();
				if (wasCancelled)
				{
					statlblBottom.Text = string.Format("Simulation cancelled. {0} frames captured.", state.StateFrames.Count);
				}
				else
				{
					statlblBottom.Text = string.Format("Simulation complete. {0} frames captured.", state.StateFrames.Count);
				}

				trackCycle.TickStyle = TickStyle.Both;
				trackCycle.TickFrequency = 1;
				trackCycle.Minimum = 0;
				trackCycle.Maximum = state.StateFrames.Count - 1;
				trackCycle.Value = 0;
				UpdateForm();
				Populate_HoldA_Tab(state);
				Populate_HoldB_Tab(state);

				//SetSimState(false);
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
			finally
			{
				SetSimState(false);
			}
		}

		private void UpdateForm()
		{
			try
			{
				if (state == null || state.StateFrames.Count == 0) { return; }
				var stateFrame = state.StateFrames[trackCycle.Value];

				Update_Code_Tab(stateFrame);
				Update_ACU_DataRam_Tab(stateFrame);
				Update_HoldA_Tab(state);
				Update_HoldB_Tab(state);
				Update_Diagram(stateFrame);
				Update_JumpCond_Tab(stateFrame);
				Update_CallDiagram(state);
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void SetFormCaption()
		{
			var sb = new StringBuilder();
			sb.Append("DFB Utility ");

			var prjNameText = project == null || string.IsNullOrEmpty(project.ProjectFileName) ? "" : Path.GetFileNameWithoutExtension(project.ProjectFileName);
			if (!string.IsNullOrEmpty(prjNameText))
			{
				sb.AppendFormat(" - {0}", prjNameText);
			}

			if (IsFormDirty())
			{
				sb.Append(" [unsaved changed]");
			}
			this.Text = sb.ToString();
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.F5:
					btnStart_Click(sender, null);
					e.Handled = true;
					break;

				case Keys.PageDown:
					StepForward();
					e.Handled = true;
					break;

				case Keys.PageUp:
					StepBack();
					e.Handled = true;
					break;
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (IsFormDirty())
				{
					var resp = MessageBox.Show("Do you want to save the changes you have made?", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
					switch (resp)
					{
						case DialogResult.Yes:
							if (SaveProject())
							{
								break;
							}
							else
							{
								e.Cancel = true;
								return;
							}

						case DialogResult.No:
							break;

						case DialogResult.Cancel:
							e.Cancel = true;
							return;
					}
				}
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void CheckGraphviz()
		{
			try
			{
				var path = ConfigurationManager.AppSettings["graphVizLocation"];
				var filePath = Path.Combine(path, "dot.exe");
				if (!File.Exists(filePath))
				{
					var ex = new ConfigurationErrorsException(string.Format("GraphViz installation not found at location specified in App.config file>appSettings>graphVizLocation:\n \t[{0}]\nPlease update this setting to point to the /bin directory of GraphViz.", path));
					LogException(ex);
				}
			}
			catch (Exception ex) { LogException(ex); }
		}

		#endregion
		#region Top Toolbar

		private void btnProjectOpen_Click(object sender, EventArgs e)
		{
			try
			{
				if (IsFormDirty())
				{
					var resp = MessageBox.Show("Do you want to save the changes you have made?", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
					switch (resp)
					{
						case DialogResult.Yes:
							if (SaveProject())
							{
								break;
							}
							else
							{
								return;
							}

						case DialogResult.No:
							break;

						case DialogResult.Cancel:
							return;
					}
				}

				var tempDefaultExt = openFileDialog1.DefaultExt;
				var tempFilter = openFileDialog1.Filter;

				openFileDialog1.DefaultExt = ".dfbproj";
				openFileDialog1.Filter = "DFB Project | *.dfbproj";

				var result = openFileDialog1.ShowDialog();
				if (result == DialogResult.OK)
				{
					OpenProject(openFileDialog1.FileName);
				}

				openFileDialog1.DefaultExt = tempDefaultExt;
				openFileDialog1.Filter = tempFilter;
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void btnProjectNew_Click(object sender, EventArgs e)
		{
			try
			{
				if (IsFormDirty())
				{
					var resp = MessageBox.Show("Do you want to save the changes you have made?", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
					switch (resp)
					{
						case DialogResult.Yes:
							if (SaveProject())
							{
								break;
							}
							else
							{
								return;
							}

						case DialogResult.No:
							break;

						case DialogResult.Cancel:
							return;
					}
				}

				project = new DFBProject();
				bsInputSequence.DataSource = project.InputSequence;
				ClearControls();

				fctb_Code_IsDirty = false;
				fctb_BusIn1_IsDirty = false;
				fctb_BusIn2_IsDirty = false;
				dgvGlobalInputs_IsDirty = false;
				tbNbrCycles_IsDirty = false;

				UpdateFormDirty();

				btnStart.Enabled = false;
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void btnProjectSave_Click(object sender, EventArgs e)
		{
			try
			{
				SaveProject();
			}
			catch (Exception ex) { LogException(ex); }
		}

		public void OpenProject(string path)
		{
			project.Open(path);
			project.ProjectFileName = path;

			tbNbrCycles.Text = project.CyclesToRun.ToString();
			fctb_BusIn1.Text = project.Bus1Data;
			fctb_BusIn2.Text = project.Bus2Data;
			fctb_Code.Text = project.Code;

			bsInputSequence.DataSource = project.InputSequence;

			fctb_Code_IsDirty = false;
			fctb_BusIn1_IsDirty = false;
			fctb_BusIn2_IsDirty = false;
			dgvGlobalInputs_IsDirty = false;
			tbNbrCycles_IsDirty = false;

			tabInfo.SelectedTab = tpCode;

			UpdateFormDirty();

			if (fctb_Code.Text.Length > 0)
			{
				btnStart.Enabled = true;
			}
		}

		/// <summary>
		/// Saves the project, prompting for save as if new
		/// </summary>
		/// <returns>true is saved, false if user cancelled</returns>
		private bool SaveProject()
		{
			dgvGlobalInputs.EndEdit();

			// Save as
			if (string.IsNullOrEmpty(project.ProjectFileName) || !File.Exists(project.ProjectFileName))
			{
				var tempDefaultExt = saveFileDialog1.DefaultExt;
				var tempFilter = saveFileDialog1.Filter;

				saveFileDialog1.DefaultExt = ".dfbproj";
				saveFileDialog1.Filter = "DFB Project | *.dfbproj";

				var result = saveFileDialog1.ShowDialog();
				saveFileDialog1.DefaultExt = tempDefaultExt;
				saveFileDialog1.Filter = tempFilter;

				if (result == DialogResult.OK)
				{
					project.ProjectFileName = saveFileDialog1.FileName;
				}
				else
				{
					return false;
				}
			}

			// Save
			project.Bus1Data = fctb_BusIn1.Text;
			project.Bus2Data = fctb_BusIn2.Text;
			project.Code = fctb_Code.Text;
			project.CyclesToRun = int.Parse(tbNbrCycles.Text);
			project.Save();
			fctb_Code_IsDirty = false;
			fctb_BusIn1_IsDirty = false;
			fctb_BusIn2_IsDirty = false;
			dgvGlobalInputs_IsDirty = false;
			tbNbrCycles_IsDirty = false;

			UpdateFormDirty();

			return true;
		}

		private void tbNbrCycles_Leave(object sender, EventArgs e)
		{
			if (!int.TryParse(tbNbrCycles.Text, out int intValue))
			{
				MessageBox.Show("Nbr of Cycles to Run must be an integer value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				tbNbrCycles.Text = "";
				tbNbrCycles.Focus();
				return;
			}
			else
			{
				tbNbrCycles_IsDirty = true;
				UpdateFormDirty();
			}
		}

		private void tbNbrCycles_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
			{
				btnStart_Click(sender, null);
			}
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			try
			{
				if (!simRunning)
				{
					SetSimState(true);
					RunSimulations();
				}
				else
				{
					// try cancel
					tokenSource.Cancel();
				}
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void btnAbout_Click(object sender, EventArgs e)
		{
			var about = new AboutBox();
			about.ShowDialog(this);
		}

		#endregion
		#region Globals Tab

		private void dgvGlobalInputs_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			try
			{
				dgvGlobalInputs_IsDirty = true;
				UpdateFormDirty();
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void dgvGlobalInputs_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			try
			{
				dgvGlobalInputs_IsDirty = true;
				UpdateFormDirty();
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void dgvGlobalInputs_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			try
			{
				dgvGlobalInputs_IsDirty = true;
				UpdateFormDirty();
			}
			catch (Exception ex) { LogException(ex); }
		}

		#endregion
		#region Bus In 1 Stage_A Tab

		private void btnBusIn1Insert_Click(object sender, EventArgs e)
		{
			try
			{
				var result = openFileDialog1.ShowDialog();
				if (result == DialogResult.OK)
				{
					fctb_BusIn1.Text = File.ReadAllText(openFileDialog1.FileName);
					SetStatusLabel(string.Format("Inserted {0}", openFileDialog1.SafeFileName));
					fctb_BusIn1_IsDirty = true;
				}
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void fctb_BusIn1_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				// TextChanged event fires during init - bypass this
				if (e.ChangedRange.Start.iLine == 0 && e.ChangedRange.Start.iChar == 0
					&& e.ChangedRange.End.iLine == 0 && e.ChangedRange.End.iChar == 0)
				{ return; }

				fctb_BusIn1_IsDirty = true;
				UpdateFormDirty();
			}
			catch (Exception ex) { LogException(ex); }
		}

		#endregion
		#region Bus In 2 Stage_B Tab

		private void btnBusIn2Insert_Click(object sender, EventArgs e)
		{
			try
			{
				var result = openFileDialog1.ShowDialog();
				if (result == DialogResult.OK)
				{
					fctb_BusIn2.Text = File.ReadAllText(openFileDialog1.FileName);
					SetStatusLabel(string.Format("Inserted {0}", openFileDialog1.SafeFileName));
					fctb_BusIn2_IsDirty = true;
				}
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void fctb_BusIn2_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				// TextChanged event fires during init - bypass this
				if (e.ChangedRange.Start.iLine == 0 && e.ChangedRange.Start.iChar == 0
					&& e.ChangedRange.End.iLine == 0 && e.ChangedRange.End.iChar == 0)
				{ return; }

				fctb_BusIn2_IsDirty = true;
				UpdateFormDirty();
			}
			catch (Exception ex) { LogException(ex); }
		}

		#endregion
		#region Code Tab

		private void btnCodeInsert_Click(object sender, EventArgs e)
		{
			try
			{
				var result = openFileDialog1.ShowDialog();
				if (result == DialogResult.OK)
				{
					fctb_Code.Text = File.ReadAllText(openFileDialog1.FileName);
					btnStart.Enabled = true;
					SetStatusLabel(string.Format("Inserted {0}", openFileDialog1.SafeFileName));
					fctb_Code_IsDirty = true;
				}
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void fctb_Code_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				// TextChanged event fires during init - bypass this
				if (e.ChangedRange.Start.iLine == 0 && e.ChangedRange.Start.iChar == 0
					&& e.ChangedRange.End.iLine == 0 && e.ChangedRange.End.iChar == 0)
				{ return; }

				CodeTab_SyntaxHighlight(e);

				if (fctb_Code.Text.Length > 0)
				{
					btnStart.Enabled = true;
				}

				fctb_Code_IsDirty = true;
				UpdateFormDirty();
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void fctb_Code_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				fctb_Code.Bookmarks.Clear();
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void Update_Code_Tab(DFBStateFrame stateFrame)
		{
			try
			{
				SetActiveLine(fctb_Code, CodeStoreModel.ProgramLineNbr(trackCycle.Value));
			}
			catch (Exception ex) { LogException(ex); }
		}

		#endregion
		#region Value Conversion Tab

		private void fctbValConv_Hex_TextChanged(object sender, TextChangedEventArgs e)
		{
			fctbValConv_DFB.TextChanged -= fctbValConv_DFB_TextChanged;
			fctbValConv_Int.TextChanged -= fctbValConv_Int_TextChanged;

			var sbDfb = new StringBuilder();
			var sbInt = new StringBuilder();

			fctbValConv_DFB.Clear();
			fctbValConv_Int.Clear();

			for (int i = 0; i < fctbValConv_Hex.Lines.Count; i++)
			{
				var line = fctbValConv_Hex.Lines[i];
				var hexString = line.ToUpper().Replace("0X", "");

				Int32 hexValue = 0;
				bool isValidHex = false;
				try
				{
					if (hexString.Length != 6)
					{
						isValidHex = false;
					}
					else
					{
						hexValue = Int32.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
						isValidHex = true;
					}
				}
				catch { /* swallow */ }

				if (isValidHex && hexValue >= 0 && hexValue <= 16777215)
				{
					sbDfb.AppendLine(DFBModelBase.Hex_To_DFB_Q23(hexString).ToString("N7").PadLeft(10, ' '));
					sbInt.AppendLine(DFBModelBase.Hex6_To_SignedInt32(hexString).ToString("N0").PadLeft(11, ' '));
				}
				else
				{
					sbDfb.AppendLine();
					sbInt.AppendLine();
				}
			}

			fctbValConv_DFB.Text = sbDfb.ToString();
			fctbValConv_Int.Text = sbInt.ToString();

			fctbValConv_DFB.TextChanged += fctbValConv_DFB_TextChanged;
			fctbValConv_Int.TextChanged += fctbValConv_Int_TextChanged;
		}

		private void fctbValConv_DFB_TextChanged(object sender, TextChangedEventArgs e)
		{
			fctbValConv_Hex.TextChanged -= fctbValConv_Hex_TextChanged;
			fctbValConv_Int.TextChanged -= fctbValConv_Int_TextChanged;

			var sbHex = new StringBuilder();
			var sbInt = new StringBuilder();

			fctbValConv_Hex.Clear();
			fctbValConv_Int.Clear();

			for (int i = 0; i < fctbValConv_DFB.Lines.Count; i++)
			{
				var line = fctbValConv_DFB.Lines[i];
				var valString = line;
				bool valValid = false;

				valValid = Decimal.TryParse(valString, out decimal val);

				if (valValid && val >= dfbMinDec && val <= dfbMaxDec)
				{
					sbHex.AppendLine(DFBModelBase.DFB_Q23_To_Hex(val, 6));
					sbInt.AppendLine(DFBModelBase.DFB_Q23_To_SignedInt32(val).ToString("N0").PadLeft(11, ' '));
				}
				else
				{
					//sbHex.AppendLine(line);
					sbHex.AppendLine();
					sbInt.AppendLine();
				}
			}

			fctbValConv_Hex.Text = sbHex.ToString();
			fctbValConv_Int.Text = sbInt.ToString();

			fctbValConv_Hex.TextChanged += fctbValConv_Hex_TextChanged;
			fctbValConv_Int.TextChanged += fctbValConv_Int_TextChanged;
		}

		private void fctbValConv_Int_TextChanged(object sender, TextChangedEventArgs e)
		{
			fctbValConv_Hex.TextChanged -= fctbValConv_Hex_TextChanged;
			fctbValConv_DFB.TextChanged -= fctbValConv_DFB_TextChanged;

			var sbHex = new StringBuilder();
			var sbDfb = new StringBuilder();

			fctbValConv_Hex.Clear();
			fctbValConv_DFB.Clear();
			
			for (int i = 0; i < fctbValConv_Int.Lines.Count; i++)
			{
				var line = fctbValConv_Int.Lines[i];
				var valString = line;
				bool valValid = false;

				valValid = Int32.TryParse(valString.Replace(",", ""), out int val);

				if (valValid && val >= dfbMinInt && val <= dfbMaxInt)
				{
					sbHex.AppendLine(DFBModelBase.SignedInt32_To_Hex(val, 6));
					sbDfb.AppendLine(DFBModelBase.SignedInt32_To_DFB_Q23(val).ToString("N7").PadLeft(10, ' '));
				}
				else
				{
					sbHex.AppendLine();
					sbDfb.AppendLine();
				}
			}

			fctbValConv_Hex.Text = sbHex.ToString();
			fctbValConv_DFB.Text = sbDfb.ToString();

			fctbValConv_Hex.TextChanged += fctbValConv_Hex_TextChanged;
			fctbValConv_DFB.TextChanged += fctbValConv_DFB_TextChanged;
		}

		#endregion
		#region Log Tab

		/// <summary>
		/// Writes the same to the debug window as the simulator program's output
		/// </summary>
		private void DebugSimulatorOutput()
		{
			/*
			var sb = new StringBuilder();
			sb.Append("Cycle ");
			sb.Append("RamA ");
			sb.Append("RamB ");
			sb.Append("Ram ");
			sb.Append("CFSM  ");
			sb.Append("Aaddr ");
			sb.Append("Baddr ");
			sb.Append("   A2Mux ");
			sb.Append("   B2Mux ");
			sb.Append("  MacOut ");
			sb.Append("  AluOut ");
			sb.Append("ShiftOut ");
			sb.AppendLine();
			sb.Append("      ");
			sb.Append("     ");
			sb.Append("     ");
			sb.Append("sel ");
			sb.Append("state ");
			sb.Append("next  ");
			sb.Append("next ");
			sb.AppendLine();

			for (int i = 0; i < sim.SimulatorCyclesCaptured; i++)
			{
				// These values are printed BEFORE the step executes:
				sb.AppendFormat("  {0,3:d} ", i);
				sb.AppendFormat(" {0,3:d} ", CodeStoreModel.InstructionAIndex(i, -1));
				sb.AppendFormat(" {0,3:d} ", CodeStoreModel.InstructionBIndex(i, -1));
				sb.AppendFormat("  {0} ", CodeStoreModel.InstructionBankActive(i, -1));
				
				// The rest of the values are shown AFTER the instruction executes
				sb.AppendFormat("  {0,3:d} ", CodeStoreModel.InstructionState(i, 0).StateNumber);
				sb.AppendFormat("  {0,3:d} ", state.StateFrames[i].ACU_A.Output.Value);
				sb.AppendFormat("  {0,3:d} ", state.StateFrames[i].ACU_B.Output.Value.Value);
				sb.AppendFormat("{0,8:X} ", state.StateFrames[i].Mux_2A_back0.Output.Value);
				sb.AppendFormat("{0,8:X} ", state.StateFrames[i].Mux_2B_back0.Output.Value);
				sb.AppendFormat("{0,8:X} ", state.StateFrames[i].MAC_back0.Output.Value);
				sb.AppendFormat("{0,8:X} ", 0); // ToDo:
				sb.AppendFormat("{0,8:X} ", 0); // ToDo:
				sb.AppendLine();
			}

			Debug.Print(sb.ToString());
			*/
		}

		private void TryAddError(CySAErrorMessage err)
		{
			try
			{
				CyParameters.ErrorAddedDelegate method = delegate (CySAErrorMessage err2)
				{
					this.TryAddError(err2);
				};

				if (base.InvokeRequired)
				{
					base.Invoke(method, err);
				}
				else
				{
					this.AddErrors(new List<CySAErrorMessage>
				{
					err
				}, true);
				}
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void TryMsgPrint(string sender, Color clr)
		{
			try
			{
				CyParameters.MsgPrintDelegate msgPrintDelegate = delegate (string snd, Color color)
				{
					this.rtbMessages.ReadOnly = false;
					int num = 1000;
					if (this.rtbMessages.Lines.Length > num)
					{
						this.rtbMessages.Select(0, this.rtbMessages.GetFirstCharIndexFromLine(this.rtbMessages.Lines.Length - num));
						this.rtbMessages.SelectedText = "";
					}
					this.rtbMessages.SelectionIndent = 10;
					this.rtbMessages.SelectionStart = this.rtbMessages.TextLength;
					this.rtbMessages.SelectionLength = 0;
					Font selectionFont = this.rtbMessages.SelectionFont;
					this.rtbMessages.SelectionColor = color;
					this.rtbMessages.AppendText(sender);
					this.rtbMessages.SelectionColor = this.rtbMessages.ForeColor;
					this.rtbMessages.SelectionFont = selectionFont;
					if (sender.Contains("\n"))
					{
						this.rtbMessages.SelectionStart = this.rtbMessages.Text.Length;
						this.rtbMessages.ScrollToCaret();
					}
					this.rtbMessages.ReadOnly = true;
				};
				if (base.InvokeRequired)
				{
					base.Invoke(msgPrintDelegate, sender, clr);
				}
				else
				{
					msgPrintDelegate(sender, clr);
				}
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void AddErrors(List<CySAErrorMessage> errors, bool print)
		{
			try
			{
				for (int i = 0; i < errors.Count; i++)
				{
					this.TryMsgPrint(errors[i].GetFullDescr() + Environment.NewLine, CyParameters.GetColor(errors[i].Type));
				}
			}
			catch (Exception ex) { LogException(ex); }
		}

		#endregion
		#region ACU and Data Ram Tab

		TextStyle StyleRamPrev = new TextStyle(Brushes.Red, null, FontStyle.Regular);

		private void Update_ACU_DataRam_Tab(DFBStateFrame stateFrame)
		{
			try
			{
				fctbRamAcuA.Text = stateFrame.ACU_A.RamString;
				fctbRamAcuB.Text = stateFrame.ACU_B.RamString;
				fctbRamDataA.Text = stateFrame.DataRam_A.RamString;
				fctbRamDataB.Text = stateFrame.DataRam_B.RamString;

				if (stateFrame.ACU_A.Address.Value.HasValue)
				{
					var addr = stateFrame.ACU_A.Address.Value.Value;
					SetActiveLine(fctbRamAcuA, addr + 1);
				}

				if (stateFrame.ACU_B.Address.Value.HasValue)
				{
					var addr = stateFrame.ACU_B.Address.Value.Value;
					SetActiveLine(fctbRamAcuB, addr + 1);
				}

				if (stateFrame.DataRam_A.Address.Value.HasValue)
				{
					var addr = stateFrame.DataRam_A.Address.Value.Value;
					SetActiveLine(fctbRamDataA, addr + 1);
				}

				if (stateFrame.DataRam_B.Address.Value.HasValue)
				{
					var addr = stateFrame.DataRam_B.Address.Value.Value;
					SetActiveLine(fctbRamDataB, addr + 1);
				}

				// Indicate previous addresses
				if (stateFrame.ACU_A.AddressPrev.Value.HasValue)
				{
					var prevAddr = stateFrame.ACU_A.AddressPrev.Value.Value;
					SetLineStyleExclusive(fctbRamAcuA, StyleRamPrev, prevAddr + 1);
				}
				if (stateFrame.ACU_B.AddressPrev.Value.HasValue)
				{
					var prevAddr = stateFrame.ACU_B.AddressPrev.Value.Value;
					SetLineStyleExclusive(fctbRamAcuB, StyleRamPrev, prevAddr + 1);
				}

				if (stateFrame.DataRam_A.AddressPrev.Value.HasValue)
				{
					var prevAddr = stateFrame.DataRam_A.AddressPrev.Value.Value;
					SetLineStyleExclusive(fctbRamDataA, StyleRamPrev, prevAddr + 1);
				}
				if (stateFrame.DataRam_B.AddressPrev.Value.HasValue)
				{
					var prevAddr = stateFrame.DataRam_B.AddressPrev.Value.Value;
					SetLineStyleExclusive(fctbRamDataB, StyleRamPrev, prevAddr + 1);
				}
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void fctbRamAcu_TextChanged(object sender, TextChangedEventArgs e)
		{
			CodeTab_SyntaxHighlight(e);
		}

		private void dataRamAPanelHide_Click(object sender, EventArgs e)
		{
			if (dataRamAPanelHide.Text == "Show Both")
			{
				// panel is hidden currently - show both
				splitRamData.Panel1Collapsed = false;
				dataRamAPanelHide.Image = Properties.Resources.Collapse_16x_24;
				splitRamData.Panel2Collapsed = false;
				dataRamBPanelHide.Image = Properties.Resources.Collapse_16x_24;
				dataRamAPanelHide.Text = "Hide";
			}
			else
			{
				splitRamData.Panel1Collapsed = true;
				dataRamBPanelHide.Image = Properties.Resources.Expand_16x_24;
				dataRamBPanelHide.Text = "Show Both";
			}
		}

		private void dataRamBPanelHide_Click(object sender, EventArgs e)
		{
			if (dataRamBPanelHide.Text == "Show Both")
			{
				// panel is hidden currently - show both
				splitRamData.Panel1Collapsed = false;
				dataRamAPanelHide.Image = Properties.Resources.Collapse_16x_24;
				splitRamData.Panel2Collapsed = false;
				dataRamBPanelHide.Image = Properties.Resources.Collapse_16x_24;
				dataRamBPanelHide.Text = "Hide";
			}
			else
			{
				splitRamData.Panel2Collapsed = true;
				dataRamAPanelHide.Image = Properties.Resources.Expand_16x_24;
				dataRamAPanelHide.Text = "Show Both";
			}
		}

		private void acuRamAPanelHide_Click(object sender, EventArgs e)
		{
			if (acuRamAPanelHide.Text == "Show Both")
			{
				// panel is hidden currently - show both
				splitRamACU.Panel1Collapsed = false;
				acuRamAPanelHide.Image = Properties.Resources.Collapse_16x_24;
				splitRamACU.Panel2Collapsed = false;
				acuRamBPanelHide.Image = Properties.Resources.Collapse_16x_24;
				acuRamAPanelHide.Text = "Hide";
			}
			else
			{
				splitRamACU.Panel1Collapsed = true;
				acuRamBPanelHide.Image = Properties.Resources.Expand_16x_24;
				acuRamBPanelHide.Text = "Show Both";
			}
		}

		private void acuRamBPanelHide_Click(object sender, EventArgs e)
		{
			if (acuRamBPanelHide.Text == "Show Both")
			{
				// panel is hidden currently - show both
				splitRamACU.Panel1Collapsed = false;
				acuRamAPanelHide.Image = Properties.Resources.Collapse_16x_24;
				splitRamACU.Panel2Collapsed = false;
				acuRamBPanelHide.Image = Properties.Resources.Collapse_16x_24;
				acuRamBPanelHide.Text = "Hide";
			}
			else
			{
				splitRamACU.Panel2Collapsed = true;
				acuRamAPanelHide.Image = Properties.Resources.Expand_16x_24;
				acuRamAPanelHide.Text = "Show Both";
			}
		}

		#endregion
		#region Jump Conditions Tab

		private void Update_JumpCond_Tab(DFBStateFrame stateFrame)
		{
			fctb_JumpCond.Text = stateFrame.JumpConditions.JumpConditionsReport;
		}

		#endregion
		#region Call Diagram Tab

		private void Update_CallDiagram(DFBState state)
		{
			try
			{
				if (webBrowser2.Document == null) { webBrowser2.Navigate("about:blank"); }
				if (webBrowser2.Document.Body == null || string.IsNullOrEmpty(webBrowser2.Document.Body.InnerHtml))
				{
					string svgHtml = null;
					switch (webBrowser2.Version.Major)
					{
						case 11:
							svgHtml = string.Format("<head><meta http-equiv=\"X-UA-Compatible\" content=\"IE=11\"/><style>svg {{overflow: hidden;}}</style></head><HTML><BODY>{0}</BODY></HTML>", state.CallDiagramSvg);
							break;

						case 10:
							svgHtml = string.Format("<head><meta http-equiv=\"X-UA-Compatible\" content=\"IE=10\"/></head><HTML><BODY>{0}</BODY></HTML>", state.CallDiagramSvg);
							break;

						case 9:
							svgHtml = string.Format("<head><meta http-equiv=\"X-UA-Compatible\" content=\"IE=9\"/></head><HTML><BODY>{0}</BODY></HTML>", state.CallDiagramSvg);
							break;

						default:
							svgHtml = string.Format("<HTML><BODY>{0}</BODY></HTML>", "You must have Internet Explorer version 9 or above installed to display the state diagrams.");
							break;
					}
					webBrowser2.DocumentText = svgHtml;
				}
				else
				{
					// Replacing the innerHtml should serve to reduce flicker, so the browser component doesn't reload the entire page when the SVG is swapped
					webBrowser2.Document.Body.InnerHtml = state.CallDiagramSvg;
				}
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		#endregion
		#region Bus Out 1 Hold_A Tab

		private void cbBusOut1Format_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.state != null)
			{
				Populate_HoldA_Tab(this.state);
			}
		}

		private void Populate_HoldA_Tab(DFBState state)
		{

			// Populate Bus Out after simulation runs
			var format = DFBValueFormat.Hex;
			switch (cbBusOut1Format.SelectedItem.ToString().ToUpperInvariant())
			{
				case "HEX":
					format = DFBValueFormat.Hex;
					break;
				case "INT":
					format = DFBValueFormat.Int;
					break;
				case "Q.23":
					format = DFBValueFormat.q23Decimal;
					break;
			}
			fctbBusOut1.Text = state.Bus1Out(format, cbBusOut1ShowAll.Checked);

			Update_HoldA_Tab(state);
		}

		private void Update_HoldA_Tab(DFBState state)
		{
			// Highlight current Stage output row for cycle
			if (trackCycle.Value > -1 && fctbBusOut1.Lines.Count > 0)
			{
				var cycle = trackCycle.Value;
				if (cbBusOut1ShowAll.Checked)
				{
					SetActiveLine(fctbBusOut1, cycle + 1);
				}
				else
				{
					var map = state.Bus1CycleToOutputline(cbBusOut1ShowAll.Checked);
					if (map.Keys.Contains(cycle))
					{
						var lineNbr = map[cycle];
						SetActiveLine(fctbBusOut1, lineNbr + 1);
					}
					else
					{
						// Clear selection
						ClearActiveLine(fctbBusOut1);
					}
				}
			}
		}

		private void AddToolstripCheckbox_HoldA()
		{
			cbBusOut1ShowAll = new CheckBox
			{
				Name = "cbBusOut1ShowAll",
				Text = "Show all cycles"
			};
			cbBusOut1ShowAll.CheckStateChanged += cbBusOut1ShowAll_CheckStateChanged;
			cbBusOut1ShowAll.BackColor = Color.Transparent;
			var host = new ToolStripControlHost(cbBusOut1ShowAll);
			tsBus1Out.Items.Add(host);
		}

		private void cbBusOut1ShowAll_CheckStateChanged(object sender, EventArgs e)
		{
			Populate_HoldA_Tab(this.state);
		}

		#endregion
		#region Bus Out 2 Hold_B Tab

		private void cbBusOut2Format_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.state != null)
			{
				Populate_HoldB_Tab(this.state);
			}
		}

		private void Populate_HoldB_Tab(DFBState state)
		{
			// Populate Bus Out after simulation runs
			var format = DFBValueFormat.Hex;
			switch (cbBusOut2Format.SelectedItem.ToString().ToUpperInvariant())
			{
				case "HEX":
					format = DFBValueFormat.Hex;
					break;
				case "INT":
					format = DFBValueFormat.Int;
					break;
				case "Q.23":
					format = DFBValueFormat.q23Decimal;
					break;
			}

			fctbBusOut2.Text = state.Bus2Out(format, cbBusOut2ShowAll.Checked);

			Update_HoldB_Tab(state);
		}

		private void Update_HoldB_Tab(DFBState state)
		{
			// Highlight current Stage output row for cycle
			if (trackCycle.Value > -1 && fctbBusOut2.Lines.Count > 0)
			{
				var cycle = trackCycle.Value;
				if (cbBusOut2ShowAll.Checked)
				{
					SetActiveLine(fctbBusOut2, cycle + 1);
				}
				else
				{
					var map = state.Bus2CycleToOutputline(cbBusOut2ShowAll.Checked);
					if (map.Keys.Contains(cycle))
					{
						var lineNbr = map[cycle];
						SetActiveLine(fctbBusOut2, lineNbr + 1);
					}
					else
					{
						// Clear selection
						ClearActiveLine(fctbBusOut2);
					}
				}
			}
		}

		private void AddToolstripCheckbox_HoldB()
		{
			cbBusOut2ShowAll = new CheckBox
			{
				Name = "cbBusOut2ShowAll",
				Text = "Show all cycles"
			};
			cbBusOut2ShowAll.CheckStateChanged += cbBusOut2ShowAll_CheckStateChanged;
			cbBusOut2ShowAll.BackColor = Color.Transparent;
			var host = new ToolStripControlHost(cbBusOut2ShowAll);
			tsBus2Out.Items.Add(host);
		}

		private void cbBusOut2ShowAll_CheckStateChanged(object sender, EventArgs e)
		{
			Populate_HoldB_Tab(this.state);
		}

		#endregion
		#region Diagram Area

		private void trackCycle_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateForm();
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void Update_Diagram(DFBStateFrame stateFrame)
		{
			try
			{
				if (webBrowser1.Document == null)
				{
					webBrowser1.Navigate("about:blank");
				}
				if (webBrowser1.Document.Body == null || string.IsNullOrEmpty(webBrowser1.Document.Body.InnerHtml))
				{
					string svgHtml = null;
					switch (webBrowser1.Version.Major)
					{
						case 11:
							svgHtml = string.Format("<head><meta http-equiv=\"X-UA-Compatible\" content=\"IE=11\"/><style>svg {{overflow: hidden;}}</style></head><HTML><BODY>{0}</BODY></HTML>", stateFrame.DiagramSvg);
							break;

						case 10:
							svgHtml = string.Format("<head><meta http-equiv=\"X-UA-Compatible\" content=\"IE=10\"/></head><HTML><BODY>{0}</BODY></HTML>", stateFrame.DiagramSvg);
							break;

						case 9:
							svgHtml = string.Format("<head><meta http-equiv=\"X-UA-Compatible\" content=\"IE=9\"/></head><HTML><BODY>{0}</BODY></HTML>", stateFrame.DiagramSvg);
							break;

						default:
							svgHtml = string.Format("<HTML><BODY>{0}</BODY></HTML>", "You must have Internet Explorer version 9 or above installed to display the state diagrams.");
							break;
					}
					webBrowser1.DocumentText = svgHtml;
				}
				else
				{
					// Replacing the innerHtml should serve to reduce flicker, so the browser component doesn't reload the entire page whent he SVG is swapped
					webBrowser1.Document.Body.InnerHtml = stateFrame.DiagramSvg;
				}

				tbCurrCycle.Text = trackCycle.Value.ToString();
				lblCycleEnd.Text = (state.StateFrames.Count - 1).ToString();
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		private void StepForward()
		{
			if (trackCycle.Value < trackCycle.Maximum)
			{
				trackCycle.Value += 1;
			}
		}

		private void StepBack()
		{
			if (trackCycle.Value > trackCycle.Minimum)
			{
				trackCycle.Value -= 1;
			}
		}

		private void tbCurrCycle_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
			{
				tbCurrCycle_Leave(sender, null);
			}
		}

		private void tbCurrCycle_Leave(object sender, EventArgs e)
		{
			if (int.TryParse(tbCurrCycle.Text, out int tryInt))
			{
				if (tryInt < trackCycle.Minimum)
				{
					tryInt = trackCycle.Minimum;
					tbCurrCycle.Text = tryInt.ToString();
				}
				else if (tryInt > trackCycle.Maximum)
				{
					tryInt = trackCycle.Maximum;
					tbCurrCycle.Text = tryInt.ToString();
				}
				trackCycle.Value = tryInt;
			}
		}

		private void webBrowser1_SizeChanged(object sender, EventArgs e)
		{
			if (state == null || state.StateFrames.Count == 0) { return; }
			var stateFrame = state.StateFrames[trackCycle.Value];
			stateFrame.Resize(webBrowser1.DisplayRectangle, dpiX, dpiY);
			Update_Diagram(stateFrame);

			Task.Run(() => state.Resize(webBrowser1.DisplayRectangle, dpiX, dpiY));
		}

		#endregion
		#region Bottom Status Bar

		private void SetStatusLabel(string format, params object[] args)
		{
			try
			{
				statlblBottom.Text = string.Format(format, args);
			}
			catch (Exception ex) { LogException(ex); }
		}

		#endregion
		#region Misc

		private void SetSimState(bool simIsRunning)
		{
			try
			{
				simRunning = simIsRunning;

				if (simRunning)
				{
					// We are starting
					previousCycleNbr = trackCycle.Value;
					btnStart.Image = Properties.Resources.StatusStop_color_32xLG;
					btnStart.Text = "Cancel (F5)";
				}
				else
				{
					// We are stopping
					btnStart.Image = Properties.Resources.StatusRun_32xLG;
					btnStart.Text = "Start (F5)";
					if (previousCycleNbr < trackCycle.Maximum)
					{
						trackCycle.Value = previousCycleNbr;
					}
				}
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void UpdateFormDirty()
		{
			try
			{
				if (IsFormDirty())
				{
					btnProjectSave.Enabled = true;
					SetFormCaption();
				}
				else
				{
					btnProjectSave.Enabled = false;
					SetFormCaption();
				}
			}
			catch (Exception ex) { LogException(ex); }
		}

		private bool IsFormDirty()
		{
			return (fctb_Code_IsDirty || fctb_BusIn1_IsDirty || fctb_BusIn2_IsDirty || dgvGlobalInputs_IsDirty || tbNbrCycles_IsDirty);
		}

		private void ClearControls()
		{
			try
			{
				fctb_BusIn1.Text = "";
				fctb_BusIn2.Text = "";
				fctb_Code.Text = "";
				rtbMessages.Clear();
				//pictureDiagram.Image = null;
				//webBrowser1.DocumentText = "";
				webBrowser1.Navigate("about:blank");
				fctbRamAcuA.Clear();
				fctbRamAcuB.Clear();
				fctbRamDataA.Clear();
				fctbRamDataB.Clear();
				tbCurrCycle.Text = "";
				lblCycleEnd.Text = "";
			}
			catch (Exception ex) { LogException(ex); }
		}

		#endregion
		#region Code Syntax Shared

		// Syntax highlighter styles
		SelectionStyle selectionStyle = new SelectionStyle(new SolidBrush(Color.FromArgb(90, Color.Yellow)));
		Color bookmarkColor = Color.FromArgb(220, Color.Green);
		TextStyle BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
		TextStyle BoldStyle = new TextStyle(null, null, FontStyle.Bold | FontStyle.Underline);
		TextStyle GrayStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
		TextStyle MagentaStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);
		TextStyle GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
		TextStyle BrownStyle = new TextStyle(Brushes.Brown, null, FontStyle.Italic);
		TextStyle MaroonStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Regular);
		TextStyle LabelStyle = new TextStyle(null, null, FontStyle.Bold);
		TextStyle CommentEmphasis = new TextStyle(Brushes.Green, null, FontStyle.Italic | FontStyle.Bold);

		private void CodeTab_SyntaxHighlight(TextChangedEventArgs e)
		{
			try
			{
				fctb_Code.LeftBracket = '(';
				fctb_Code.RightBracket = ')';
				fctb_Code.LeftBracket2 = '\x0';
				fctb_Code.RightBracket2 = '\x0';

				//clear style of changed range
				e.ChangedRange.ClearStyle(BlueStyle, BoldStyle, GrayStyle, MagentaStyle, GreenStyle, BrownStyle, LabelStyle, CommentEmphasis);

				//string highlighting
				//e.ChangedRange.SetStyle(BrownStyle, @"""""|@""""|''|@"".*?""|(?<!@)(?<range>"".*?[^\\]"")|'.*?[^\\]'");

				// emphasis in comment (needs to be set before comment style)
				//e.ChangedRange.SetStyle(CommentEmphasis, @"@\S*", RegexOptions.Multiline); // any words starting with '@' symbol
				e.ChangedRange.SetStyle(CommentEmphasis, @"\[([^]]+)\]", RegexOptions.Multiline); // any words inside square brackets []

				// words in brackets
				//e.ChangedRange.SetStyle(CommentEmphasis, @"//.*\b(area|org|dw|acu|addr|dmux|alu|mac|shift|write)", RegexOptions.Multiline);

				//comment highlighting
				e.ChangedRange.SetStyle(GreenStyle, @"//.*$", RegexOptions.Multiline);
				e.ChangedRange.SetStyle(GreenStyle, @"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline);
				e.ChangedRange.SetStyle(GreenStyle, @"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline | RegexOptions.RightToLeft);

				//number highlighting
				e.ChangedRange.SetStyle(BrownStyle, @"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b");

				//attribute highlighting
				//e.ChangedRange.SetStyle(GrayStyle, @"^\s*(?<range>\[.+?\])\s*$", RegexOptions.Multiline);

				//keyword highlighting
				e.ChangedRange.SetStyle(BlueStyle, @"\b(area|org|dw|acu|addr|dmux|alu|mac|shift|write)");

				// label
				e.ChangedRange.SetStyle(LabelStyle, @"^\w.*:", RegexOptions.Multiline);
				//e.ChangedRange.SetStyle(LabelStyle, @"\w.*:)");



				//clear folding markers
				e.ChangedRange.ClearFoldingMarkers();

				//set folding markers
				//e.ChangedRange.SetFoldingMarkers("{", "}");//allow to collapse brackets block
				//e.ChangedRange.SetFoldingMarkers(@"#region\b", @"#endregion\b");//allow to collapse #region blocks
				//e.ChangedRange.SetFoldingMarkers(@"/\*", @"\*/");//allow to collapse comment block
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void SetActiveLine(FastColoredTextBox tb, int lineNumber)
		{
			try
			{
				tb.Bookmarks.Clear();
				var book = new Bookmark(tb, "currLine", lineNumber - 1);
				tb.Bookmarks.Add(book);
				book.DoVisible();
				tb.Selection = tb.GetLine(lineNumber - 1);
			}
			catch (Exception ex) { LogException(ex); }
		}

		private void ClearActiveLine(FastColoredTextBox tb)
		{
			tb.Bookmarks.Clear();
			var r = new Range(tb);
			tb.Selection = r;
		}

		/// <summary>
		/// Clears all other instances of this style and sets the style for the specified line
		/// </summary>
		/// <param name="tb"></param>
		/// <param name="style"></param>
		/// <param name="lineNumber"></param>
		private void SetLineStyleExclusive(FastColoredTextBox tb, Style style, int lineNumber)
		{
			try
			{
				tb.Range.ClearStyle(style);
				var line = tb.GetLine(lineNumber - 1);
				line.SetStyle(style);
			}
			catch (Exception ex) { LogException(ex); }
		}

		#endregion
		#region Exception Handling

		private void LogException(Exception ex)
		{
			TryMsgPrint("\n-------------------", Color.Red);
			TryMsgPrint("\nEXCEPTION:", Color.Red);
			TryMsgPrint("\n-------------------", Color.Red);
			TryMsgPrint("\n" + ex.Message, Color.Red);
			TryMsgPrint("\n" + ex.StackTrace, Color.Black);
			if(ex.InnerException != null)
			{
				LogException(ex.InnerException);
			}
		}

		#endregion
	}

	public static class Extensions
	{
		public static string GetExceptionMessages(this Exception e, string msgs = "")
		{
			if (e == null) return string.Empty;
			if (msgs == "") msgs = e.Message;
			if (e.InnerException != null)
				msgs += "\r\nInnerException: " + GetExceptionMessages(e.InnerException);
			return msgs;
		}
	}
}
