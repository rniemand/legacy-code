using System;
using System.Collections.Generic;
using RnCore.Helpers;

namespace RnCore.Configuration
{
    public static class ConfigFactory
    {
        private static readonly Dictionary<string, Config> Configs;

        static ConfigFactory()
        {
            Configs = new Dictionary<string, Config>();
        }


        // Working with the registration of the config files
        public static bool ConfigLoaded(string name = "default")
        {
            return Configs.ContainsKey(name);
        }

        public static void RegisterConfig(Config cfg, string name = "default")
        {
            if (ConfigLoaded(name))
            {
                RnLocale.LogEvent("rn", "rn.0004", name, cfg.FilePath);
                return;
            }

            Configs.Add(name, cfg);
        }

        public static void RegisterConfig(string fileName, string xmlRootEl, string name = "default")
        {
            if (ConfigLoaded(name))
                return;

            try
            {
                Configs.Add(name, new Config(fileName, xmlRootEl));
                RnLocale.LogEvent("rn", "rn.0005", fileName, name);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }

        public static void DeregisterConfig(string name = "default")
        {
            if(!ConfigLoaded(name))
                return;

            try
            {
                Configs.Remove(name);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }



        // Misc methods
        public static void CreateLoggers()
        {
            if (Configs.Count == 0)
                return;

            try
            {
                foreach (var cfg in Configs)
                    cfg.Value.CreateLoggers();
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }




    }
}
