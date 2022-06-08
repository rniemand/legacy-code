using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.Configuration
{
    public static class ConfigObj
    {
        private static readonly Dictionary<string, ConfigXml> ConfigFiles;

        static ConfigObj()
        {
            ConfigFiles = new Dictionary<string, ConfigXml>();
        }



        public static bool ConfigLoaded(string configName = "default")
        {
            if (ConfigFiles.Count == 0) return false;
            return ConfigFiles.ContainsKey(configName);
        }

        public static ConfigXml GetConfig(string configName = "default")
        {
            if (!ConfigLoaded(configName)) return null;
            return ConfigFiles[configName];
        }

        public static ConfigXml LoadConfigFile(string filePath, string rootNode, string configName = "default")
        {
            if (ConfigLoaded(configName))
                return ConfigFiles[configName];

            ConfigFiles.Add(configName, new ConfigXml(filePath, rootNode));

            return ConfigFiles[configName];
        }
    }
}
