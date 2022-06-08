using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FileLockerGui.Properties;

namespace FileLockerGui
{
    public partial class FileLocker : Form
    {
        private string _filePath;
        private FileStream _fs;
        private bool _hasFile;
        private bool _fileLocked;
        private About _about;

        public FileLocker()
        {
            InitializeComponent();
        }


        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
            if (files.Length < 1) return;
            
            _hasFile = true;
            var newFile = files[0];
            if (newFile != _filePath) UnlockFile();
            _filePath = files[0];

            LockFile();
        }

        private void BReleaseFileClick(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void Button2Click(object sender, EventArgs e)
        {
            if (_about == null || _about.IsDisposed)
                _about = new About();

            if (!_about.Visible)
                _about.Show();

            _about.Focus();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PictureBox1Click(object sender, EventArgs e)
        {
            if (_hasFile && _fileLocked)
            {
                UnlockFile();
                return;
            }

            if (_hasFile && !_fileLocked)
            {
                LockFile();
                return;
            }
        }


        private void LockFile()
        {
            if (String.IsNullOrEmpty(_filePath))
            {
                MessageBox.Show(Resources.NoPathGiven, Resources.NoPathGivenTitle);
                return;
            }

            if (!File.Exists(_filePath))
            {
                MessageBox.Show(String.Format(Resources.GivenPathNotFound, _filePath), Resources.GivenPathNotFoundTitle);
                ResetForm();
                return;
            }

            try
            {
                if (!pictureBox1.Enabled) pictureBox1.Enabled = true;
                _fs = new FileStream(_filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                pictureBox1.Image = Resources.locked;

                lStatus.Enabled = true;
                lStatus.Text = "Locked";
                richTextBox1.Text = _filePath;
                _fileLocked = true;
                bReleaseFile.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format(Resources.ErrorLockingFile, _filePath, ex.Message),
                                Resources.ErrorLockingFileTitle);
            }
        }

        private void UnlockFile()
        {
            if (!_fileLocked) return;

            _fileLocked = false;
            pictureBox1.Image = Resources.open;
            lStatus.Text = "Unlocked";
            bReleaseFile.Enabled = true;

            _fs.Close();
        }

        private void ResetForm()
        {
            if (_hasFile && _fileLocked) UnlockFile();

            lStatus.Text = "No File...";
            lStatus.Enabled = false;
            richTextBox1.Text = "";
            _filePath = "";
            _fileLocked = false;
            _hasFile = false;
            pictureBox1.Image = Resources.open_grey;
            pictureBox1.Enabled = false;
            bReleaseFile.Enabled = false;
        }
    }
}
