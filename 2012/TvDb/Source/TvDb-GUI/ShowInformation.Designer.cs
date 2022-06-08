namespace TvDvGui
{
    partial class ShowInformation
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
            this.cbShows = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ssl1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbShows
            // 
            this.cbShows.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShows.Enabled = false;
            this.cbShows.FormattingEnabled = true;
            this.cbShows.Location = new System.Drawing.Point(12, 12);
            this.cbShows.Name = "cbShows";
            this.cbShows.Size = new System.Drawing.Size(406, 21);
            this.cbShows.TabIndex = 0;
            this.cbShows.SelectedIndexChanged += new System.EventHandler(this.CbShowsSelectedIndexChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssl1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 332);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(430, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ssl1
            // 
            this.ssl1.Name = "ssl1";
            this.ssl1.Size = new System.Drawing.Size(118, 17);
            this.ssl1.Text = "toolStripStatusLabel1";
            // 
            // ShowInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 354);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cbShows);
            this.Name = "ShowInformation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShowInformation";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbShows;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ssl1;
    }
}