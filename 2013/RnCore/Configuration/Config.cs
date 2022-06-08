using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using RnCore.Helpers;
using RnCore.Logging;

namespace RnCore.Configuration
{
    public class Config
    {
        public string RootElName { get; private set; }
        public string FilePath { get; private set; }
        private readonly XmlDocument _config;

        public Config(string fileName, string xmlRootEl)
        {
            try
            {
                RootElName = xmlRootEl;
                FilePath = String.Format("{0}{1}.xml", RnIO.BaseDir, fileName);

                if (!File.Exists(FilePath))
                {
                    RnLocale.LogEvent("rn", "rn.0001", FilePath);
                    Thread.Sleep(2500);
                    Environment.Exit(-1);
                }

                _config = new XmlDocument();
                _config.Load(FilePath);
                SetupLocale();
                RnLocale.LogEvent("rn", "rn.0002", FilePath);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }

        private void SetupLocale()
        {
            try
            {
                // Get config values
                var xpath = String.Format("/{0}/Locale", RootElName);
                var node = _config.SelectSingleNode(xpath);
                var sLanguage = (node == null ? "en" : node.GetAttr("Language", "en"));
                var sDirectory = (node == null ? "./Locale" : node.GetAttr("Directory", "./Locale"));

                // Build the working directory
                sDirectory = sDirectory.MakeRelative();
                if (sDirectory.Substring(sDirectory.Length - 1, 1) != @"\")
                    sDirectory = String.Format(@"{0}\", sDirectory);

                // Setup RnLocale
                RnLocale.SetPaths(sLanguage, sDirectory);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }


        // Working with the system loggers
        public XmlNodeList GetLoggersXmlNodes()
        {
            try
            {
                var xpath = String.Format("/{0}/Loggers/Logger", RootElName);
                return _config.SelectNodes(xpath);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return null;
            }
        }

        public void CreateLoggers()
        {
            try
            {
                var loggerNodes = GetLoggersXmlNodes();
                if (loggerNodes == null || loggerNodes.Count == 0)
                {
                    RnLocale.LogEvent("rn", "rn.0003", FilePath);
                    return;
                }

                foreach (XmlNode n in loggerNodes)
                {
                    var loggerEnabled = n.GetAttrBool("Enabled", true);
                    if (!loggerEnabled) continue;
                    RnLogger.CreateLogger(n);
                }
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }


    }
}
