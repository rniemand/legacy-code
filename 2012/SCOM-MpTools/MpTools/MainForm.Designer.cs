namespace MpTools
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.bMpSealer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bMpSealer
            // 
            this.bMpSealer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bMpSealer.Image = ((System.Drawing.Image)(resources.GetObject("bMpSealer.Image")));
            this.bMpSealer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bMpSealer.Location = new System.Drawing.Point(12, 12);
            this.bMpSealer.Name = "bMpSealer";
            this.bMpSealer.Size = new System.Drawing.Size(157, 65);
            this.bMpSealer.TabIndex = 0;
            this.bMpSealer.Text = "MP Sealer";
            this.bMpSealer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bMpSealer.UseVisualStyleBackColor = true;
            this.bMpSealer.Click += new System.EventHandler(this.ButtonMpSealerOnClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(177, 86);
            this.Controls.Add(this.bMpSealer);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bMpSealer;
    }
}

