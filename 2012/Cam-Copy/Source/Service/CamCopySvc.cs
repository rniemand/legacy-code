using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Threading;
using System.Xml;
using Rn.Core.Configuration;
using Rn.Core.Logging;

namespace CamCopy.Svc
{
    public partial class CamCopySvc : ServiceBase
    {
        private List<CamDumpFolder> _watchDirs;
        private System.Timers.Timer _timer;

        #region Service Required Methods
        public CamCopySvc()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            SvcStart();
        }

        protected override void OnStop()
        {
            SvcStop();
        } 
        #endregion


        public void SvcStart()
        {
            // Load the service configuration, create the loggers
            Logger.LogInfo("*** Service Starting ***");
            SetupService();
            Logger.LogInfo("*** Service Started ***");
        }

        private void SetupService()
        {
            // Load the config file
            Config.LoadXmlConfig("CamCopy");
            Config.GetXmlConfig().CreateLoggers();

            // Create lists that the service needs
            _watchDirs = new List<CamDumpFolder>();

            // Create startup timer
            _timer = new System.Timers.Timer { Interval = 500 };
            _timer.Elapsed += StartCopyThreads;
            _timer.Start();
        }

        void StartCopyThreads(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();

            // Get a list of all the target folders
            _watchDirs.Clear();
            const string xpath = "/CamCopy/WatchFolders/Folder[@Enabled='true']";
            var targets = Config.GetXmlConfig().SelectNodes(xpath);

            try
            {
                // Check that we have something
                if (targets.Count == 0)
                {
                    Logger.LogError("There are no folders to monitor...");
                    Thread.Sleep(2000);
                    Environment.Exit(-1);
                }

                // Create the folder watchers
                foreach (XmlNode n in targets)
                    _watchDirs.Add(new CamDumpFolder(n));

                Logger.LogInfo(String.Format("Successfully loaded '{0}' folder watchers", targets.Count));
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format("Exception while creating the configured folder watchers: {0}", ex.Message));
            }
        }

        public void SvcStop()
        {
            Logger.LogInfo("*** Service Stopped ***");
        }

    }
}
