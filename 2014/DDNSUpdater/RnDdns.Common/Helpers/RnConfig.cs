using System;
using System.Linq;
using System.Xml;
using RnDdns.Common.Mappers;
using RnDdns.Common.Models;

namespace RnDdns.Common.Helpers
{
    public class RnConfig
    {
        private static readonly Lazy<RnConfig> Lazy = new Lazy<RnConfig>(() => new RnConfig());

        public static RnConfig Instance { get { return Lazy.Value; } }

        private readonly string _configFile;
        public RnDdnsConfig CurrentConfig { get; set; }

        private RnConfig()
        {
            _configFile = "./rn.config.xml";
            CurrentConfig = new RnDdnsConfig();

            LoadConfigFile();
        }

        private void LoadConfigFile()
        {
            RnLogger.Instance.Debug("Attempting to load configuration file...");

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(_configFile);
                LoadCredentials(xmlDoc);
                LoadDomains(xmlDoc);

                RnLogger.Instance.Info("Successfully loaded config file");
            }
            catch (Exception ex)
            {
                RnLogger.Instance.Error("Error while loading config file", ex);
            }
        }

        private void LoadCredentials(XmlDocument xmlDoc)
        {
            var nodes = xmlDoc.SelectNodes("/RnConfig/Credentials/Credentials");

            if (nodes == null)
            {
                // todo: make this better
                throw new Exception("There are no configured CREDENTIALS");
            }

            RnLogger.Instance.Info(string.Format("Found {0} credential entries", nodes.Count));

            foreach (XmlNode node in nodes)
            {
                CurrentConfig.Credentials.Add(ConfigurationMapper.MapCredentials(node));
            }

            RnLogger.Instance.Debug("Finished mapping credentials");
        }

        private void LoadDomains(XmlDocument xmlDoc)
        {
            var nodes = xmlDoc.SelectNodes("/RnConfig/Domains/Domain");

            if (nodes == null)
            {
                // todo: make this better
                throw new Exception("There are no configured DOMAINS");
            }

            RnLogger.Instance.Info(string.Format("Found {0} domain entries", nodes.Count));

            foreach (XmlNode node in nodes)
            {
                var domain = ConfigurationMapper.MapDomain(node);

                domain.Credentials = CurrentConfig.Credentials.FirstOrDefault(c =>
                    c.Id.Equals(domain.CredentialsId, StringComparison.InvariantCultureIgnoreCase));

                CurrentConfig.Domains.Add(domain);
            }

            RnLogger.Instance.Debug("Finished mapping domains");
        }
    }
}
