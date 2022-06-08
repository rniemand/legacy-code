using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MpTools.Classes;
using MpTools.Tools.Sealer;

namespace MpTools
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Shown += MainFormShown;
        }

        private void LoadConfigFile()
        {
            try
            {
                const string filePath = "./MainConfig.xml";

                if (!File.Exists(filePath))
                {
                    MessageBox.Show(String.Format(
                        "Could not find the main configuration file '{0}', the application will now exit.",
                        filePath));
                    Close();
                }

                GlobalObj.LoadMainConfigFile(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format(
                    "Something went wrong launching the application: {0}",
                    ex.Message));
                Close();
            }
            
        }


        void MainFormShown(object sender, EventArgs e)
        {
            if (DesignMode) return;

            LoadConfigFile();

            var mps = new MpSealer();
            mps.Show();
        }

        private void ButtonMpSealerOnClick(object sender, EventArgs e)
        {
            var mps = new MpSealer();
            mps.Show();
        }
    }
}
