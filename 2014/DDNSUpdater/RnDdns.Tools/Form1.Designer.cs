namespace RnDdns.Tools
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
            this.bPasswordHasher = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bPasswordHasher
            // 
            this.bPasswordHasher.Location = new System.Drawing.Point(12, 12);
            this.bPasswordHasher.Name = "bPasswordHasher";
            this.bPasswordHasher.Size = new System.Drawing.Size(260, 29);
            this.bPasswordHasher.TabIndex = 0;
            this.bPasswordHasher.Text = "Password Hasher";
            this.bPasswordHasher.UseVisualStyleBackColor = true;
            this.bPasswordHasher.Click += new System.EventHandler(this.bPasswordHasher_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.bPasswordHasher);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RnDdns Tools";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bPasswordHasher;
    }
}

