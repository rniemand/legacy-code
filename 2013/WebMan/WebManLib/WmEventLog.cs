using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rn.WebManLib.Externals.WmEventLog;
using Rn.WebManLib.Helpers;
using Rn.WebManLib.Interfaces;
using Rn.WebManLib.Utils;
using RnCore.Logging;

namespace Rn.WebManLib
{
    public class WmEventLog : IWmEventLog
    {

        public List<SoapEventLog> ListEventLogs(WmAuthApi authInfo)
        {
            if (!ApiUsers.Instance.ValidateApiUser(authInfo))
                return null;

            var logs = new List<SoapEventLog>();

            try
            {
                RnLogger.Loggers.LogDebug("Attempting to list all event logs");
                var eventLogs = EventLog.GetEventLogs();
                logs.AddRange(eventLogs.Select(e => new SoapEventLog(e)));
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return logs;
        }

        public int GetEventCount(WmAuthApi authInfo, string eventlogName)
        {
            if (!ApiUsers.Instance.ValidateApiUser(authInfo))
                return 0;

            try
            {
                RnLogger.Loggers.LogDebug("Getting eventlog event count for '{0}'", 100, eventlogName);

                var log = WmEventLogHelper.GetEventLog(eventlogName);
                return log == null ? 0 : log.Entries.Count;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return 0;
            }
        }

        public SoapEventLog GetEventLogInfo(WmAuthApi authInfo, string eventlogName)
        {
            if (!ApiUsers.Instance.ValidateApiUser(authInfo))
                return null;

            try
            {
                RnLogger.Loggers.LogDebug("Getting information for the eventlog '{0}'", 100, eventlogName);
                
                var log = WmEventLogHelper.GetEventLog(eventlogName);
                return log == null ? null : new SoapEventLog(log);
            }
            catch (Exception ex)
            {
                ex.LogException();
                return null;
            }
        }

        public List<SoapEventLogEntry> GetEventLogEntries(WmAuthApi authInfo, string eventlogName, int numOfEntries, bool includeMessageInfo = false)
        {
            if (!ApiUsers.Instance.ValidateApiUser(authInfo))
                return null;

            var entries = new List<SoapEventLogEntry>();

            try
            {
                var log = WmEventLogHelper.GetEventLog(eventlogName);
                if (log != null)
                {
                    var maxEntry = log.Entries.Count - 1;
                    var startEntry = maxEntry - numOfEntries;

                    for (var i = maxEntry; i >= startEntry; i--)
                        entries.Add(new SoapEventLogEntry(log.Entries[i], includeMessageInfo));
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return entries;
        }

        public SoapEventLogEntry GetEvent(WmAuthApi authInfo, string eventlogName, int eventId)
        {
            if (!ApiUsers.Instance.ValidateApiUser(authInfo))
                return null;

            try
            {
                var log = WmEventLogHelper.GetEventLog(eventlogName);
                
                if (log.Entries.Count + 1 < eventId)
                {
                    // todo: add to logger
                    return null;
                }

                return new SoapEventLogEntry(log.Entries[eventId]);
            }
            catch (Exception ex)
            {
                ex.LogException();
                return null;
            }
        }

    }
}
