using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RnCore.Logging;

namespace RnCore.Config
{
    public class RnConfig
    {
        private static readonly RnConfig instance = new RnConfig();
        private readonly Dictionary<string, ConfigFile> _configFiles = null;
        private string _defaultConfig = null;

        private RnConfig() {
            _configFiles = new Dictionary<string, ConfigFile>();
        }

        public static RnConfig Instance
        {
            get {
                return instance;
            }
        }

        public static ConfigFile DefaultConfig
        {
            get
            {
                return instance._defaultConfig == null ? null : instance._configFiles[instance._defaultConfig];
            }
        }



        public bool LoadConfig(string filePath, string rootEl = "RnConfig", string configName = "default")
        {
            try
            {
                if (ConfigLoaded(configName))
                {
                    RnLogger.Loggers.LogWarning("The config '{0}' is already loaded", 103, configName);
                    return false;
                }

                if (!File.Exists(filePath))
                {

                    RnLogger.Loggers.LogError("The config file '{0}' could not be found", 102, filePath);
                    return false;
                }

                var isDefaultConfig = _configFiles.Count == 0;
                _configFiles.Add(configName, new ConfigFile(filePath, rootEl, configName));
                
                if (isDefaultConfig)
                    instance._defaultConfig = configName;

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool ConfigLoaded(string configName)
        {
            try {
                return _configFiles.ContainsKey(configName);
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public ConfigFile GetConfig(string configName)
        {
            if (!ConfigLoaded(configName))
            {
                RnLogger.Loggers.LogWarning("The requested config object '{0}' is not loaded", 104, configName);
                return null;
            }

            try
            {
                return _configFiles[configName];
            }
            catch (Exception ex)
            {
                ex.LogException();
                return null;
            }
        }


    }
}
