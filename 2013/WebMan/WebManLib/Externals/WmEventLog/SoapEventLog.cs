using System;
using System.Diagnostics;
using RnCore.Logging;

namespace Rn.WebManLib.Externals.WmEventLog
{
    public class SoapEventLog
    {
        public bool EnableRaisingEvents { get; set; }
        public string Log { get; set; }
        public string LogDisplayName { get; set; }
        public string MachineName { get; set; }
        public long MaximumKilobytes { get; set; }
        public int MinimumRetentionDays { get; set; }
        public string OverflowAction { get; set; }
        public string Source { get; set; }


        public SoapEventLog()
        {
            // Generic constructor
        }

        public SoapEventLog(EventLog log)
        {
            try
            {
                EnableRaisingEvents = log.EnableRaisingEvents;
                Log = log.Log;
                LogDisplayName = log.LogDisplayName;
                MachineName = log.MachineName;
                MaximumKilobytes = log.MaximumKilobytes;
                MinimumRetentionDays = log.MinimumRetentionDays;
                OverflowAction = log.OverflowAction.ToString();
                Source = log.Source;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

    }
}
