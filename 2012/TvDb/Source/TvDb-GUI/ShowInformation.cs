using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Rn.Core.DB;
using Rn.Core.Logging;
using Rn.TvDb;

namespace TvDvGui
{
    public partial class ShowInformation : Form
    {
        public string ShowName { get; private set; }
        public int StartingSeason { get; private set; }

        private List<TvdbSearchResult> _results;

        public ShowInformation(string showName, int season = 1)
        {
            InitializeComponent();

            ShowName = showName;
            StartingSeason = season;

            Shown += ShowInformationShown;
        }


        void ShowInformationShown(object sender, EventArgs e)
        {
            ssl1.Text = "";
            LoadShowInformation();
        }

        private void SetStatusText(string msg)
        {
            ssl1.Text = msg;
        }


        private void LoadShowInformation()
        {
            var bgw = new BackgroundWorker();
            bgw.DoWork += BgwLoadShowInfoStart;
            bgw.RunWorkerCompleted += BgwLoadShowInfoCompleted;
            bgw.WorkerReportsProgress = false;
            bgw.RunWorkerAsync();
        }

        void BgwLoadShowInfoStart(object sender, DoWorkEventArgs e)
        {
            cbShows.Enabled = false;
            SetStatusText("Looking up show information...");
            _results = TvDbGlobals.TVDB.RunSearch(ShowName);
        }

        void BgwLoadShowInfoCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_results.Count == 0)
            {
                SetStatusText("No show found matching criteria!");
                var msg = String.Format("There were no shows found with the name {0}", ShowName);
                MessageBox.Show(msg, "Show not found!");
                Close();
            }

            SetStatusText(String.Format("Found {0} show(s)", _results.Count));
            cbShows.DataSource = _results;
            cbShows.DisplayMember = "SeriesName";
            cbShows.SelectedIndex = 0;
            cbShows.Enabled = true;  
        }

        private void CbShowsSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_results.Count == 0) return;

            TvDbGlobals.TVDB.DownloadShowInfo(_results[cbShows.SelectedIndex]);
            var si = TvDbGlobals.TVDB.GetShowInfo(_results[cbShows.SelectedIndex]);


            DbSp.GetSp("pr_SeriesAdd", DbConType.MySql);

            /*
            var insert = "INSERT INTO tb_shows (showId, showName) VALUES ({0}, '{1}')";
            insert = String.Format(insert, si.Id, si.SeriesName);
            
            using(var con = MySQLFactory.GetNewConnection())
            {
                using(var cmd = new MySqlCommand(insert, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            */


        }

        




    }
}
