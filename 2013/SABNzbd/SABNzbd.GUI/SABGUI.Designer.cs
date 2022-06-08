namespace RichardTestJson
{
    partial class SabGui
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
            this.tbHost = new System.Windows.Forms.TextBox();
            this.tbApi = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bGo = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbSpeed = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.bSetDlSpeed = new System.Windows.Forms.Button();
            this.tbDlSpeed = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lKbps = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.bRefreshJobs = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cmJobs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePriorityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.tbHistoryCount = new System.Windows.Forms.TextBox();
            this.bHistoryRefresh = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.cmHistory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.retryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.bWarnings = new System.Windows.Forms.Button();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.button4 = new System.Windows.Forms.Button();
            this.bGetServers = new System.Windows.Forms.Button();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.cmServers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.bErrorLog = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.bQueueState = new System.Windows.Forms.Button();
            this.lQueueState = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.cmJobs.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.cmHistory.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            this.cmServers.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbHost
            // 
            this.tbHost.Location = new System.Drawing.Point(59, 12);
            this.tbHost.Name = "tbHost";
            this.tbHost.Size = new System.Drawing.Size(520, 20);
            this.tbHost.TabIndex = 0;
            this.tbHost.Text = "";
            this.tbHost.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // tbApi
            // 
            this.tbApi.Location = new System.Drawing.Point(59, 38);
            this.tbApi.Name = "tbApi";
            this.tbApi.Size = new System.Drawing.Size(520, 20);
            this.tbApi.TabIndex = 1;
            this.tbApi.Text = "";
            this.tbApi.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "URL:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "API:";
            // 
            // bGo
            // 
            this.bGo.Enabled = false;
            this.bGo.Location = new System.Drawing.Point(585, 10);
            this.bGo.Name = "bGo";
            this.bGo.Size = new System.Drawing.Size(89, 48);
            this.bGo.TabIndex = 4;
            this.bGo.Text = "Connect";
            this.bGo.UseVisualStyleBackColor = true;
            this.bGo.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Speed:";
            // 
            // tbSpeed
            // 
            this.tbSpeed.Location = new System.Drawing.Point(59, 64);
            this.tbSpeed.Name = "tbSpeed";
            this.tbSpeed.Size = new System.Drawing.Size(53, 20);
            this.tbSpeed.TabIndex = 7;
            this.tbSpeed.Text = "5";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(118, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Second(s)";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(12, 90);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(666, 312);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lQueueState);
            this.tabPage1.Controls.Add(this.bQueueState);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.bSetDlSpeed);
            this.tabPage1.Controls.Add(this.tbDlSpeed);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.lKbps);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(658, 286);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic Info";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // bSetDlSpeed
            // 
            this.bSetDlSpeed.Enabled = false;
            this.bSetDlSpeed.Location = new System.Drawing.Point(139, 4);
            this.bSetDlSpeed.Name = "bSetDlSpeed";
            this.bSetDlSpeed.Size = new System.Drawing.Size(47, 23);
            this.bSetDlSpeed.TabIndex = 9;
            this.bSetDlSpeed.Text = "Set";
            this.bSetDlSpeed.UseVisualStyleBackColor = true;
            this.bSetDlSpeed.Click += new System.EventHandler(this.bSetDlSpeed_Click);
            // 
            // tbDlSpeed
            // 
            this.tbDlSpeed.Location = new System.Drawing.Point(66, 6);
            this.tbDlSpeed.Name = "tbDlSpeed";
            this.tbDlSpeed.Size = new System.Drawing.Size(67, 20);
            this.tbDlSpeed.TabIndex = 8;
            this.tbDlSpeed.Text = "500";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Dl Speed:";
            // 
            // lKbps
            // 
            this.lKbps.AutoSize = true;
            this.lKbps.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lKbps.Location = new System.Drawing.Point(443, 241);
            this.lKbps.Name = "lKbps";
            this.lKbps.Size = new System.Drawing.Size(141, 42);
            this.lKbps.TabIndex = 6;
            this.lKbps.Text = "0 Kbps";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.bRefreshJobs);
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(658, 286);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Jobs";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // bRefreshJobs
            // 
            this.bRefreshJobs.Location = new System.Drawing.Point(575, 6);
            this.bRefreshJobs.Name = "bRefreshJobs";
            this.bRefreshJobs.Size = new System.Drawing.Size(75, 23);
            this.bRefreshJobs.TabIndex = 1;
            this.bRefreshJobs.Text = "Refresh";
            this.bRefreshJobs.UseVisualStyleBackColor = true;
            this.bRefreshJobs.Click += new System.EventHandler(this.bRefreshJobs_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.cmJobs;
            this.dataGridView1.Location = new System.Drawing.Point(6, 36);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(644, 244);
            this.dataGridView1.TabIndex = 0;
            // 
            // cmJobs
            // 
            this.cmJobs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem,
            this.pauseToolStripMenuItem,
            this.resumeToolStripMenuItem,
            this.changeScriptToolStripMenuItem,
            this.changeCategoryToolStripMenuItem,
            this.changePriorityToolStripMenuItem});
            this.cmJobs.Name = "cmJobs";
            this.cmJobs.Size = new System.Drawing.Size(167, 136);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.pauseToolStripMenuItem.Text = "Pause";
            // 
            // resumeToolStripMenuItem
            // 
            this.resumeToolStripMenuItem.Name = "resumeToolStripMenuItem";
            this.resumeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.resumeToolStripMenuItem.Text = "Resume";
            // 
            // changeScriptToolStripMenuItem
            // 
            this.changeScriptToolStripMenuItem.Name = "changeScriptToolStripMenuItem";
            this.changeScriptToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.changeScriptToolStripMenuItem.Text = "Change Script";
            // 
            // changeCategoryToolStripMenuItem
            // 
            this.changeCategoryToolStripMenuItem.Name = "changeCategoryToolStripMenuItem";
            this.changeCategoryToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.changeCategoryToolStripMenuItem.Text = "Change Category";
            // 
            // changePriorityToolStripMenuItem
            // 
            this.changePriorityToolStripMenuItem.Name = "changePriorityToolStripMenuItem";
            this.changePriorityToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.changePriorityToolStripMenuItem.Text = "Change Priority";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.button2);
            this.tabPage3.Controls.Add(this.tbHistoryCount);
            this.tabPage3.Controls.Add(this.bHistoryRefresh);
            this.tabPage3.Controls.Add(this.dataGridView2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(658, 286);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "History";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tbHistoryCount
            // 
            this.tbHistoryCount.Location = new System.Drawing.Point(519, 8);
            this.tbHistoryCount.Name = "tbHistoryCount";
            this.tbHistoryCount.Size = new System.Drawing.Size(50, 20);
            this.tbHistoryCount.TabIndex = 6;
            this.tbHistoryCount.Text = "10";
            // 
            // bHistoryRefresh
            // 
            this.bHistoryRefresh.Enabled = false;
            this.bHistoryRefresh.Location = new System.Drawing.Point(575, 6);
            this.bHistoryRefresh.Name = "bHistoryRefresh";
            this.bHistoryRefresh.Size = new System.Drawing.Size(75, 23);
            this.bHistoryRefresh.TabIndex = 2;
            this.bHistoryRefresh.Text = "Refresh";
            this.bHistoryRefresh.UseVisualStyleBackColor = true;
            this.bHistoryRefresh.Click += new System.EventHandler(this.bHistoryRefresh_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.ContextMenuStrip = this.cmHistory;
            this.dataGridView2.Location = new System.Drawing.Point(6, 36);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(644, 244);
            this.dataGridView2.TabIndex = 1;
            // 
            // cmHistory
            // 
            this.cmHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem1,
            this.retryToolStripMenuItem});
            this.cmHistory.Name = "cmHistory";
            this.cmHistory.Size = new System.Drawing.Size(108, 48);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            // 
            // retryToolStripMenuItem
            // 
            this.retryToolStripMenuItem.Name = "retryToolStripMenuItem";
            this.retryToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.retryToolStripMenuItem.Text = "Retry";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button3);
            this.tabPage4.Controls.Add(this.bWarnings);
            this.tabPage4.Controls.Add(this.dataGridView3);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(658, 286);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Warnings";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Clear";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // bWarnings
            // 
            this.bWarnings.Enabled = false;
            this.bWarnings.Location = new System.Drawing.Point(575, 6);
            this.bWarnings.Name = "bWarnings";
            this.bWarnings.Size = new System.Drawing.Size(75, 23);
            this.bWarnings.TabIndex = 4;
            this.bWarnings.Text = "Refresh";
            this.bWarnings.UseVisualStyleBackColor = true;
            this.bWarnings.Click += new System.EventHandler(this.bWarnings_Click);
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(6, 36);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView3.Size = new System.Drawing.Size(644, 244);
            this.dataGridView3.TabIndex = 3;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.button4);
            this.tabPage5.Controls.Add(this.bGetServers);
            this.tabPage5.Controls.Add(this.dataGridView4);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(658, 286);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Servers";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(6, 6);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 9;
            this.button4.Text = "Clear";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // bGetServers
            // 
            this.bGetServers.Enabled = false;
            this.bGetServers.Location = new System.Drawing.Point(575, 6);
            this.bGetServers.Name = "bGetServers";
            this.bGetServers.Size = new System.Drawing.Size(75, 23);
            this.bGetServers.TabIndex = 6;
            this.bGetServers.Text = "Refresh";
            this.bGetServers.UseVisualStyleBackColor = true;
            this.bGetServers.Click += new System.EventHandler(this.bGetServers_Click);
            // 
            // dataGridView4
            // 
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.ContextMenuStrip = this.cmServers;
            this.dataGridView4.Location = new System.Drawing.Point(6, 36);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView4.Size = new System.Drawing.Size(644, 244);
            this.dataGridView4.TabIndex = 5;
            // 
            // cmServers
            // 
            this.cmServers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem2});
            this.cmServers.Name = "cmServers";
            this.cmServers.Size = new System.Drawing.Size(108, 70);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // deleteToolStripMenuItem2
            // 
            this.deleteToolStripMenuItem2.Name = "deleteToolStripMenuItem2";
            this.deleteToolStripMenuItem2.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem2.Text = "Delete";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel7});
            this.statusStrip1.Location = new System.Drawing.Point(0, 405);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(678, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(42, 17);
            this.toolStripStatusLabel1.Text = "0 Kbps";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(34, 17);
            this.toolStripStatusLabel3.Text = "0 Mb";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(18, 17);
            this.toolStripStatusLabel4.Text = "of";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(34, 17);
            this.toolStripStatusLabel5.Text = "0 Mb";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel6.Text = "|";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(28, 17);
            this.toolStripStatusLabel7.Text = "0:00";
            // 
            // bErrorLog
            // 
            this.bErrorLog.Location = new System.Drawing.Point(585, 62);
            this.bErrorLog.Name = "bErrorLog";
            this.bErrorLog.Size = new System.Drawing.Size(89, 23);
            this.bErrorLog.TabIndex = 11;
            this.bErrorLog.Text = "View Error Log";
            this.bErrorLog.UseVisualStyleBackColor = true;
            this.bErrorLog.Click += new System.EventHandler(this.bErrorLog_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Q State:";
            // 
            // bQueueState
            // 
            this.bQueueState.Enabled = false;
            this.bQueueState.Location = new System.Drawing.Point(139, 33);
            this.bQueueState.Name = "bQueueState";
            this.bQueueState.Size = new System.Drawing.Size(47, 23);
            this.bQueueState.TabIndex = 11;
            this.bQueueState.Text = "...";
            this.bQueueState.UseVisualStyleBackColor = true;
            this.bQueueState.Click += new System.EventHandler(this.bQueueState_Click);
            // 
            // lQueueState
            // 
            this.lQueueState.AutoSize = true;
            this.lQueueState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lQueueState.Location = new System.Drawing.Point(63, 38);
            this.lQueueState.Name = "lQueueState";
            this.lQueueState.Size = new System.Drawing.Size(60, 13);
            this.lQueueState.TabIndex = 12;
            this.lQueueState.Text = "Unknown";
            // 
            // SabGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 427);
            this.Controls.Add(this.bErrorLog);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbSpeed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bGo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbApi);
            this.Controls.Add(this.tbHost);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SabGui";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SABNZBD Test :P";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.cmJobs.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.cmHistory.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            this.cmServers.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbHost;
        private System.Windows.Forms.TextBox tbApi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bGo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbSpeed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lKbps;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.Button bRefreshJobs;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button bHistoryRefresh;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button bSetDlSpeed;
        private System.Windows.Forms.TextBox tbDlSpeed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button bWarnings;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.TextBox tbHistoryCount;
        private System.Windows.Forms.ContextMenuStrip cmJobs;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resumeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeCategoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePriorityToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmHistory;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem retryToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button bGetServers;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.ContextMenuStrip cmServers;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button bErrorLog;
        private System.Windows.Forms.Label lQueueState;
        private System.Windows.Forms.Button bQueueState;
        private System.Windows.Forms.Label label6;
    }
}

