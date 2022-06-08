using System;
using System.IO;
using System.Windows.Forms;
using Rn.Core.Configuration;
using Rn.Core.Helpers;
using Rn.TvDb;

namespace TvDvGui
{
    public partial class SeriesManager : Form
    {
        public SeriesManager()
        {
            InitializeComponent();
            Shown += LayoutTestShown;
        }


        void LayoutTestShown(object sender, EventArgs e)
        {
            if(DesignMode) return;

            SetupEnv();
            SetNewFolder(tbCurrentPath.Text);
        }

        private void SetupEnv()
        {
            Config.LoadXmlConfig("RnConfig");
            Config.GetXmlConfig().CreateLoggers();

            TvDbGlobals.TVDB = new TVDB(Config.GetXmlConfig());
            TvDbGlobals.TVDB.RefreshMirrors();
        }



        private void RedrawTree(string path)
        {
            treeView1.Nodes.Clear();

            treeView1.Nodes.Add(new TreeNode
            {
                Name = path,
                Text = "Root Node"
            });

            var di = new DirectoryInfo(tbCurrentPath.Text);
            var folders = di.GetDirectories();
            var rootNode = treeView1.Nodes[0];

            if (folders.Length > 0)
            {
                foreach (DirectoryInfo folder in folders)
                {
                    rootNode.Nodes.Add(new TreeNode
                    {
                        Name = folder.FullName,
                        Text = folder.Name
                    });
                }
            }

            rootNode.Expand();
        }

        private void TreeView1AfterSelect(object sender, TreeViewEventArgs e)
        {
            var di = new DirectoryInfo(e.Node.Name);
            var folders = di.GetDirectories();
            e.Node.Nodes.Clear();

            if (folders.Length > 0)
            {
                foreach (DirectoryInfo dir in folders)
                {
                    e.Node.Nodes.Add(new TreeNode
                    {
                        Name = dir.FullName,
                        Text = dir.Name
                    });
                }
            }

            ShowFiles(e.Node.Name);
            e.Node.Expand();
        }

        private void ShowFiles(string path)
        {
            var di = new DirectoryInfo(path);
            var files = di.GetFiles();

            dataGridView1.DataSource = files;

        }

        private void BChooseFolderClick(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = tbCurrentPath.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                SetNewFolder(folderBrowserDialog1.SelectedPath);
            }
        }

        private void SetNewFolder(string path)
        {
            tbCurrentPath.Text = path;
            RedrawTree(path);
            ShowFiles(path);
        }

        private void BSeriesInfoClick(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            var lookFor = tbCurrentPath.Text.ToLower();
            if (lookFor.Substring(lookFor.Length - 1, 1) != @"\")
                lookFor = lookFor + @"\";

            int season = 1;
            var showName = treeView1.SelectedNode.Name.ToLower().Replace(lookFor, "");
            if (showName.Contains(@"\")) showName = showName.Split('\\')[0];

            const string rxp = @".*?season (\d+)";
            if (treeView1.SelectedNode.Name.RxIsMatch(rxp))
                season = treeView1.SelectedNode.Name.RxGetMatch(rxp, 1, "1").AsInt(1);

            var si = new ShowInformation(showName, season);
            si.Show();
        }

    }
}
