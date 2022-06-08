using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Timers;
using RnDdns.Common.Helpers;
using RnDdns.Common.Models;

namespace RnDdns.Service
{
    public partial class DdnsUpdateService : ServiceBase
    {
        #region Crud
        public DdnsUpdateService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartService();
        }

        protected override void OnStop()
        {
            StopService();
        } 
        #endregion

        private List<UserDomain> _domains;
        private Timer _updateTimer;
        private bool _firstRun;

        public void StartService()
        {
            RnLogger.Instance.Debug("RnDdns service starting.");
            
            CreateCoreObjects();
            LoadConfiguredSites();
            StartUpdateTimerLoop();
        }

        public void StopService()
        {
            Console.WriteLine("bye bye");
        }


        // helper methods
        private void LoadConfiguredSites()
        {
            RnLogger.Instance.Debug("Loading configured sites...");

            var domains = RnConfig.Instance.CurrentConfig.Domains;

            foreach (var domain in domains)
            {
                // skip any disabled domains...
                if (domain.Enabled == false)
                {
                    RnLogger.Instance.Info(string.Format(
                        "Domain '{0}' (id: {1}) disabled in config, skipping",
                        domain.Hostname, domain.Id));
                    continue;
                }

                // todo: switch on domain type...
                _domains.Add(new UserDomain
                {
                    Creds = new UserCredentials
                    {
                        Username = domain.Credentials.Username,
                        Password = domain.Credentials.Password
                    },
                    HostName = domain.Hostname,
                    ServerUrl = domain.ServerUrl
                });

                RnLogger.Instance.Info(string.Format("Loaded domain :: {0}", domain.Hostname));
            }

            //var creds = new UserCredentials
            //{
            //    Password = "",
            //    Username = ""
            //};

            //_domains.Add(new UserDomain
            //{
            //    Creds = creds,
            //    HostName = ".kguard.org",
            //    ServerUrl = "http://www.kguard.org"
            //});
            //RnLogger.Instance.Info("Loaded domain :: rncam1.kguard.org");

            //_domains.Add(new UserDomain
            //{
            //    Creds = creds,
            //    HostName = ".kguard.org",
            //    ServerUrl = "http://www.kguard.org"
            //});
            //RnLogger.Instance.Info("Loaded domain :: rncam2.kguard.org");

            //_domains.Add(new UserDomain
            //{
            //    Creds = creds,
            //    HostName = ".kguard.org",
            //    ServerUrl = "http://www.kguard.org"
            //});
            //RnLogger.Instance.Info("Loaded domain :: rniemand.kguard.org");

            //_domains.Add(new UserDomain
            //{
            //    Creds = creds,
            //    HostName = ".kguard.org",
            //    ServerUrl = "http://www.kguard.org"
            //});
            //RnLogger.Instance.Info("Loaded domain :: rniemand.kguard.org");
        }

        private void CreateCoreObjects()
        {
            RnLogger.Instance.Debug("Creating core objects");

            _domains = new List<UserDomain>();
            _firstRun = true;

            // todo: change frequency
            _updateTimer = new Timer(2*1000); // 5 min
            _updateTimer.Elapsed += _updateTimer_Elapsed;
        }

        private void RunUpdates()
        {
            RnLogger.Instance.Debug("Running updates");

            foreach (var domain in _domains)
            {
                DdnsWorker.Instance.UpdateIpAddress(domain);
            }
        }

        private void StartUpdateTimerLoop()
        {
            _updateTimer.Start();
            RnLogger.Instance.Info(string.Format(
                "Started update timer loop. ({0} ms)",
                _updateTimer.Interval));
        }

        void _updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RnLogger.Instance.Debug("Update timer elapsed");
            _updateTimer.Stop();
            
            RunUpdates();

            if (_firstRun)
            {
                _firstRun = false;
                _updateTimer.Interval = 5*60*1000; // 5 min
                RnLogger.Instance.Debug("Setting update timer to 5 min - completed first run");
            }

            _updateTimer.Start();
        }
    }
}
