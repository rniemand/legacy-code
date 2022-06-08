using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Core.Helpers;
using Rn.Core.Logging;

namespace SyncLib
{
    public static class SyncConfig
    {
        private static readonly string ConfigFilePath;
        private static XmlDocument _configXml;

        static SyncConfig()
        {
            // Check for the config file
            ConfigFilePath = String.Format("{0}SyncConfig.xml", IOHelper.BaseDir);
            
            if (File.Exists(ConfigFilePath))
            {
                Locale.LogEvent("sync_lib", "0001", ConfigFilePath);
                Environment.Exit(-1);
            }

            LoadConfigFile();
        } 


        private static void LoadConfigFile()
        {
            try
            {
                _configXml = new XmlDocument();
                _configXml.Load(ConfigFilePath);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                Environment.Exit(-1);
            }
        }

        public static void GetActiveSyncs()
        {
            
        }


    }

}
