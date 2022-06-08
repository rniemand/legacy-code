using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RnCore.Logging;

namespace Rn.WebManLib.Helpers
{
    static class WmEventLogHelper
    {

        public static EventLog GetEventLog(string logName)
        {
            try
            {
                RnLogger.Loggers.LogDebug("Looking for eventlog '{0}'", 100, logName);

                logName = logName.ToLower().Trim();
                var logs = EventLog.GetEventLogs();
                foreach (var l in logs.Where(l => l.Log.ToLower().Trim() == logName))
                    return l;

                RnLogger.Loggers.LogWarning("Could not find the eventlog '{0}'", 100, logName);
                return null;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return null;
            }
        }

    }
}
