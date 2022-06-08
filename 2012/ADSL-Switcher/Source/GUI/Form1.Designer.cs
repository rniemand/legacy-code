namespace AdslSwitcher2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxAccount = new System.Windows.Forms.ComboBox();
            this.textBoxIP1 = new System.Windows.Forms.TextBox();
            this.textBoxIP4 = new System.Windows.Forms.TextBox();
            this.textBoxIP3 = new System.Windows.Forms.TextBox();
            this.textBoxIP2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.textBoxUserPass = new System.Windows.Forms.TextBox();
            this.buttonAccountAdd = new System.Windows.Forms.Button();
            this.buttonAccountEdit = new System.Windows.Forms.Button();
            this.buttonAccountDelete = new System.Windows.Forms.Button();
            this.buttonConnectionTest = new System.Windows.Forms.Button();
            this.buttonApplyChange = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forgetDefaultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAccountAsDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Account:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Router IP:";
            // 
            // comboBoxAccount
            // 
            this.comboBoxAccount.FormattingEnabled = true;
            this.comboBoxAccount.Location = new System.Drawing.Point(86, 6);
            this.comboBoxAccount.Name = "comboBoxAccount";
            this.comboBoxAccount.Size = new System.Drawing.Size(121, 21);
            this.comboBoxAccount.TabIndex = 1;
            this.comboBoxAccount.SelectedIndexChanged += new System.EventHandler(this.comboBoxAccount_SelectedIndexChanged);
            // 
            // textBoxIP1
            // 
            this.textBoxIP1.Location = new System.Drawing.Point(86, 33);
            this.textBoxIP1.MaxLength = 4;
            this.textBoxIP1.Name = "textBoxIP1";
            this.textBoxIP1.Size = new System.Drawing.Size(30, 20);
            this.textBoxIP1.TabIndex = 2;
            this.textBoxIP1.TextChanged += new System.EventHandler(this.textBoxIP1_TextChanged);
            this.textBoxIP1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxIP1_KeyDown);
            // 
            // textBoxIP4
            // 
            this.textBoxIP4.Location = new System.Drawing.Point(194, 33);
            this.textBoxIP4.MaxLength = 4;
            this.textBoxIP4.Name = "textBoxIP4";
            this.textBoxIP4.Size = new System.Drawing.Size(30, 20);
            this.textBoxIP4.TabIndex = 5;
            this.textBoxIP4.TextChanged += new System.EventHandler(this.textBoxIP4_TextChanged);
            this.textBoxIP4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxIP4_KeyDown);
            // 
            // textBoxIP3
            // 
            this.textBoxIP3.Location = new System.Drawing.Point(158, 33);
            this.textBoxIP3.MaxLength = 4;
            this.textBoxIP3.Name = "textBoxIP3";
            this.textBoxIP3.Size = new System.Drawing.Size(30, 20);
            this.textBoxIP3.TabIndex = 4;
            this.textBoxIP3.TextChanged += new System.EventHandler(this.textBoxIP3_TextChanged);
            this.textBoxIP3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxIP3_KeyDown);
            // 
            // textBoxIP2
            // 
            this.textBoxIP2.Location = new System.Drawing.Point(122, 33);
            this.textBoxIP2.MaxLength = 4;
            this.textBoxIP2.Name = "textBoxIP2";
            this.textBoxIP2.Size = new System.Drawing.Size(30, 20);
            this.textBoxIP2.TabIndex = 3;
            this.textBoxIP2.TextChanged += new System.EventHandler(this.textBoxIP2_TextChanged);
            this.textBoxIP2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxIP2_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Router User:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Router Pass:";
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(86, 59);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(138, 20);
            this.textBoxUserName.TabIndex = 6;
            // 
            // textBoxUserPass
            // 
            this.textBoxUserPass.Location = new System.Drawing.Point(86, 85);
            this.textBoxUserPass.Name = "textBoxUserPass";
            this.textBoxUserPass.PasswordChar = '*';
            this.textBoxUserPass.Size = new System.Drawing.Size(138, 20);
            this.textBoxUserPass.TabIndex = 7;
            // 
            // buttonAccountAdd
            // 
            this.buttonAccountAdd.Image = ((System.Drawing.Image)(resources.GetObject("buttonAccountAdd.Image")));
            this.buttonAccountAdd.Location = new System.Drawing.Point(213, 6);
            this.buttonAccountAdd.Name = "buttonAccountAdd";
            this.buttonAccountAdd.Size = new System.Drawing.Size(27, 23);
            this.buttonAccountAdd.TabIndex = 10;
            this.buttonAccountAdd.UseVisualStyleBackColor = true;
            this.buttonAccountAdd.Click += new System.EventHandler(this.buttonAccountAdd_Click);
            // 
            // buttonAccountEdit
            // 
            this.buttonAccountEdit.Image = ((System.Drawing.Image)(resources.GetObject("buttonAccountEdit.Image")));
            this.buttonAccountEdit.Location = new System.Drawing.Point(246, 6);
            this.buttonAccountEdit.Name = "buttonAccountEdit";
            this.buttonAccountEdit.Size = new System.Drawing.Size(27, 23);
            this.buttonAccountEdit.TabIndex = 11;
            this.buttonAccountEdit.UseVisualStyleBackColor = true;
            this.buttonAccountEdit.Click += new System.EventHandler(this.buttonAccountEdit_Click);
            // 
            // buttonAccountDelete
            // 
            this.buttonAccountDelete.Image = ((System.Drawing.Image)(resources.GetObject("buttonAccountDelete.Image")));
            this.buttonAccountDelete.Location = new System.Drawing.Point(279, 6);
            this.buttonAccountDelete.Name = "buttonAccountDelete";
            this.buttonAccountDelete.Size = new System.Drawing.Size(27, 23);
            this.buttonAccountDelete.TabIndex = 12;
            this.buttonAccountDelete.UseVisualStyleBackColor = true;
            this.buttonAccountDelete.Click += new System.EventHandler(this.buttonAccountDelete_Click);
            // 
            // buttonConnectionTest
            // 
            this.buttonConnectionTest.Image = ((System.Drawing.Image)(resources.GetObject("buttonConnectionTest.Image")));
            this.buttonConnectionTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonConnectionTest.Location = new System.Drawing.Point(231, 31);
            this.buttonConnectionTest.Name = "buttonConnectionTest";
            this.buttonConnectionTest.Size = new System.Drawing.Size(75, 23);
            this.buttonConnectionTest.TabIndex = 9;
            this.buttonConnectionTest.Text = "Test";
            this.buttonConnectionTest.UseVisualStyleBackColor = true;
            this.buttonConnectionTest.Click += new System.EventHandler(this.buttonConnectionTest_Click);
            // 
            // buttonApplyChange
            // 
            this.buttonApplyChange.Image = ((System.Drawing.Image)(resources.GetObject("buttonApplyChange.Image")));
            this.buttonApplyChange.Location = new System.Drawing.Point(230, 57);
            this.buttonApplyChange.Name = "buttonApplyChange";
            this.buttonApplyChange.Size = new System.Drawing.Size(76, 48);
            this.buttonApplyChange.TabIndex = 8;
            this.buttonApplyChange.UseVisualStyleBackColor = true;
            this.buttonApplyChange.Click += new System.EventHandler(this.buttonApplyChange_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1,
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 113);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(313, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.setAccountAsDefaultToolStripMenuItem,
            this.forgetDefaultsToolStripMenuItem,
            this.configFileToolStripMenuItem});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 20);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            this.toolStripSplitButton1.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.toolStripSplitButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutToolStripMenuItem.Image")));
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // forgetDefaultsToolStripMenuItem
            // 
            this.forgetDefaultsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("forgetDefaultsToolStripMenuItem.Image")));
            this.forgetDefaultsToolStripMenuItem.Name = "forgetDefaultsToolStripMenuItem";
            this.forgetDefaultsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.forgetDefaultsToolStripMenuItem.Text = "Forget Defaults";
            this.forgetDefaultsToolStripMenuItem.Click += new System.EventHandler(this.forgetDefaultsToolStripMenuItem_Click);
            // 
            // setAccountAsDefaultToolStripMenuItem
            // 
            this.setAccountAsDefaultToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("setAccountAsDefaultToolStripMenuItem.Image")));
            this.setAccountAsDefaultToolStripMenuItem.Name = "setAccountAsDefaultToolStripMenuItem";
            this.setAccountAsDefaultToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.setAccountAsDefaultToolStripMenuItem.Text = "Set Account as Default";
            this.setAccountAsDefaultToolStripMenuItem.Click += new System.EventHandler(this.setAccountAsDefaultToolStripMenuItem_Click);
            // 
            // configFileToolStripMenuItem
            // 
            this.configFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dsToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.configFileToolStripMenuItem.Name = "configFileToolStripMenuItem";
            this.configFileToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.configFileToolStripMenuItem.Text = "Config File";
            // 
            // dsToolStripMenuItem
            // 
            this.dsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("dsToolStripMenuItem.Image")));
            this.dsToolStripMenuItem.Name = "dsToolStripMenuItem";
            this.dsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dsToolStripMenuItem.Text = "Import";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportToolStripMenuItem.Image")));
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 135);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonApplyChange);
            this.Controls.Add(this.buttonConnectionTest);
            this.Controls.Add(this.buttonAccountDelete);
            this.Controls.Add(this.buttonAccountEdit);
            this.Controls.Add(this.buttonAccountAdd);
            this.Controls.Add(this.textBoxUserPass);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxIP2);
            this.Controls.Add(this.textBoxIP3);
            this.Controls.Add(this.textBoxIP4);
            this.Controls.Add(this.textBoxIP1);
            this.Controls.Add(this.comboBoxAccount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(329, 173);
            this.MinimumSize = new System.Drawing.Size(329, 173);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ADSL Switcher";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxAccount;
        private System.Windows.Forms.TextBox textBoxIP1;
        private System.Windows.Forms.TextBox textBoxIP4;
        private System.Windows.Forms.TextBox textBoxIP3;
        private System.Windows.Forms.TextBox textBoxIP2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.TextBox textBoxUserPass;
        private System.Windows.Forms.Button buttonAccountAdd;
        private System.Windows.Forms.Button buttonAccountEdit;
        private System.Windows.Forms.Button buttonAccountDelete;
        private System.Windows.Forms.Button buttonConnectionTest;
        private System.Windows.Forms.Button buttonApplyChange;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAccountAsDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forgetDefaultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem configFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
    }
}

