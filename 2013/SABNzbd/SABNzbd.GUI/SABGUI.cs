using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using RichardTestJson.Helpers;

namespace RichardTestJson
{
    public partial class SabGui : Form
    {
        private Timer _t;
        private bool _running;
        private ErrorLog _errorLog = null;


        public SabGui()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            EnableGoButton();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            EnableGoButton();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_running)
                {
                    _t = new Timer {Interval = int.Parse(tbSpeed.Text)*1000};
                    _t.Tick += _t_Tick;
                    _running = true;
                    bGo.Text = "Stop";

                    tbApi.Enabled = false;
                    tbHost.Enabled = false;
                    tbSpeed.Enabled = false;
                    bQueueState.Enabled = true;
                    
                    UpdateInfo();
                }
                else
                {
                    _t.Stop();
                    _running = false;
                    bGo.Text = "Connect";
                    lKbps.Text = "0 Kbps";

                    tbApi.Enabled = true;
                    tbHost.Enabled = true;
                    tbSpeed.Enabled = true;
                    bQueueState.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }

        void _t_Tick(object sender, EventArgs e)
        {
            UpdateInfo();
        }




        private XmlDocument GetSabNzbdInfo()
        {
            try
            {
                var url = String.Format("{0}/api?mode=qstatus&output=xml&apikey={1}", tbHost.Text, tbApi.Text);
                var wc = new WebClient();
                var xml = wc.DownloadString(url);

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
                return null;
            }
        }

        private static XmlDocument GetSabNzbdInfo(string url)
        {
            if (String.IsNullOrEmpty(url))
                return null;

            try
            {
                var wc = new WebClient();
                var xml = wc.DownloadString(url);

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
                return null;
            }
        }

        private static string GetUrl(string url)
        {
            try
            {
                var wc = new WebClient();
                return wc.DownloadString(url);
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
                return String.Empty;
            }
        }




        private void EnableGoButton()
        {
            try
            {
                if (!String.IsNullOrEmpty(tbHost.Text) && !String.IsNullOrEmpty(tbApi.Text))
                {
                    bGo.Enabled = true;
                    bRefreshJobs.Enabled = true;
                    bSetDlSpeed.Enabled = true;
                    bHistoryRefresh.Enabled = true;
                    bWarnings.Enabled = true;
                    bGetServers.Enabled = true;
                }
                else
                {
                    bGo.Enabled = false;
                    bRefreshJobs.Enabled = false;
                    bSetDlSpeed.Enabled = false;
                    bHistoryRefresh.Enabled = false;
                    bWarnings.Enabled = false;
                    bGetServers.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SabLogger.Instance.LogDebug("Application started");
            EnableGoButton();
        }

        private void UpdateInfo()
        {
            try
            {
                _t.Stop();
                var xmlDoc = GetSabNzbdInfo();

                var node = xmlDoc.SelectSingleNode("/queue/kbpersec");


                var value = Math.Floor(node.InnerText.AsDouble());


                lKbps.Text = String.Format("{0} Kbps", value);
                toolStripStatusLabel1.Text = String.Format("{0} Kbps", value);

                toolStripStatusLabel3.Text = node.GetNodeValString("/queue/mbleft");
                toolStripStatusLabel5.Text = node.GetNodeValString("/queue/mb");
                toolStripStatusLabel7.Text = node.GetNodeValString("/queue/timeleft");
                SetQueueState(node.SelectSingleNode("/queue/state"));

                _t.Start();
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }

        private void bRefreshJobs_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.DataSource = null;

                var xmlDoc = GetSabNzbdInfo();
                const string xpathJobs = "/queue/jobs/job";
                var jobs = xmlDoc.SelectNodes(xpathJobs);

                var lJobs = (from XmlNode sabJob in jobs select new SabJob(sabJob)).ToList();
                dataGridView1.DataSource = lJobs;
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }

        private void bHistoryRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.DataSource = null;

                var url = String.Format("{0}/api?mode=history&start=0&limit={2}&output=xml&apikey={1}", tbHost.Text,
                                        tbApi.Text, tbHistoryCount.Text);
                var xmlDoc = GetSabNzbdInfo(url);
                var history = xmlDoc.SelectNodes("/history/slots/slot");

                if (history == null)
                    return;

                var hList = (from XmlNode n in history select new SabHistory(n)).ToList();
                dataGridView2.DataSource = hList;
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }

        private void bSetDlSpeed_Click(object sender, EventArgs e)
        {
            try
            {
                var url = String.Format("{0}/api?mode=config&name=speedlimit&value={1}&apikey={2}", tbHost.Text,
                                        tbDlSpeed.Text, tbApi.Text);
                var response = GetUrl(url).ToLower().Trim();

                if (response == "ok")
                    MessageBox.Show(String.Format("Download speed set to: {0}", tbDlSpeed.Text), "Success");
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }

        private void bWarnings_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView3.DataSource = null;

                var url = String.Format("{0}/api?mode=warnings&output=xml&apikey={1}", tbHost.Text, tbApi.Text);
                var xmlDoc = GetSabNzbdInfo(url);
                if (xmlDoc == null) return;

                var nWarnings = xmlDoc.SelectNodes("/warnings/warning");
                var warnings = (from XmlNode w in nWarnings select new SabWarning(w)).ToList();
                if (warnings.Count == 0) return;

                dataGridView3.DataSource = warnings;
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }

        private void bGetServers_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView4.DataSource = null;
                var url = String.Format("{0}/api?mode=get_config&apikey={1}&output=xml", tbHost.Text, tbApi.Text);
                var xmlDoc = GetSabNzbdInfo(url);
                if (xmlDoc == null) return;

                var servers = xmlDoc.SelectNodes("/config/servers/server");
                if (servers == null) return;
                
                var items = (from XmlNode n in servers select new SabServer(n)).ToList();
                dataGridView4.DataSource = items;
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView3.DataSource = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView4.DataSource = null;
        }



        /* ******************************************************************************************
         * Queue States
         ****************************************************************************************** */
        private void SetQueueState(XmlNode n)
        {
            try
            {
                lQueueState.Text = n == null ? "Unknown" : n.InnerText;

                var tState = lQueueState.Text.ToLower().Trim();
                bQueueState.Text = (tState == "unknown" || tState == "paused") ? "Start" : "Stop";
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }

        private void bQueueState_Click(object sender, EventArgs e)
        {
            try
            {
                if (bQueueState.Text == "Start")
                {
                    var url = String.Format("{0}/api?mode=resume&apikey={1}", tbHost.Text, tbApi.Text);
                    var response = GetUrl(url);
                    if (response.ToLower().Trim() == "ok")
                    {
                        bQueueState.Text = "Stop";
                        return;
                    }
                    MessageBox.Show("Error starting queue!");
                }
                else
                {
                    var url = String.Format("{0}/api?mode=pause&apikey={1}", tbHost.Text, tbApi.Text);
                    var response = GetUrl(url);
                    if (response.ToLower().Trim() == "ok")
                    {
                        bQueueState.Text = "Start";
                        return;
                    }
                    MessageBox.Show("Error stopping queue!");
                }
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }






        private void bErrorLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (_errorLog == null)
                {
                    _errorLog = new ErrorLog();
                    _errorLog.Closed += _errorLog_Closed;
                }

                if (!_errorLog.Visible)
                    _errorLog.Show();
                
                _errorLog.Focus();
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }

        void _errorLog_Closed(object sender, EventArgs e)
        {
            try
            {
                _errorLog = null;
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }

        


    }
}
