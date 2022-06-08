using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rn.Configuration;
using Rn.Logging;

namespace MpTools.Classes
{
    public static class GlobalObj
    {
        public static ConfigXml MainConfig { get; internal set; }

        public static void LoadMainConfigFile(string filePath)
        {
            MainConfig = ConfigObj.LoadConfigFile(filePath, "MpToolsConfig");
            MainConfig.CreateLoggers();
            Logger.LogDebug("MpTools GlobalObj created.");
        }
    }
}
