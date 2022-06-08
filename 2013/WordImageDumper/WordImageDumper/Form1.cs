using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ionic.Zip;

namespace WordImageDumper
{
    public partial class Form1 : Form
    {
        private string _baseDir;
        private string _mediaExt;

        public Form1()
        {
            InitializeComponent();
            Shown += Form1_Shown;
        }

        void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                _baseDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\".Replace(@"\\", @"\");
                _baseDir += @"Extracted\";
                _mediaExt = ".png|.jpg|.bmp|.gif";
            }
            catch (Exception ex)
            {
                // todo: log
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var files = (string[])
                    e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                    HandleFileProcessing(file);
            }
            catch (Exception ex)
            {
                // todo: log this
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }


        private void HandleFileProcessing(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    // todo: log this
                    MessageBox.Show("Not a valid file");
                    return;
                }

                if (!IsAllowedExtension(filePath))
                {
                    // todo: handle this
                    return;
                }

                ProcessFileDocX(filePath);
            }
            catch (Exception ex)
            {
                // todo: log this
                Log(ex.Message);
            }
        }

        private bool IsAllowedExtension(string filePath)
        {
            try
            {
                var extension = Path.GetExtension(filePath);
                if (extension != null)
                {
                    var ext = extension.Replace(".", "").ToLower().Trim();
                    return ext == "docx";
                }
            }
            catch (Exception ex)
            {
                // todo: log this
                MessageBox.Show(ex.Message);
            }

            return false;
        }

        private void ProcessFileDocX(string filePath)
        {
            try
            {
                var fileName = Path.GetFileName(filePath);

                if (fileName == null)
                {
                    // todo: log this
                    return;
                }

                var dirBaseName = fileName.ToLower().Trim().Replace(".docx", "");
                var dirName = String.Format("{0}{1}", _baseDir, dirBaseName);

                using (var zip = new ZipFile(filePath))
                {
                    var media =
                        (from e in zip.Entries
                         let extension = Path.GetExtension(e.FileName)
                         where e.FileName.ToLower().StartsWith("word/media/")
                         where extension != null && _mediaExt.Split('|').Contains(extension.ToLower())
                         select e).ToList();

                    if (media.Count > 0) CheckBaseDir(dirName);
                    foreach (ZipEntry e in media)
                    {
                        var tgtFileName = String.Format(@"{0}\{1}", dirName, e.FileName.Replace("word/media/", ""));
                        ExtractFile(e, tgtFileName, true);
                    }
                }
            }
            catch (Exception ex)
            {
                // todo: log
                Log(ex.Message);
            }
        }

        private void ExtractFile(ZipEntry e, string filePath, bool replace = false)
        {
            try
            {
                if (e == null || e.FileName.Length == 0)
                    return;
                if (File.Exists(filePath) && !replace)
                    return;
                if (File.Exists(filePath)) File.Delete(filePath);
                if (File.Exists(filePath))
                {
                    // todo: log
                    Log("FILE CANNOT BE DELETED");
                    return;
                }

                using (var fs = new FileStream(filePath, FileMode.CreateNew))
                {
                    e.Extract(fs);
                }
            }
            catch (Exception ex)
            {
                // todo: log
                Log(ex.Message);
            }
        }

        private bool CheckBaseDir(string dirName)
        {
            try
            {
                // todo: make better
                if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);
                if (!Directory.Exists(dirName))
                {
                    // todo: log
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // todo: log
                Log(ex.Message);
            }

            return false;
        }


        private void Log(string msg)
        {
            richTextBox1.Text += msg;
            richTextBox1.Text += Environment.NewLine;
        }

    }
}
