using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Core.Logging;

namespace Rn.Core.Configuration
{
    public static class Config
    {
        private static readonly Dictionary<string, XmlConfig> XmlConfig;
        

        static Config()
        {
            XmlConfig = new Dictionary<string, XmlConfig>();
        }


        public static XmlConfig GetXmlConfig(string name = "default")
        {
            if (!HasXmlConfigFile(name))
            {
                Logger.LogDebug(String.Format("Could not find XmlConfig: {0}", name));
                return null;
            }

            return XmlConfig[name];
        }

        public static bool HasXmlConfigFile(string cfgName)
        {
            return XmlConfig.ContainsKey(cfgName.ToLower().Trim());
        }

        public static void LoadXmlConfig(string rootEl, string filePath = "./Config.xml", string name = "default")
        {
            if (HasXmlConfigFile(name))
            {
                Locale.LogEvent("rn.core", "0002", name);
                return;
            }

            XmlConfig.Add(name, new XmlConfig(rootEl, filePath, name));
            Locale.LogEvent("rn.core", "0003", name);
        }

    }
}
