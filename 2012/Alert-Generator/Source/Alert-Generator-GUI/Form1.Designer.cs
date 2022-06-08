namespace Alert_Generator
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
            this.MakeAlert = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.evtID = new System.Windows.Forms.TextBox();
            this.evtSource = new System.Windows.Forms.TextBox();
            this.eventText = new System.Windows.Forms.RichTextBox();
            this.Quit = new System.Windows.Forms.Button();
            this.eventLog = new System.Windows.Forms.ComboBox();
            this.evtSeverity = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.messageArea = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MakeAlert
            // 
            this.MakeAlert.Location = new System.Drawing.Point(15, 192);
            this.MakeAlert.Name = "MakeAlert";
            this.MakeAlert.Size = new System.Drawing.Size(91, 23);
            this.MakeAlert.TabIndex = 0;
            this.MakeAlert.Text = "Create Alert";
            this.MakeAlert.UseVisualStyleBackColor = true;
            this.MakeAlert.Click += new System.EventHandler(this.MakeAlert_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Event ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Event Source:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Event Description:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Event Log:";
            // 
            // evtID
            // 
            this.evtID.Location = new System.Drawing.Point(112, 67);
            this.evtID.Name = "evtID";
            this.evtID.Size = new System.Drawing.Size(267, 20);
            this.evtID.TabIndex = 6;
            this.evtID.Text = "100";
            // 
            // evtSource
            // 
            this.evtSource.Location = new System.Drawing.Point(112, 93);
            this.evtSource.Name = "evtSource";
            this.evtSource.Size = new System.Drawing.Size(267, 20);
            this.evtSource.TabIndex = 7;
            this.evtSource.Text = "Richard Test";
            // 
            // eventText
            // 
            this.eventText.Location = new System.Drawing.Point(112, 119);
            this.eventText.Name = "eventText";
            this.eventText.Size = new System.Drawing.Size(267, 96);
            this.eventText.TabIndex = 8;
            this.eventText.Text = "The monitor DB was not found";
            // 
            // Quit
            // 
            this.Quit.Location = new System.Drawing.Point(15, 163);
            this.Quit.Name = "Quit";
            this.Quit.Size = new System.Drawing.Size(91, 23);
            this.Quit.TabIndex = 9;
            this.Quit.Text = "Quit";
            this.Quit.UseVisualStyleBackColor = true;
            this.Quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // eventLog
            // 
            this.eventLog.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.eventLog.FormattingEnabled = true;
            this.eventLog.Location = new System.Drawing.Point(112, 13);
            this.eventLog.Name = "eventLog";
            this.eventLog.Size = new System.Drawing.Size(267, 21);
            this.eventLog.TabIndex = 10;
            // 
            // evtSeverity
            // 
            this.evtSeverity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.evtSeverity.FormattingEnabled = true;
            this.evtSeverity.Location = new System.Drawing.Point(112, 40);
            this.evtSeverity.Name = "evtSeverity";
            this.evtSeverity.Size = new System.Drawing.Size(267, 21);
            this.evtSeverity.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Severity:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.messageArea});
            this.statusStrip1.Location = new System.Drawing.Point(0, 222);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(385, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // messageArea
            // 
            this.messageArea.Name = "messageArea";
            this.messageArea.Size = new System.Drawing.Size(118, 17);
            this.messageArea.Text = "toolStripStatusLabel1";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.TimerTick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 244);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.evtSeverity);
            this.Controls.Add(this.eventLog);
            this.Controls.Add(this.Quit);
            this.Controls.Add(this.eventText);
            this.Controls.Add(this.evtSource);
            this.Controls.Add(this.evtID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MakeAlert);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(401, 282);
            this.MinimumSize = new System.Drawing.Size(401, 282);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Event Creator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button MakeAlert;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox evtID;
        private System.Windows.Forms.TextBox evtSource;
        private System.Windows.Forms.RichTextBox eventText;
        private System.Windows.Forms.Button Quit;
        private System.Windows.Forms.ComboBox eventLog;
        private System.Windows.Forms.ComboBox evtSeverity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel messageArea;
        private System.Windows.Forms.Timer timer1;
    }
}

