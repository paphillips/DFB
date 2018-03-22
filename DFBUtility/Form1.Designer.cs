namespace DFBUtility
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.progressBottom = new System.Windows.Forms.ToolStripProgressBar();
			this.statlblBottom = new System.Windows.Forms.ToolStripStatusLabel();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.splitCodeTab = new System.Windows.Forms.SplitContainer();
			this.tabInfo = new System.Windows.Forms.TabControl();
			this.tpGlobals = new System.Windows.Forms.TabPage();
			this.panel3 = new System.Windows.Forms.Panel();
			this.dgvGlobalInputs = new System.Windows.Forms.DataGridView();
			this.cycleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.globalInput1DataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.globalInput2DataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.semaphore0DataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.semaphore1DataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.semaphore2DataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.bsInputSequence = new System.Windows.Forms.BindingSource(this.components);
			this.toolStrip11 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel12 = new System.Windows.Forms.ToolStripLabel();
			this.tpBusIn1 = new System.Windows.Forms.TabPage();
			this.fctb_BusIn1 = new FastColoredTextBoxNS.FastColoredTextBox();
			this.tollBusIn1 = new System.Windows.Forms.ToolStrip();
			this.btnBusIn1Insert = new System.Windows.Forms.ToolStripButton();
			this.tpBusIn2 = new System.Windows.Forms.TabPage();
			this.fctb_BusIn2 = new FastColoredTextBoxNS.FastColoredTextBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnBusIn2Insert = new System.Windows.Forms.ToolStripButton();
			this.tpCode = new System.Windows.Forms.TabPage();
			this.fctb_Code = new FastColoredTextBoxNS.FastColoredTextBox();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.btnCodeInsert = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.lblCurrentCodeFile = new System.Windows.Forms.ToolStripLabel();
			this.toolStripLabel11 = new System.Windows.Forms.ToolStripLabel();
			this.tpValueConvert = new System.Windows.Forms.TabPage();
			this.tlp_ValueConversion = new System.Windows.Forms.TableLayoutPanel();
			this.fctbValConv_Hex = new FastColoredTextBoxNS.FastColoredTextBox();
			this.fctbValConv_DFB = new FastColoredTextBoxNS.FastColoredTextBox();
			this.fctbValConv_Int = new FastColoredTextBoxNS.FastColoredTextBox();
			this.toolStrip10 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel10 = new System.Windows.Forms.ToolStripLabel();
			this.toolStrip9 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel9 = new System.Windows.Forms.ToolStripLabel();
			this.toolStrip8 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
			this.toolStrip7 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.tabLogAndRam = new System.Windows.Forms.TabControl();
			this.tpLog = new System.Windows.Forms.TabPage();
			this.rtbMessages = new System.Windows.Forms.RichTextBox();
			this.tpRam = new System.Windows.Forms.TabPage();
			this.splitRams = new System.Windows.Forms.SplitContainer();
			this.splitRamACU = new System.Windows.Forms.SplitContainer();
			this.panel1 = new System.Windows.Forms.Panel();
			this.fctbRamAcuA = new FastColoredTextBoxNS.FastColoredTextBox();
			this.toolStrip3 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.acuRamAPanelHide = new System.Windows.Forms.ToolStripButton();
			this.fctbRamAcuB = new FastColoredTextBoxNS.FastColoredTextBox();
			this.toolStrip4 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
			this.acuRamBPanelHide = new System.Windows.Forms.ToolStripButton();
			this.panel2 = new System.Windows.Forms.Panel();
			this.splitRamData = new System.Windows.Forms.SplitContainer();
			this.fctbRamDataA = new FastColoredTextBoxNS.FastColoredTextBox();
			this.toolStrip5 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
			this.dataRamAPanelHide = new System.Windows.Forms.ToolStripButton();
			this.fctbRamDataB = new FastColoredTextBoxNS.FastColoredTextBox();
			this.toolStrip6 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
			this.dataRamBPanelHide = new System.Windows.Forms.ToolStripButton();
			this.tpJumpCond = new System.Windows.Forms.TabPage();
			this.fctb_JumpCond = new FastColoredTextBoxNS.FastColoredTextBox();
			this.tpCallDiag = new System.Windows.Forms.TabPage();
			this.webBrowser2 = new System.Windows.Forms.WebBrowser();
			this.tpBusOut1 = new System.Windows.Forms.TabPage();
			this.panel4 = new System.Windows.Forms.Panel();
			this.fctbBusOut1 = new FastColoredTextBoxNS.FastColoredTextBox();
			this.tsBus1Out = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel13 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.cbBusOut1Format = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.tpBusOut2 = new System.Windows.Forms.TabPage();
			this.panel5 = new System.Windows.Forms.Panel();
			this.fctbBusOut2 = new FastColoredTextBoxNS.FastColoredTextBox();
			this.tsBus2Out = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel14 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.cbBusOut2Format = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.webBrowser1 = new System.Windows.Forms.WebBrowser();
			this.pnlScrub = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tbCurrCycle = new System.Windows.Forms.TextBox();
			this.lblCycleEnd = new System.Windows.Forms.Label();
			this.trackCycle = new System.Windows.Forms.TrackBar();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.btnProjectOpen = new System.Windows.Forms.ToolStripButton();
			this.btnProjectNew = new System.Windows.Forms.ToolStripButton();
			this.btnProjectSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.tbNbrCycles = new System.Windows.Forms.ToolStripTextBox();
			this.btnStart = new System.Windows.Forms.ToolStripButton();
			this.btnAbout = new System.Windows.Forms.ToolStripButton();
			this.toolMain = new System.Windows.Forms.ToolStrip();
			this.tsddPlugins = new System.Windows.Forms.ToolStripDropDownButton();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitCodeTab)).BeginInit();
			this.splitCodeTab.Panel1.SuspendLayout();
			this.splitCodeTab.Panel2.SuspendLayout();
			this.splitCodeTab.SuspendLayout();
			this.tabInfo.SuspendLayout();
			this.tpGlobals.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvGlobalInputs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bsInputSequence)).BeginInit();
			this.toolStrip11.SuspendLayout();
			this.tpBusIn1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctb_BusIn1)).BeginInit();
			this.tollBusIn1.SuspendLayout();
			this.tpBusIn2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctb_BusIn2)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.tpCode.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctb_Code)).BeginInit();
			this.toolStrip2.SuspendLayout();
			this.tpValueConvert.SuspendLayout();
			this.tlp_ValueConversion.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctbValConv_Hex)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fctbValConv_DFB)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fctbValConv_Int)).BeginInit();
			this.toolStrip10.SuspendLayout();
			this.toolStrip9.SuspendLayout();
			this.toolStrip8.SuspendLayout();
			this.toolStrip7.SuspendLayout();
			this.tabLogAndRam.SuspendLayout();
			this.tpLog.SuspendLayout();
			this.tpRam.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitRams)).BeginInit();
			this.splitRams.Panel1.SuspendLayout();
			this.splitRams.Panel2.SuspendLayout();
			this.splitRams.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitRamACU)).BeginInit();
			this.splitRamACU.Panel1.SuspendLayout();
			this.splitRamACU.Panel2.SuspendLayout();
			this.splitRamACU.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctbRamAcuA)).BeginInit();
			this.toolStrip3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctbRamAcuB)).BeginInit();
			this.toolStrip4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitRamData)).BeginInit();
			this.splitRamData.Panel1.SuspendLayout();
			this.splitRamData.Panel2.SuspendLayout();
			this.splitRamData.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctbRamDataA)).BeginInit();
			this.toolStrip5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctbRamDataB)).BeginInit();
			this.toolStrip6.SuspendLayout();
			this.tpJumpCond.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctb_JumpCond)).BeginInit();
			this.tpCallDiag.SuspendLayout();
			this.tpBusOut1.SuspendLayout();
			this.panel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctbBusOut1)).BeginInit();
			this.tsBus1Out.SuspendLayout();
			this.tpBusOut2.SuspendLayout();
			this.panel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctbBusOut2)).BeginInit();
			this.tsBus2Out.SuspendLayout();
			this.pnlScrub.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackCycle)).BeginInit();
			this.toolMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBottom,
            this.statlblBottom});
			this.statusStrip1.Location = new System.Drawing.Point(0, 948);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1582, 25);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// progressBottom
			// 
			this.progressBottom.Name = "progressBottom";
			this.progressBottom.Size = new System.Drawing.Size(300, 19);
			// 
			// statlblBottom
			// 
			this.statlblBottom.Name = "statlblBottom";
			this.statlblBottom.Size = new System.Drawing.Size(0, 20);
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitMain.Location = new System.Drawing.Point(0, 47);
			this.splitMain.Name = "splitMain";
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.splitCodeTab);
			this.splitMain.Panel1MinSize = 440;
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.webBrowser1);
			this.splitMain.Panel2.Controls.Add(this.pnlScrub);
			this.splitMain.Size = new System.Drawing.Size(1582, 901);
			this.splitMain.SplitterDistance = 606;
			this.splitMain.TabIndex = 4;
			// 
			// splitCodeTab
			// 
			this.splitCodeTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitCodeTab.Location = new System.Drawing.Point(0, 0);
			this.splitCodeTab.Name = "splitCodeTab";
			this.splitCodeTab.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitCodeTab.Panel1
			// 
			this.splitCodeTab.Panel1.Controls.Add(this.tabInfo);
			// 
			// splitCodeTab.Panel2
			// 
			this.splitCodeTab.Panel2.Controls.Add(this.tabLogAndRam);
			this.splitCodeTab.Size = new System.Drawing.Size(606, 901);
			this.splitCodeTab.SplitterDistance = 449;
			this.splitCodeTab.TabIndex = 0;
			// 
			// tabInfo
			// 
			this.tabInfo.Controls.Add(this.tpGlobals);
			this.tabInfo.Controls.Add(this.tpBusIn1);
			this.tabInfo.Controls.Add(this.tpBusIn2);
			this.tabInfo.Controls.Add(this.tpCode);
			this.tabInfo.Controls.Add(this.tpValueConvert);
			this.tabInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabInfo.ImageList = this.imageList1;
			this.tabInfo.ItemSize = new System.Drawing.Size(58, 40);
			this.tabInfo.Location = new System.Drawing.Point(0, 0);
			this.tabInfo.Name = "tabInfo";
			this.tabInfo.SelectedIndex = 0;
			this.tabInfo.Size = new System.Drawing.Size(606, 449);
			this.tabInfo.TabIndex = 3;
			// 
			// tpGlobals
			// 
			this.tpGlobals.Controls.Add(this.panel3);
			this.tpGlobals.Controls.Add(this.toolStrip11);
			this.tpGlobals.ImageKey = "GlobalVariable_64x.png";
			this.tpGlobals.Location = new System.Drawing.Point(4, 44);
			this.tpGlobals.Name = "tpGlobals";
			this.tpGlobals.Size = new System.Drawing.Size(598, 401);
			this.tpGlobals.TabIndex = 5;
			this.tpGlobals.Text = "Globals";
			this.tpGlobals.UseVisualStyleBackColor = true;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.dgvGlobalInputs);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 25);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(598, 376);
			this.panel3.TabIndex = 2;
			// 
			// dgvGlobalInputs
			// 
			this.dgvGlobalInputs.AutoGenerateColumns = false;
			this.dgvGlobalInputs.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dgvGlobalInputs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvGlobalInputs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cycleDataGridViewTextBoxColumn,
            this.globalInput1DataGridViewCheckBoxColumn,
            this.globalInput2DataGridViewCheckBoxColumn,
            this.semaphore0DataGridViewCheckBoxColumn,
            this.semaphore1DataGridViewCheckBoxColumn,
            this.semaphore2DataGridViewCheckBoxColumn});
			this.dgvGlobalInputs.DataSource = this.bsInputSequence;
			this.dgvGlobalInputs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvGlobalInputs.Location = new System.Drawing.Point(0, 0);
			this.dgvGlobalInputs.Name = "dgvGlobalInputs";
			this.dgvGlobalInputs.RowTemplate.Height = 24;
			this.dgvGlobalInputs.Size = new System.Drawing.Size(598, 376);
			this.dgvGlobalInputs.TabIndex = 1;
			// 
			// cycleDataGridViewTextBoxColumn
			// 
			this.cycleDataGridViewTextBoxColumn.DataPropertyName = "Cycle";
			this.cycleDataGridViewTextBoxColumn.HeaderText = "Cycle";
			this.cycleDataGridViewTextBoxColumn.Name = "cycleDataGridViewTextBoxColumn";
			// 
			// globalInput1DataGridViewCheckBoxColumn
			// 
			this.globalInput1DataGridViewCheckBoxColumn.DataPropertyName = "GlobalInput1";
			this.globalInput1DataGridViewCheckBoxColumn.HeaderText = "GlobalInput1";
			this.globalInput1DataGridViewCheckBoxColumn.Name = "globalInput1DataGridViewCheckBoxColumn";
			// 
			// globalInput2DataGridViewCheckBoxColumn
			// 
			this.globalInput2DataGridViewCheckBoxColumn.DataPropertyName = "GlobalInput2";
			this.globalInput2DataGridViewCheckBoxColumn.HeaderText = "GlobalInput2";
			this.globalInput2DataGridViewCheckBoxColumn.Name = "globalInput2DataGridViewCheckBoxColumn";
			// 
			// semaphore0DataGridViewCheckBoxColumn
			// 
			this.semaphore0DataGridViewCheckBoxColumn.DataPropertyName = "Semaphore0";
			this.semaphore0DataGridViewCheckBoxColumn.HeaderText = "Semaphore0";
			this.semaphore0DataGridViewCheckBoxColumn.Name = "semaphore0DataGridViewCheckBoxColumn";
			// 
			// semaphore1DataGridViewCheckBoxColumn
			// 
			this.semaphore1DataGridViewCheckBoxColumn.DataPropertyName = "Semaphore1";
			this.semaphore1DataGridViewCheckBoxColumn.HeaderText = "Semaphore1";
			this.semaphore1DataGridViewCheckBoxColumn.Name = "semaphore1DataGridViewCheckBoxColumn";
			// 
			// semaphore2DataGridViewCheckBoxColumn
			// 
			this.semaphore2DataGridViewCheckBoxColumn.DataPropertyName = "Semaphore2";
			this.semaphore2DataGridViewCheckBoxColumn.HeaderText = "Semaphore2";
			this.semaphore2DataGridViewCheckBoxColumn.Name = "semaphore2DataGridViewCheckBoxColumn";
			// 
			// bsInputSequence
			// 
			this.bsInputSequence.DataMember = "InputSequence";
			this.bsInputSequence.DataSource = typeof(DFBProject.Project);
			// 
			// toolStrip11
			// 
			this.toolStrip11.CanOverflow = false;
			this.toolStrip11.GripMargin = new System.Windows.Forms.Padding(0);
			this.toolStrip11.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip11.ImageScalingSize = new System.Drawing.Size(30, 30);
			this.toolStrip11.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel12});
			this.toolStrip11.Location = new System.Drawing.Point(0, 0);
			this.toolStrip11.Name = "toolStrip11";
			this.toolStrip11.Padding = new System.Windows.Forms.Padding(0);
			this.toolStrip11.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.toolStrip11.Size = new System.Drawing.Size(598, 25);
			this.toolStrip11.TabIndex = 1;
			this.toolStrip11.Text = "toolStrip2";
			// 
			// toolStripLabel12
			// 
			this.toolStripLabel12.Name = "toolStripLabel12";
			this.toolStripLabel12.Size = new System.Drawing.Size(379, 22);
			this.toolStripLabel12.Text = "Enter a cycle number and the globals to set at that cycle";
			// 
			// tpBusIn1
			// 
			this.tpBusIn1.Controls.Add(this.fctb_BusIn1);
			this.tpBusIn1.Controls.Add(this.tollBusIn1);
			this.tpBusIn1.ImageKey = "ServiceQueueSource_48x.png";
			this.tpBusIn1.Location = new System.Drawing.Point(4, 44);
			this.tpBusIn1.Name = "tpBusIn1";
			this.tpBusIn1.Size = new System.Drawing.Size(598, 401);
			this.tpBusIn1.TabIndex = 3;
			this.tpBusIn1.Text = "Stage_A";
			this.tpBusIn1.UseVisualStyleBackColor = true;
			// 
			// fctb_BusIn1
			// 
			this.fctb_BusIn1.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.fctb_BusIn1.AutoScrollMinSize = new System.Drawing.Size(2, 18);
			this.fctb_BusIn1.BackBrush = null;
			this.fctb_BusIn1.CharHeight = 18;
			this.fctb_BusIn1.CharWidth = 10;
			this.fctb_BusIn1.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fctb_BusIn1.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fctb_BusIn1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fctb_BusIn1.IsReplaceMode = false;
			this.fctb_BusIn1.Location = new System.Drawing.Point(0, 37);
			this.fctb_BusIn1.Name = "fctb_BusIn1";
			this.fctb_BusIn1.Paddings = new System.Windows.Forms.Padding(0);
			this.fctb_BusIn1.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fctb_BusIn1.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctb_BusIn1.ServiceColors")));
			this.fctb_BusIn1.Size = new System.Drawing.Size(598, 364);
			this.fctb_BusIn1.TabIndex = 1;
			this.fctb_BusIn1.Zoom = 100;
			this.fctb_BusIn1.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fctb_BusIn1_TextChanged);
			// 
			// tollBusIn1
			// 
			this.tollBusIn1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tollBusIn1.ImageScalingSize = new System.Drawing.Size(30, 30);
			this.tollBusIn1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBusIn1Insert});
			this.tollBusIn1.Location = new System.Drawing.Point(0, 0);
			this.tollBusIn1.Name = "tollBusIn1";
			this.tollBusIn1.Size = new System.Drawing.Size(598, 37);
			this.tollBusIn1.TabIndex = 0;
			this.tollBusIn1.Text = "toolStrip2";
			// 
			// btnBusIn1Insert
			// 
			this.btnBusIn1Insert.Image = global::DFBUtility.Properties.Resources.VSO_Action_Add_12x_16x;
			this.btnBusIn1Insert.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnBusIn1Insert.Name = "btnBusIn1Insert";
			this.btnBusIn1Insert.Size = new System.Drawing.Size(144, 34);
			this.btnBusIn1Insert.Text = "Insert From File";
			this.btnBusIn1Insert.Click += new System.EventHandler(this.btnBusIn1Insert_Click);
			// 
			// tpBusIn2
			// 
			this.tpBusIn2.Controls.Add(this.fctb_BusIn2);
			this.tpBusIn2.Controls.Add(this.toolStrip1);
			this.tpBusIn2.ImageKey = "ServiceQueueSource_48x.png";
			this.tpBusIn2.Location = new System.Drawing.Point(4, 44);
			this.tpBusIn2.Name = "tpBusIn2";
			this.tpBusIn2.Size = new System.Drawing.Size(598, 401);
			this.tpBusIn2.TabIndex = 4;
			this.tpBusIn2.Text = "Stage_B";
			this.tpBusIn2.UseVisualStyleBackColor = true;
			// 
			// fctb_BusIn2
			// 
			this.fctb_BusIn2.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.fctb_BusIn2.AutoScrollMinSize = new System.Drawing.Size(2, 18);
			this.fctb_BusIn2.BackBrush = null;
			this.fctb_BusIn2.CharHeight = 18;
			this.fctb_BusIn2.CharWidth = 10;
			this.fctb_BusIn2.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fctb_BusIn2.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fctb_BusIn2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fctb_BusIn2.IsReplaceMode = false;
			this.fctb_BusIn2.Location = new System.Drawing.Point(0, 37);
			this.fctb_BusIn2.Name = "fctb_BusIn2";
			this.fctb_BusIn2.Paddings = new System.Windows.Forms.Padding(0);
			this.fctb_BusIn2.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fctb_BusIn2.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctb_BusIn2.ServiceColors")));
			this.fctb_BusIn2.Size = new System.Drawing.Size(598, 364);
			this.fctb_BusIn2.TabIndex = 2;
			this.fctb_BusIn2.Zoom = 100;
			this.fctb_BusIn2.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fctb_BusIn2_TextChanged);
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(30, 30);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBusIn2Insert});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(598, 37);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip2";
			// 
			// btnBusIn2Insert
			// 
			this.btnBusIn2Insert.Image = global::DFBUtility.Properties.Resources.VSO_Action_Add_12x_16x;
			this.btnBusIn2Insert.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnBusIn2Insert.Name = "btnBusIn2Insert";
			this.btnBusIn2Insert.Size = new System.Drawing.Size(144, 34);
			this.btnBusIn2Insert.Text = "Insert From File";
			this.btnBusIn2Insert.Click += new System.EventHandler(this.btnBusIn2Insert_Click);
			// 
			// tpCode
			// 
			this.tpCode.Controls.Add(this.fctb_Code);
			this.tpCode.Controls.Add(this.toolStrip2);
			this.tpCode.ImageKey = "PowerShellScript_64x.png";
			this.tpCode.Location = new System.Drawing.Point(4, 44);
			this.tpCode.Name = "tpCode";
			this.tpCode.Size = new System.Drawing.Size(598, 401);
			this.tpCode.TabIndex = 2;
			this.tpCode.Text = "Code";
			this.tpCode.UseVisualStyleBackColor = true;
			// 
			// fctb_Code
			// 
			this.fctb_Code.AllowMacroRecording = false;
			this.fctb_Code.AutoCompleteBracketsList = new char[0];
			this.fctb_Code.AutoIndent = false;
			this.fctb_Code.AutoIndentChars = false;
			this.fctb_Code.AutoIndentCharsPatterns = "";
			this.fctb_Code.AutoIndentExistingLines = false;
			this.fctb_Code.AutoScrollMinSize = new System.Drawing.Size(17, 18);
			this.fctb_Code.BackBrush = null;
			this.fctb_Code.CharHeight = 18;
			this.fctb_Code.CharWidth = 10;
			this.fctb_Code.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fctb_Code.DelayedEventsInterval = 50;
			this.fctb_Code.DelayedTextChangedInterval = 50;
			this.fctb_Code.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fctb_Code.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fctb_Code.IsReplaceMode = false;
			this.fctb_Code.LeftBracket = '0';
			this.fctb_Code.LeftBracket2 = '0';
			this.fctb_Code.LeftPadding = 15;
			this.fctb_Code.Location = new System.Drawing.Point(0, 37);
			this.fctb_Code.Margin = new System.Windows.Forms.Padding(4);
			this.fctb_Code.Name = "fctb_Code";
			this.fctb_Code.Paddings = new System.Windows.Forms.Padding(0);
			this.fctb_Code.RightBracket = '0';
			this.fctb_Code.RightBracket2 = '0';
			this.fctb_Code.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fctb_Code.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctb_Code.ServiceColors")));
			this.fctb_Code.Size = new System.Drawing.Size(598, 364);
			this.fctb_Code.TabIndex = 5;
			this.fctb_Code.Zoom = 100;
			this.fctb_Code.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fctb_Code_TextChanged);
			this.fctb_Code.MouseClick += new System.Windows.Forms.MouseEventHandler(this.fctb_Code_MouseClick);
			// 
			// toolStrip2
			// 
			this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip2.ImageScalingSize = new System.Drawing.Size(30, 30);
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCodeInsert,
            this.toolStripSeparator2,
            this.lblCurrentCodeFile,
            this.toolStripLabel11});
			this.toolStrip2.Location = new System.Drawing.Point(0, 0);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(598, 37);
			this.toolStrip2.TabIndex = 2;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// btnCodeInsert
			// 
			this.btnCodeInsert.Image = global::DFBUtility.Properties.Resources.VSO_Action_Add_12x_16x;
			this.btnCodeInsert.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnCodeInsert.Name = "btnCodeInsert";
			this.btnCodeInsert.Size = new System.Drawing.Size(144, 34);
			this.btnCodeInsert.Text = "Insert From File";
			this.btnCodeInsert.Click += new System.EventHandler(this.btnCodeInsert_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 37);
			// 
			// lblCurrentCodeFile
			// 
			this.lblCurrentCodeFile.Name = "lblCurrentCodeFile";
			this.lblCurrentCodeFile.Size = new System.Drawing.Size(0, 34);
			// 
			// toolStripLabel11
			// 
			this.toolStripLabel11.Name = "toolStripLabel11";
			this.toolStripLabel11.Size = new System.Drawing.Size(355, 34);
			this.toolStripLabel11.Text = "When stepping, line that executed last is highlighted";
			// 
			// tpValueConvert
			// 
			this.tpValueConvert.Controls.Add(this.tlp_ValueConversion);
			this.tpValueConvert.Controls.Add(this.toolStrip7);
			this.tpValueConvert.ImageKey = "ConvertPartition_64x.png";
			this.tpValueConvert.Location = new System.Drawing.Point(4, 44);
			this.tpValueConvert.Name = "tpValueConvert";
			this.tpValueConvert.Size = new System.Drawing.Size(598, 401);
			this.tpValueConvert.TabIndex = 6;
			this.tpValueConvert.Text = "Value Converter";
			this.tpValueConvert.UseVisualStyleBackColor = true;
			// 
			// tlp_ValueConversion
			// 
			this.tlp_ValueConversion.ColumnCount = 3;
			this.tlp_ValueConversion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tlp_ValueConversion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tlp_ValueConversion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tlp_ValueConversion.Controls.Add(this.fctbValConv_Hex, 0, 1);
			this.tlp_ValueConversion.Controls.Add(this.fctbValConv_DFB, 0, 1);
			this.tlp_ValueConversion.Controls.Add(this.fctbValConv_Int, 0, 1);
			this.tlp_ValueConversion.Controls.Add(this.toolStrip10, 2, 0);
			this.tlp_ValueConversion.Controls.Add(this.toolStrip9, 1, 0);
			this.tlp_ValueConversion.Controls.Add(this.toolStrip8, 0, 0);
			this.tlp_ValueConversion.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlp_ValueConversion.Location = new System.Drawing.Point(0, 37);
			this.tlp_ValueConversion.Name = "tlp_ValueConversion";
			this.tlp_ValueConversion.RowCount = 2;
			this.tlp_ValueConversion.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlp_ValueConversion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlp_ValueConversion.Size = new System.Drawing.Size(598, 364);
			this.tlp_ValueConversion.TabIndex = 4;
			// 
			// fctbValConv_Hex
			// 
			this.fctbValConv_Hex.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.fctbValConv_Hex.AutoScrollMinSize = new System.Drawing.Size(2, 18);
			this.fctbValConv_Hex.BackBrush = null;
			this.fctbValConv_Hex.CharHeight = 18;
			this.fctbValConv_Hex.CharWidth = 10;
			this.fctbValConv_Hex.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fctbValConv_Hex.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fctbValConv_Hex.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fctbValConv_Hex.IsReplaceMode = false;
			this.fctbValConv_Hex.LineNumberStartValue = ((uint)(0u));
			this.fctbValConv_Hex.Location = new System.Drawing.Point(3, 28);
			this.fctbValConv_Hex.Name = "fctbValConv_Hex";
			this.fctbValConv_Hex.Paddings = new System.Windows.Forms.Padding(0);
			this.fctbValConv_Hex.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fctbValConv_Hex.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbValConv_Hex.ServiceColors")));
			this.fctbValConv_Hex.Size = new System.Drawing.Size(193, 333);
			this.fctbValConv_Hex.TabIndex = 7;
			this.fctbValConv_Hex.Zoom = 100;
			// 
			// fctbValConv_DFB
			// 
			this.fctbValConv_DFB.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.fctbValConv_DFB.AutoScrollMinSize = new System.Drawing.Size(2, 18);
			this.fctbValConv_DFB.BackBrush = null;
			this.fctbValConv_DFB.CharHeight = 18;
			this.fctbValConv_DFB.CharWidth = 10;
			this.fctbValConv_DFB.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fctbValConv_DFB.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fctbValConv_DFB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fctbValConv_DFB.IsReplaceMode = false;
			this.fctbValConv_DFB.LineNumberStartValue = ((uint)(0u));
			this.fctbValConv_DFB.Location = new System.Drawing.Point(202, 28);
			this.fctbValConv_DFB.Name = "fctbValConv_DFB";
			this.fctbValConv_DFB.Paddings = new System.Windows.Forms.Padding(0);
			this.fctbValConv_DFB.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fctbValConv_DFB.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbValConv_DFB.ServiceColors")));
			this.fctbValConv_DFB.Size = new System.Drawing.Size(193, 333);
			this.fctbValConv_DFB.TabIndex = 6;
			this.fctbValConv_DFB.Zoom = 100;
			// 
			// fctbValConv_Int
			// 
			this.fctbValConv_Int.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.fctbValConv_Int.AutoScrollMinSize = new System.Drawing.Size(2, 18);
			this.fctbValConv_Int.BackBrush = null;
			this.fctbValConv_Int.CharHeight = 18;
			this.fctbValConv_Int.CharWidth = 10;
			this.fctbValConv_Int.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fctbValConv_Int.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fctbValConv_Int.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fctbValConv_Int.IsReplaceMode = false;
			this.fctbValConv_Int.LineNumberStartValue = ((uint)(0u));
			this.fctbValConv_Int.Location = new System.Drawing.Point(401, 28);
			this.fctbValConv_Int.Name = "fctbValConv_Int";
			this.fctbValConv_Int.Paddings = new System.Windows.Forms.Padding(0);
			this.fctbValConv_Int.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fctbValConv_Int.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbValConv_Int.ServiceColors")));
			this.fctbValConv_Int.Size = new System.Drawing.Size(194, 333);
			this.fctbValConv_Int.TabIndex = 5;
			this.fctbValConv_Int.Zoom = 100;
			// 
			// toolStrip10
			// 
			this.toolStrip10.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip10.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip10.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel10});
			this.toolStrip10.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.toolStrip10.Location = new System.Drawing.Point(398, 0);
			this.toolStrip10.Name = "toolStrip10";
			this.toolStrip10.Size = new System.Drawing.Size(200, 25);
			this.toolStrip10.Stretch = true;
			this.toolStrip10.TabIndex = 4;
			this.toolStrip10.Text = "Integral";
			// 
			// toolStripLabel10
			// 
			this.toolStripLabel10.Name = "toolStripLabel10";
			this.toolStripLabel10.Size = new System.Drawing.Size(106, 22);
			this.toolStripLabel10.Text = "Signed Integer";
			// 
			// toolStrip9
			// 
			this.toolStrip9.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip9.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip9.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel9});
			this.toolStrip9.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.toolStrip9.Location = new System.Drawing.Point(199, 0);
			this.toolStrip9.Name = "toolStrip9";
			this.toolStrip9.Size = new System.Drawing.Size(199, 25);
			this.toolStrip9.Stretch = true;
			this.toolStrip9.TabIndex = 3;
			this.toolStrip9.Text = "toolStrip9";
			// 
			// toolStripLabel9
			// 
			this.toolStripLabel9.Name = "toolStripLabel9";
			this.toolStripLabel9.Size = new System.Drawing.Size(70, 22);
			this.toolStripLabel9.Text = "DFB Q.23";
			// 
			// toolStrip8
			// 
			this.toolStrip8.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip8.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip8.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel8});
			this.toolStrip8.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.toolStrip8.Location = new System.Drawing.Point(0, 0);
			this.toolStrip8.Name = "toolStrip8";
			this.toolStrip8.Size = new System.Drawing.Size(199, 25);
			this.toolStrip8.Stretch = true;
			this.toolStrip8.TabIndex = 2;
			this.toolStrip8.Text = "toolStrip8";
			// 
			// toolStripLabel8
			// 
			this.toolStripLabel8.Name = "toolStripLabel8";
			this.toolStripLabel8.Size = new System.Drawing.Size(131, 22);
			this.toolStripLabel8.Text = "Hex (0x) + 6 digits";
			// 
			// toolStrip7
			// 
			this.toolStrip7.AutoSize = false;
			this.toolStrip7.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip7.ImageScalingSize = new System.Drawing.Size(30, 30);
			this.toolStrip7.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel6,
            this.toolStripLabel7});
			this.toolStrip7.Location = new System.Drawing.Point(0, 0);
			this.toolStrip7.Name = "toolStrip7";
			this.toolStrip7.Size = new System.Drawing.Size(598, 37);
			this.toolStrip7.TabIndex = 3;
			this.toolStrip7.Text = "toolStrip7";
			// 
			// toolStripLabel6
			// 
			this.toolStripLabel6.Name = "toolStripLabel6";
			this.toolStripLabel6.Size = new System.Drawing.Size(0, 34);
			// 
			// toolStripLabel7
			// 
			this.toolStripLabel7.Name = "toolStripLabel7";
			this.toolStripLabel7.Size = new System.Drawing.Size(403, 34);
			this.toolStripLabel7.Text = "Enter / change values in one format to convert to the others";
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "EventLog_32x.png");
			this.imageList1.Images.SetKeyName(1, "Memory_32x.png");
			this.imageList1.Images.SetKeyName(2, "Branch_64x.png");
			this.imageList1.Images.SetKeyName(3, "Web_64x.png");
			this.imageList1.Images.SetKeyName(4, "ServiceBusSubscriptionTrid_16x_32.bmp");
			this.imageList1.Images.SetKeyName(5, "ServiceQueueSource_48x.png");
			this.imageList1.Images.SetKeyName(6, "GlobalVariable_64x.png");
			this.imageList1.Images.SetKeyName(7, "GoToSourceCode_64x.png");
			this.imageList1.Images.SetKeyName(8, "PowerShellScript_64x.png");
			this.imageList1.Images.SetKeyName(9, "ConvertPartition_64x.png");
			this.imageList1.Images.SetKeyName(10, "ServiceQueueDestination_48x.png");
			this.imageList1.Images.SetKeyName(11, "Hierarchy_48x.png");
			this.imageList1.Images.SetKeyName(12, "CheckBox_32x.png");
			// 
			// tabLogAndRam
			// 
			this.tabLogAndRam.Controls.Add(this.tpLog);
			this.tabLogAndRam.Controls.Add(this.tpRam);
			this.tabLogAndRam.Controls.Add(this.tpJumpCond);
			this.tabLogAndRam.Controls.Add(this.tpCallDiag);
			this.tabLogAndRam.Controls.Add(this.tpBusOut1);
			this.tabLogAndRam.Controls.Add(this.tpBusOut2);
			this.tabLogAndRam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabLogAndRam.ImageList = this.imageList1;
			this.tabLogAndRam.Location = new System.Drawing.Point(0, 0);
			this.tabLogAndRam.Name = "tabLogAndRam";
			this.tabLogAndRam.SelectedIndex = 0;
			this.tabLogAndRam.Size = new System.Drawing.Size(606, 448);
			this.tabLogAndRam.TabIndex = 0;
			// 
			// tpLog
			// 
			this.tpLog.Controls.Add(this.rtbMessages);
			this.tpLog.ImageIndex = 0;
			this.tpLog.Location = new System.Drawing.Point(4, 39);
			this.tpLog.Name = "tpLog";
			this.tpLog.Size = new System.Drawing.Size(598, 405);
			this.tpLog.TabIndex = 2;
			this.tpLog.Text = "Log";
			this.tpLog.UseVisualStyleBackColor = true;
			// 
			// rtbMessages
			// 
			this.rtbMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtbMessages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbMessages.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rtbMessages.Location = new System.Drawing.Point(0, 0);
			this.rtbMessages.Name = "rtbMessages";
			this.rtbMessages.Size = new System.Drawing.Size(598, 405);
			this.rtbMessages.TabIndex = 1;
			this.rtbMessages.Text = "";
			this.rtbMessages.WordWrap = false;
			// 
			// tpRam
			// 
			this.tpRam.Controls.Add(this.splitRams);
			this.tpRam.ImageIndex = 1;
			this.tpRam.Location = new System.Drawing.Point(4, 39);
			this.tpRam.Name = "tpRam";
			this.tpRam.Padding = new System.Windows.Forms.Padding(3);
			this.tpRam.Size = new System.Drawing.Size(598, 405);
			this.tpRam.TabIndex = 0;
			this.tpRam.Text = "Ram";
			this.tpRam.UseVisualStyleBackColor = true;
			// 
			// splitRams
			// 
			this.splitRams.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitRams.Location = new System.Drawing.Point(3, 3);
			this.splitRams.Name = "splitRams";
			this.splitRams.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitRams.Panel1
			// 
			this.splitRams.Panel1.Controls.Add(this.splitRamACU);
			// 
			// splitRams.Panel2
			// 
			this.splitRams.Panel2.Controls.Add(this.splitRamData);
			this.splitRams.Size = new System.Drawing.Size(592, 399);
			this.splitRams.SplitterDistance = 158;
			this.splitRams.TabIndex = 0;
			// 
			// splitRamACU
			// 
			this.splitRamACU.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitRamACU.Location = new System.Drawing.Point(0, 0);
			this.splitRamACU.Name = "splitRamACU";
			// 
			// splitRamACU.Panel1
			// 
			this.splitRamACU.Panel1.Controls.Add(this.panel1);
			this.splitRamACU.Panel1.Controls.Add(this.toolStrip3);
			// 
			// splitRamACU.Panel2
			// 
			this.splitRamACU.Panel2.Controls.Add(this.fctbRamAcuB);
			this.splitRamACU.Panel2.Controls.Add(this.toolStrip4);
			this.splitRamACU.Panel2.Controls.Add(this.panel2);
			this.splitRamACU.Size = new System.Drawing.Size(592, 158);
			this.splitRamACU.SplitterDistance = 263;
			this.splitRamACU.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.fctbRamAcuA);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 27);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(263, 131);
			this.panel1.TabIndex = 2;
			// 
			// fctbRamAcuA
			// 
			this.fctbRamAcuA.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.fctbRamAcuA.AutoScrollMinSize = new System.Drawing.Size(12, 18);
			this.fctbRamAcuA.BackBrush = null;
			this.fctbRamAcuA.CharHeight = 18;
			this.fctbRamAcuA.CharWidth = 10;
			this.fctbRamAcuA.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fctbRamAcuA.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fctbRamAcuA.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fctbRamAcuA.IsReplaceMode = false;
			this.fctbRamAcuA.LeftPadding = 10;
			this.fctbRamAcuA.LineNumberStartValue = ((uint)(0u));
			this.fctbRamAcuA.Location = new System.Drawing.Point(0, 0);
			this.fctbRamAcuA.Name = "fctbRamAcuA";
			this.fctbRamAcuA.Paddings = new System.Windows.Forms.Padding(0);
			this.fctbRamAcuA.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fctbRamAcuA.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbRamAcuA.ServiceColors")));
			this.fctbRamAcuA.Size = new System.Drawing.Size(263, 131);
			this.fctbRamAcuA.TabIndex = 2;
			this.fctbRamAcuA.Zoom = 100;
			// 
			// toolStrip3
			// 
			this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.acuRamAPanelHide});
			this.toolStrip3.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.toolStrip3.Location = new System.Drawing.Point(0, 0);
			this.toolStrip3.Name = "toolStrip3";
			this.toolStrip3.Size = new System.Drawing.Size(263, 27);
			this.toolStrip3.Stretch = true;
			this.toolStrip3.TabIndex = 1;
			this.toolStrip3.Text = "toolStrip3";
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(86, 24);
			this.toolStripLabel2.Text = "ACU Ram A";
			// 
			// acuRamAPanelHide
			// 
			this.acuRamAPanelHide.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.acuRamAPanelHide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.acuRamAPanelHide.Image = global::DFBUtility.Properties.Resources.Collapse_16x_24;
			this.acuRamAPanelHide.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.acuRamAPanelHide.Name = "acuRamAPanelHide";
			this.acuRamAPanelHide.Size = new System.Drawing.Size(24, 24);
			this.acuRamAPanelHide.Text = "Hide";
			this.acuRamAPanelHide.Click += new System.EventHandler(this.acuRamAPanelHide_Click);
			// 
			// fctbRamAcuB
			// 
			this.fctbRamAcuB.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.fctbRamAcuB.AutoScrollMinSize = new System.Drawing.Size(12, 18);
			this.fctbRamAcuB.BackBrush = null;
			this.fctbRamAcuB.CharHeight = 18;
			this.fctbRamAcuB.CharWidth = 10;
			this.fctbRamAcuB.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fctbRamAcuB.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fctbRamAcuB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fctbRamAcuB.IsReplaceMode = false;
			this.fctbRamAcuB.LeftPadding = 10;
			this.fctbRamAcuB.LineNumberStartValue = ((uint)(0u));
			this.fctbRamAcuB.Location = new System.Drawing.Point(0, 27);
			this.fctbRamAcuB.Name = "fctbRamAcuB";
			this.fctbRamAcuB.Paddings = new System.Windows.Forms.Padding(0);
			this.fctbRamAcuB.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fctbRamAcuB.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbRamAcuB.ServiceColors")));
			this.fctbRamAcuB.Size = new System.Drawing.Size(325, 131);
			this.fctbRamAcuB.TabIndex = 4;
			this.fctbRamAcuB.Zoom = 100;
			// 
			// toolStrip4
			// 
			this.toolStrip4.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.acuRamBPanelHide});
			this.toolStrip4.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.toolStrip4.Location = new System.Drawing.Point(0, 0);
			this.toolStrip4.Name = "toolStrip4";
			this.toolStrip4.Size = new System.Drawing.Size(325, 27);
			this.toolStrip4.TabIndex = 2;
			this.toolStrip4.Text = "toolStrip4";
			// 
			// toolStripLabel3
			// 
			this.toolStripLabel3.Name = "toolStripLabel3";
			this.toolStripLabel3.Size = new System.Drawing.Size(85, 24);
			this.toolStripLabel3.Text = "ACU Ram B";
			// 
			// acuRamBPanelHide
			// 
			this.acuRamBPanelHide.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.acuRamBPanelHide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.acuRamBPanelHide.Image = global::DFBUtility.Properties.Resources.Collapse_16x_24;
			this.acuRamBPanelHide.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.acuRamBPanelHide.Name = "acuRamBPanelHide";
			this.acuRamBPanelHide.Size = new System.Drawing.Size(24, 24);
			this.acuRamBPanelHide.Text = "Hide";
			this.acuRamBPanelHide.Click += new System.EventHandler(this.acuRamBPanelHide_Click);
			// 
			// panel2
			// 
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(325, 158);
			this.panel2.TabIndex = 3;
			// 
			// splitRamData
			// 
			this.splitRamData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitRamData.Location = new System.Drawing.Point(0, 0);
			this.splitRamData.Name = "splitRamData";
			// 
			// splitRamData.Panel1
			// 
			this.splitRamData.Panel1.Controls.Add(this.fctbRamDataA);
			this.splitRamData.Panel1.Controls.Add(this.toolStrip5);
			// 
			// splitRamData.Panel2
			// 
			this.splitRamData.Panel2.Controls.Add(this.fctbRamDataB);
			this.splitRamData.Panel2.Controls.Add(this.toolStrip6);
			this.splitRamData.Size = new System.Drawing.Size(592, 237);
			this.splitRamData.SplitterDistance = 261;
			this.splitRamData.TabIndex = 0;
			// 
			// fctbRamDataA
			// 
			this.fctbRamDataA.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.fctbRamDataA.AutoScrollMinSize = new System.Drawing.Size(12, 18);
			this.fctbRamDataA.BackBrush = null;
			this.fctbRamDataA.CharHeight = 18;
			this.fctbRamDataA.CharWidth = 10;
			this.fctbRamDataA.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fctbRamDataA.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fctbRamDataA.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fctbRamDataA.IsReplaceMode = false;
			this.fctbRamDataA.LeftPadding = 10;
			this.fctbRamDataA.LineNumberStartValue = ((uint)(0u));
			this.fctbRamDataA.Location = new System.Drawing.Point(0, 27);
			this.fctbRamDataA.Name = "fctbRamDataA";
			this.fctbRamDataA.Paddings = new System.Windows.Forms.Padding(0);
			this.fctbRamDataA.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fctbRamDataA.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbRamDataA.ServiceColors")));
			this.fctbRamDataA.Size = new System.Drawing.Size(261, 210);
			this.fctbRamDataA.TabIndex = 3;
			this.fctbRamDataA.Zoom = 100;
			// 
			// toolStrip5
			// 
			this.toolStrip5.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip5.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel4,
            this.dataRamAPanelHide});
			this.toolStrip5.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.toolStrip5.Location = new System.Drawing.Point(0, 0);
			this.toolStrip5.Name = "toolStrip5";
			this.toolStrip5.Size = new System.Drawing.Size(261, 27);
			this.toolStrip5.Stretch = true;
			this.toolStrip5.TabIndex = 2;
			this.toolStrip5.Text = "toolStrip5";
			// 
			// toolStripLabel4
			// 
			this.toolStripLabel4.Name = "toolStripLabel4";
			this.toolStripLabel4.Size = new System.Drawing.Size(89, 24);
			this.toolStripLabel4.Text = "Data Ram A";
			// 
			// dataRamAPanelHide
			// 
			this.dataRamAPanelHide.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.dataRamAPanelHide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.dataRamAPanelHide.Image = global::DFBUtility.Properties.Resources.Collapse_16x_24;
			this.dataRamAPanelHide.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.dataRamAPanelHide.Name = "dataRamAPanelHide";
			this.dataRamAPanelHide.Size = new System.Drawing.Size(24, 24);
			this.dataRamAPanelHide.Text = "Hide";
			this.dataRamAPanelHide.Click += new System.EventHandler(this.dataRamAPanelHide_Click);
			// 
			// fctbRamDataB
			// 
			this.fctbRamDataB.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.fctbRamDataB.AutoScrollMinSize = new System.Drawing.Size(12, 18);
			this.fctbRamDataB.BackBrush = null;
			this.fctbRamDataB.CharHeight = 18;
			this.fctbRamDataB.CharWidth = 10;
			this.fctbRamDataB.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fctbRamDataB.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fctbRamDataB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fctbRamDataB.IsReplaceMode = false;
			this.fctbRamDataB.LeftPadding = 10;
			this.fctbRamDataB.LineNumberStartValue = ((uint)(0u));
			this.fctbRamDataB.Location = new System.Drawing.Point(0, 27);
			this.fctbRamDataB.Name = "fctbRamDataB";
			this.fctbRamDataB.Paddings = new System.Windows.Forms.Padding(0);
			this.fctbRamDataB.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fctbRamDataB.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbRamDataB.ServiceColors")));
			this.fctbRamDataB.Size = new System.Drawing.Size(327, 210);
			this.fctbRamDataB.TabIndex = 4;
			this.fctbRamDataB.Zoom = 100;
			// 
			// toolStrip6
			// 
			this.toolStrip6.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip6.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel5,
            this.dataRamBPanelHide});
			this.toolStrip6.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.toolStrip6.Location = new System.Drawing.Point(0, 0);
			this.toolStrip6.Name = "toolStrip6";
			this.toolStrip6.Size = new System.Drawing.Size(327, 27);
			this.toolStrip6.Stretch = true;
			this.toolStrip6.TabIndex = 2;
			this.toolStrip6.Text = "toolStrip6";
			// 
			// toolStripLabel5
			// 
			this.toolStripLabel5.Name = "toolStripLabel5";
			this.toolStripLabel5.Size = new System.Drawing.Size(88, 24);
			this.toolStripLabel5.Text = "Data Ram B";
			// 
			// dataRamBPanelHide
			// 
			this.dataRamBPanelHide.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.dataRamBPanelHide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.dataRamBPanelHide.Image = global::DFBUtility.Properties.Resources.Collapse_16x_24;
			this.dataRamBPanelHide.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.dataRamBPanelHide.Name = "dataRamBPanelHide";
			this.dataRamBPanelHide.Size = new System.Drawing.Size(24, 24);
			this.dataRamBPanelHide.Text = "Hide";
			this.dataRamBPanelHide.Click += new System.EventHandler(this.dataRamBPanelHide_Click);
			// 
			// tpJumpCond
			// 
			this.tpJumpCond.Controls.Add(this.fctb_JumpCond);
			this.tpJumpCond.ImageKey = "Branch_64x.png";
			this.tpJumpCond.Location = new System.Drawing.Point(4, 39);
			this.tpJumpCond.Name = "tpJumpCond";
			this.tpJumpCond.Size = new System.Drawing.Size(598, 405);
			this.tpJumpCond.TabIndex = 3;
			this.tpJumpCond.Text = "Jump Cond";
			this.tpJumpCond.UseVisualStyleBackColor = true;
			// 
			// fctb_JumpCond
			// 
			this.fctb_JumpCond.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.fctb_JumpCond.AutoScrollMinSize = new System.Drawing.Size(2, 18);
			this.fctb_JumpCond.BackBrush = null;
			this.fctb_JumpCond.CharHeight = 18;
			this.fctb_JumpCond.CharWidth = 10;
			this.fctb_JumpCond.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fctb_JumpCond.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fctb_JumpCond.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fctb_JumpCond.IsReplaceMode = false;
			this.fctb_JumpCond.Location = new System.Drawing.Point(0, 0);
			this.fctb_JumpCond.Name = "fctb_JumpCond";
			this.fctb_JumpCond.Paddings = new System.Windows.Forms.Padding(0);
			this.fctb_JumpCond.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fctb_JumpCond.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctb_JumpCond.ServiceColors")));
			this.fctb_JumpCond.Size = new System.Drawing.Size(598, 405);
			this.fctb_JumpCond.TabIndex = 0;
			this.fctb_JumpCond.Zoom = 100;
			// 
			// tpCallDiag
			// 
			this.tpCallDiag.Controls.Add(this.webBrowser2);
			this.tpCallDiag.ImageKey = "Hierarchy_48x.png";
			this.tpCallDiag.Location = new System.Drawing.Point(4, 39);
			this.tpCallDiag.Name = "tpCallDiag";
			this.tpCallDiag.Size = new System.Drawing.Size(598, 405);
			this.tpCallDiag.TabIndex = 6;
			this.tpCallDiag.Text = "Call Diagram";
			this.tpCallDiag.UseVisualStyleBackColor = true;
			// 
			// webBrowser2
			// 
			this.webBrowser2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser2.Location = new System.Drawing.Point(0, 0);
			this.webBrowser2.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser2.Name = "webBrowser2";
			this.webBrowser2.Size = new System.Drawing.Size(598, 405);
			this.webBrowser2.TabIndex = 4;
			// 
			// tpBusOut1
			// 
			this.tpBusOut1.Controls.Add(this.panel4);
			this.tpBusOut1.Controls.Add(this.tsBus1Out);
			this.tpBusOut1.ImageKey = "ServiceQueueDestination_48x.png";
			this.tpBusOut1.Location = new System.Drawing.Point(4, 39);
			this.tpBusOut1.Name = "tpBusOut1";
			this.tpBusOut1.Size = new System.Drawing.Size(598, 405);
			this.tpBusOut1.TabIndex = 4;
			this.tpBusOut1.Text = "Hold_A";
			this.tpBusOut1.UseVisualStyleBackColor = true;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.fctbBusOut1);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(0, 28);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(598, 377);
			this.panel4.TabIndex = 3;
			// 
			// fctbBusOut1
			// 
			this.fctbBusOut1.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.fctbBusOut1.AutoScrollMinSize = new System.Drawing.Size(12, 18);
			this.fctbBusOut1.BackBrush = null;
			this.fctbBusOut1.CharHeight = 18;
			this.fctbBusOut1.CharWidth = 10;
			this.fctbBusOut1.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fctbBusOut1.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fctbBusOut1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fctbBusOut1.IsReplaceMode = false;
			this.fctbBusOut1.LeftPadding = 10;
			this.fctbBusOut1.LineNumberStartValue = ((uint)(0u));
			this.fctbBusOut1.Location = new System.Drawing.Point(0, 0);
			this.fctbBusOut1.Name = "fctbBusOut1";
			this.fctbBusOut1.Paddings = new System.Windows.Forms.Padding(0);
			this.fctbBusOut1.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fctbBusOut1.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbBusOut1.ServiceColors")));
			this.fctbBusOut1.Size = new System.Drawing.Size(598, 377);
			this.fctbBusOut1.TabIndex = 2;
			this.fctbBusOut1.Zoom = 100;
			// 
			// tsBus1Out
			// 
			this.tsBus1Out.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsBus1Out.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.tsBus1Out.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel13,
            this.toolStripSeparator3,
            this.cbBusOut1Format,
            this.toolStripSeparator4});
			this.tsBus1Out.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.tsBus1Out.Location = new System.Drawing.Point(0, 0);
			this.tsBus1Out.Name = "tsBus1Out";
			this.tsBus1Out.Size = new System.Drawing.Size(598, 28);
			this.tsBus1Out.Stretch = true;
			this.tsBus1Out.TabIndex = 2;
			this.tsBus1Out.Text = "toolStrip12";
			// 
			// toolStripLabel13
			// 
			this.toolStripLabel13.Name = "toolStripLabel13";
			this.toolStripLabel13.Size = new System.Drawing.Size(133, 25);
			this.toolStripLabel13.Text = "Hold A Bus Output";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 28);
			// 
			// cbBusOut1Format
			// 
			this.cbBusOut1Format.Items.AddRange(new object[] {
            "Hex",
            "Int",
            "q.23"});
			this.cbBusOut1Format.Name = "cbBusOut1Format";
			this.cbBusOut1Format.Size = new System.Drawing.Size(75, 28);
			this.cbBusOut1Format.SelectedIndexChanged += new System.EventHandler(this.cbBusOut1Format_SelectedIndexChanged);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 28);
			// 
			// tpBusOut2
			// 
			this.tpBusOut2.Controls.Add(this.panel5);
			this.tpBusOut2.Controls.Add(this.tsBus2Out);
			this.tpBusOut2.ImageKey = "ServiceQueueDestination_48x.png";
			this.tpBusOut2.Location = new System.Drawing.Point(4, 39);
			this.tpBusOut2.Name = "tpBusOut2";
			this.tpBusOut2.Size = new System.Drawing.Size(598, 405);
			this.tpBusOut2.TabIndex = 5;
			this.tpBusOut2.Text = "Hold_B";
			this.tpBusOut2.UseVisualStyleBackColor = true;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.fctbBusOut2);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel5.Location = new System.Drawing.Point(0, 28);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(598, 377);
			this.panel5.TabIndex = 4;
			// 
			// fctbBusOut2
			// 
			this.fctbBusOut2.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.fctbBusOut2.AutoScrollMinSize = new System.Drawing.Size(12, 18);
			this.fctbBusOut2.BackBrush = null;
			this.fctbBusOut2.CharHeight = 18;
			this.fctbBusOut2.CharWidth = 10;
			this.fctbBusOut2.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fctbBusOut2.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fctbBusOut2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fctbBusOut2.IsReplaceMode = false;
			this.fctbBusOut2.LeftPadding = 10;
			this.fctbBusOut2.LineNumberStartValue = ((uint)(0u));
			this.fctbBusOut2.Location = new System.Drawing.Point(0, 0);
			this.fctbBusOut2.Name = "fctbBusOut2";
			this.fctbBusOut2.Paddings = new System.Windows.Forms.Padding(0);
			this.fctbBusOut2.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fctbBusOut2.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbBusOut2.ServiceColors")));
			this.fctbBusOut2.Size = new System.Drawing.Size(598, 377);
			this.fctbBusOut2.TabIndex = 2;
			this.fctbBusOut2.Zoom = 100;
			// 
			// tsBus2Out
			// 
			this.tsBus2Out.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsBus2Out.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.tsBus2Out.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel14,
            this.toolStripSeparator7,
            this.cbBusOut2Format,
            this.toolStripSeparator8});
			this.tsBus2Out.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.tsBus2Out.Location = new System.Drawing.Point(0, 0);
			this.tsBus2Out.Name = "tsBus2Out";
			this.tsBus2Out.Size = new System.Drawing.Size(598, 28);
			this.tsBus2Out.Stretch = true;
			this.tsBus2Out.TabIndex = 3;
			this.tsBus2Out.Text = "toolStrip13";
			// 
			// toolStripLabel14
			// 
			this.toolStripLabel14.Name = "toolStripLabel14";
			this.toolStripLabel14.Size = new System.Drawing.Size(132, 25);
			this.toolStripLabel14.Text = "Hold B Bus Output";
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 28);
			// 
			// cbBusOut2Format
			// 
			this.cbBusOut2Format.Items.AddRange(new object[] {
            "Hex",
            "Int",
            "q.23"});
			this.cbBusOut2Format.Name = "cbBusOut2Format";
			this.cbBusOut2Format.Size = new System.Drawing.Size(75, 28);
			this.cbBusOut2Format.SelectedIndexChanged += new System.EventHandler(this.cbBusOut2Format_SelectedIndexChanged);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(6, 28);
			// 
			// webBrowser1
			// 
			this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser1.Location = new System.Drawing.Point(0, 0);
			this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new System.Drawing.Size(972, 861);
			this.webBrowser1.TabIndex = 2;
			this.webBrowser1.SizeChanged += new System.EventHandler(this.webBrowser1_SizeChanged);
			// 
			// pnlScrub
			// 
			this.pnlScrub.Controls.Add(this.tableLayoutPanel1);
			this.pnlScrub.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlScrub.Location = new System.Drawing.Point(0, 861);
			this.pnlScrub.Name = "pnlScrub";
			this.pnlScrub.Size = new System.Drawing.Size(972, 40);
			this.pnlScrub.TabIndex = 1;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.Controls.Add(this.tbCurrCycle, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblCycleEnd, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.trackCycle, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(972, 40);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tbCurrCycle
			// 
			this.tbCurrCycle.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.tbCurrCycle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbCurrCycle.Location = new System.Drawing.Point(3, 5);
			this.tbCurrCycle.Name = "tbCurrCycle";
			this.tbCurrCycle.Size = new System.Drawing.Size(94, 30);
			this.tbCurrCycle.TabIndex = 0;
			this.tbCurrCycle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbCurrCycle_KeyDown);
			this.tbCurrCycle.Leave += new System.EventHandler(this.tbCurrCycle_Leave);
			// 
			// lblCycleEnd
			// 
			this.lblCycleEnd.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblCycleEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCycleEnd.Location = new System.Drawing.Point(909, 7);
			this.lblCycleEnd.Name = "lblCycleEnd";
			this.lblCycleEnd.Size = new System.Drawing.Size(60, 25);
			this.lblCycleEnd.TabIndex = 1;
			this.lblCycleEnd.Text = "label1";
			// 
			// trackCycle
			// 
			this.trackCycle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.trackCycle.LargeChange = 1;
			this.trackCycle.Location = new System.Drawing.Point(103, 3);
			this.trackCycle.Name = "trackCycle";
			this.trackCycle.Size = new System.Drawing.Size(766, 34);
			this.trackCycle.TabIndex = 2;
			this.trackCycle.TickStyle = System.Windows.Forms.TickStyle.None;
			this.trackCycle.ValueChanged += new System.EventHandler(this.trackCycle_ValueChanged);
			// 
			// btnProjectOpen
			// 
			this.btnProjectOpen.Image = global::DFBUtility.Properties.Resources.FolderOpen_48x;
			this.btnProjectOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnProjectOpen.Name = "btnProjectOpen";
			this.btnProjectOpen.Size = new System.Drawing.Size(139, 44);
			this.btnProjectOpen.Text = "&Open Project";
			this.btnProjectOpen.Click += new System.EventHandler(this.btnProjectOpen_Click);
			// 
			// btnProjectNew
			// 
			this.btnProjectNew.Image = global::DFBUtility.Properties.Resources.VSO_Action_Add_12x_16x;
			this.btnProjectNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnProjectNew.Name = "btnProjectNew";
			this.btnProjectNew.Size = new System.Drawing.Size(133, 44);
			this.btnProjectNew.Text = "&New Project";
			this.btnProjectNew.ToolTipText = "New Project";
			this.btnProjectNew.Click += new System.EventHandler(this.btnProjectNew_Click);
			// 
			// btnProjectSave
			// 
			this.btnProjectSave.Image = global::DFBUtility.Properties.Resources.Save_32x;
			this.btnProjectSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnProjectSave.Name = "btnProjectSave";
			this.btnProjectSave.Size = new System.Drawing.Size(134, 44);
			this.btnProjectSave.Text = "&Save Project";
			this.btnProjectSave.ToolTipText = "Save Project and associated files";
			this.btnProjectSave.Click += new System.EventHandler(this.btnProjectSave_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 47);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(147, 44);
			this.toolStripLabel1.Text = "Nbr of Cycles to Run:";
			// 
			// tbNbrCycles
			// 
			this.tbNbrCycles.AcceptsReturn = true;
			this.tbNbrCycles.Name = "tbNbrCycles";
			this.tbNbrCycles.Size = new System.Drawing.Size(100, 47);
			this.tbNbrCycles.Text = "20";
			this.tbNbrCycles.Leave += new System.EventHandler(this.tbNbrCycles_Leave);
			this.tbNbrCycles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbNbrCycles_KeyDown);
			// 
			// btnStart
			// 
			this.btnStart.AutoSize = false;
			this.btnStart.Enabled = false;
			this.btnStart.Image = global::DFBUtility.Properties.Resources.StatusRun_32xLG;
			this.btnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(150, 44);
			this.btnStart.Text = "Start (F5)";
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnAbout
			// 
			this.btnAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.btnAbout.Image = global::DFBUtility.Properties.Resources.UIAboutBox_64x;
			this.btnAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnAbout.Name = "btnAbout";
			this.btnAbout.Size = new System.Drawing.Size(94, 44);
			this.btnAbout.Text = "About";
			this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
			// 
			// toolMain
			// 
			this.toolMain.ImageScalingSize = new System.Drawing.Size(40, 40);
			this.toolMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnProjectOpen,
            this.btnProjectNew,
            this.btnProjectSave,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.tbNbrCycles,
            this.btnStart,
            this.btnAbout,
            this.tsddPlugins});
			this.toolMain.Location = new System.Drawing.Point(0, 0);
			this.toolMain.Name = "toolMain";
			this.toolMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.toolMain.Size = new System.Drawing.Size(1582, 47);
			this.toolMain.TabIndex = 2;
			this.toolMain.Text = "toolStrip1";
			// 
			// tsddPlugins
			// 
			this.tsddPlugins.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsddPlugins.Image = ((System.Drawing.Image)(resources.GetObject("tsddPlugins.Image")));
			this.tsddPlugins.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsddPlugins.Name = "tsddPlugins";
			this.tsddPlugins.Size = new System.Drawing.Size(70, 44);
			this.tsddPlugins.Text = "Plugins";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1582, 973);
			this.Controls.Add(this.splitMain);
			this.Controls.Add(this.toolMain);
			this.Controls.Add(this.statusStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "Form1";
			this.Text = "Form1";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
			this.splitMain.ResumeLayout(false);
			this.splitCodeTab.Panel1.ResumeLayout(false);
			this.splitCodeTab.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitCodeTab)).EndInit();
			this.splitCodeTab.ResumeLayout(false);
			this.tabInfo.ResumeLayout(false);
			this.tpGlobals.ResumeLayout(false);
			this.tpGlobals.PerformLayout();
			this.panel3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvGlobalInputs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bsInputSequence)).EndInit();
			this.toolStrip11.ResumeLayout(false);
			this.toolStrip11.PerformLayout();
			this.tpBusIn1.ResumeLayout(false);
			this.tpBusIn1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctb_BusIn1)).EndInit();
			this.tollBusIn1.ResumeLayout(false);
			this.tollBusIn1.PerformLayout();
			this.tpBusIn2.ResumeLayout(false);
			this.tpBusIn2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctb_BusIn2)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.tpCode.ResumeLayout(false);
			this.tpCode.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctb_Code)).EndInit();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.tpValueConvert.ResumeLayout(false);
			this.tlp_ValueConversion.ResumeLayout(false);
			this.tlp_ValueConversion.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctbValConv_Hex)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fctbValConv_DFB)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fctbValConv_Int)).EndInit();
			this.toolStrip10.ResumeLayout(false);
			this.toolStrip10.PerformLayout();
			this.toolStrip9.ResumeLayout(false);
			this.toolStrip9.PerformLayout();
			this.toolStrip8.ResumeLayout(false);
			this.toolStrip8.PerformLayout();
			this.toolStrip7.ResumeLayout(false);
			this.toolStrip7.PerformLayout();
			this.tabLogAndRam.ResumeLayout(false);
			this.tpLog.ResumeLayout(false);
			this.tpRam.ResumeLayout(false);
			this.splitRams.Panel1.ResumeLayout(false);
			this.splitRams.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitRams)).EndInit();
			this.splitRams.ResumeLayout(false);
			this.splitRamACU.Panel1.ResumeLayout(false);
			this.splitRamACU.Panel1.PerformLayout();
			this.splitRamACU.Panel2.ResumeLayout(false);
			this.splitRamACU.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitRamACU)).EndInit();
			this.splitRamACU.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fctbRamAcuA)).EndInit();
			this.toolStrip3.ResumeLayout(false);
			this.toolStrip3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctbRamAcuB)).EndInit();
			this.toolStrip4.ResumeLayout(false);
			this.toolStrip4.PerformLayout();
			this.splitRamData.Panel1.ResumeLayout(false);
			this.splitRamData.Panel1.PerformLayout();
			this.splitRamData.Panel2.ResumeLayout(false);
			this.splitRamData.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitRamData)).EndInit();
			this.splitRamData.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fctbRamDataA)).EndInit();
			this.toolStrip5.ResumeLayout(false);
			this.toolStrip5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fctbRamDataB)).EndInit();
			this.toolStrip6.ResumeLayout(false);
			this.toolStrip6.PerformLayout();
			this.tpJumpCond.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fctb_JumpCond)).EndInit();
			this.tpCallDiag.ResumeLayout(false);
			this.tpBusOut1.ResumeLayout(false);
			this.tpBusOut1.PerformLayout();
			this.panel4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fctbBusOut1)).EndInit();
			this.tsBus1Out.ResumeLayout(false);
			this.tsBus1Out.PerformLayout();
			this.tpBusOut2.ResumeLayout(false);
			this.tpBusOut2.PerformLayout();
			this.panel5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fctbBusOut2)).EndInit();
			this.tsBus2Out.ResumeLayout(false);
			this.tsBus2Out.PerformLayout();
			this.pnlScrub.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackCycle)).EndInit();
			this.toolMain.ResumeLayout(false);
			this.toolMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.ToolStripProgressBar progressBottom;
		private System.Windows.Forms.ToolStripStatusLabel statlblBottom;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.SplitContainer splitCodeTab;
		private System.Windows.Forms.TabControl tabInfo;
		private System.Windows.Forms.TabPage tpBusIn1;
		private FastColoredTextBoxNS.FastColoredTextBox fctb_BusIn1;
		private System.Windows.Forms.ToolStrip tollBusIn1;
		private System.Windows.Forms.ToolStripButton btnBusIn1Insert;
		private System.Windows.Forms.TabPage tpBusIn2;
		private FastColoredTextBoxNS.FastColoredTextBox fctb_BusIn2;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btnBusIn2Insert;
		private System.Windows.Forms.TabPage tpCode;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripButton btnCodeInsert;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripLabel lblCurrentCodeFile;
		private System.Windows.Forms.TabControl tabLogAndRam;
		private System.Windows.Forms.TabPage tpLog;
		private System.Windows.Forms.TabPage tpRam;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.RichTextBox rtbMessages;
		private FastColoredTextBoxNS.FastColoredTextBox fctb_Code;
		private System.Windows.Forms.SplitContainer splitRams;
		private System.Windows.Forms.SplitContainer splitRamACU;
		private System.Windows.Forms.SplitContainer splitRamData;
		private System.Windows.Forms.Panel panel1;
		private FastColoredTextBoxNS.FastColoredTextBox fctbRamAcuA;
		private System.Windows.Forms.ToolStrip toolStrip3;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
		private System.Windows.Forms.ToolStrip toolStrip4;
		private System.Windows.Forms.ToolStripLabel toolStripLabel3;
		private FastColoredTextBoxNS.FastColoredTextBox fctbRamAcuB;
		private System.Windows.Forms.Panel panel2;
		private FastColoredTextBoxNS.FastColoredTextBox fctbRamDataA;
		private System.Windows.Forms.ToolStrip toolStrip5;
		private System.Windows.Forms.ToolStripLabel toolStripLabel4;
		private FastColoredTextBoxNS.FastColoredTextBox fctbRamDataB;
		private System.Windows.Forms.ToolStrip toolStrip6;
		private System.Windows.Forms.ToolStripLabel toolStripLabel5;
		private System.Windows.Forms.TabPage tpGlobals;
		private System.Windows.Forms.BindingSource bsInputSequence;
		private System.Windows.Forms.TabPage tpJumpCond;
		private FastColoredTextBoxNS.FastColoredTextBox fctb_JumpCond;
		private System.Windows.Forms.TabPage tpValueConvert;
		private System.Windows.Forms.TableLayoutPanel tlp_ValueConversion;
		private FastColoredTextBoxNS.FastColoredTextBox fctbValConv_Hex;
		private FastColoredTextBoxNS.FastColoredTextBox fctbValConv_DFB;
		private FastColoredTextBoxNS.FastColoredTextBox fctbValConv_Int;
		private System.Windows.Forms.ToolStrip toolStrip10;
		private System.Windows.Forms.ToolStripLabel toolStripLabel10;
		private System.Windows.Forms.ToolStrip toolStrip9;
		private System.Windows.Forms.ToolStripLabel toolStripLabel9;
		private System.Windows.Forms.ToolStrip toolStrip8;
		private System.Windows.Forms.ToolStripLabel toolStripLabel8;
		private System.Windows.Forms.ToolStrip toolStrip7;
		private System.Windows.Forms.ToolStripLabel toolStripLabel6;
		private System.Windows.Forms.ToolStripLabel toolStripLabel7;
		private System.Windows.Forms.ToolStripLabel toolStripLabel11;
		private System.Windows.Forms.ToolStripButton dataRamAPanelHide;
		private System.Windows.Forms.ToolStripButton dataRamBPanelHide;
		private System.Windows.Forms.ToolStripButton acuRamAPanelHide;
		private System.Windows.Forms.ToolStripButton acuRamBPanelHide;
		private System.Windows.Forms.ToolStrip toolStrip11;
		private System.Windows.Forms.ToolStripLabel toolStripLabel12;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.DataGridView dgvGlobalInputs;
		private System.Windows.Forms.TabPage tpBusOut1;
		private System.Windows.Forms.Panel panel4;
		private FastColoredTextBoxNS.FastColoredTextBox fctbBusOut1;
		private System.Windows.Forms.ToolStrip tsBus1Out;
		private System.Windows.Forms.ToolStripLabel toolStripLabel13;
		private System.Windows.Forms.TabPage tpBusOut2;
		private System.Windows.Forms.Panel panel5;
		private FastColoredTextBoxNS.FastColoredTextBox fctbBusOut2;
		private System.Windows.Forms.ToolStrip tsBus2Out;
		private System.Windows.Forms.ToolStripLabel toolStripLabel14;
		private System.Windows.Forms.ToolStripComboBox cbBusOut1Format;
		private System.Windows.Forms.ToolStripComboBox cbBusOut2Format;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.Panel pnlScrub;
		private System.Windows.Forms.WebBrowser webBrowser1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox tbCurrCycle;
		private System.Windows.Forms.Label lblCycleEnd;
		private System.Windows.Forms.ToolStripButton btnProjectOpen;
		private System.Windows.Forms.ToolStripButton btnProjectNew;
		private System.Windows.Forms.ToolStripButton btnProjectSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripTextBox tbNbrCycles;
		private System.Windows.Forms.ToolStripButton btnStart;
		private System.Windows.Forms.ToolStripButton btnAbout;
		private System.Windows.Forms.ToolStrip toolMain;
		private System.Windows.Forms.TrackBar trackCycle;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.TabPage tpCallDiag;
		private System.Windows.Forms.WebBrowser webBrowser2;
		private System.Windows.Forms.DataGridViewTextBoxColumn cycleDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn globalInput1DataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn globalInput2DataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn semaphore0DataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn semaphore1DataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn semaphore2DataGridViewCheckBoxColumn;
		private System.Windows.Forms.ToolStripDropDownButton tsddPlugins;
	}
}

