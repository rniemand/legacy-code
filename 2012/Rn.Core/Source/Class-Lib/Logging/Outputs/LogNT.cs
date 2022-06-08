using System;
using System.Diagnostics;
using System.Xml;
using Rn.Core.Helpers;

namespace Rn.Core.Logging.Outputs
{
    public class LogNT : LogOutput
    {
        public string NTLog { get; private set; }
        public string NTSource { get; private set; }
        private EventLog _log;


        // Class Constructor
        public LogNT(string name, LogSeverity sev, XmlNode n)
            : base(name, LoggerType.NT, sev)
        {
            try
            {
                NTLog = n.GetAttribute("Log", "Application");
                NTSource = n.GetAttribute("Source", "Rn.Core");
                
                if (!CheckEventLog()) return;
                if (!CheckEventSource()) return;

                LoggerReady = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format("Error creating a new instance of 'LogNT': {0}", ex.Message));
            }
        }


        // Constructro methods
        private bool CheckEventLog()
        {
            if (!EventLog.Exists(NTLog))
            {
                Logger.LogWarning(String.Format("The NT Log '{0}' was not found", NTLog));
                return false;
            }

            try
            {
                _log = new EventLog(NTLog);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format("Error checking for the given event log: {0}", ex.Message));
                return false;
            }
        }

        private bool CheckEventSource()
        {
            try
            {
                if (!EventLog.SourceExists(NTSource))
                    EventLog.CreateEventSource(NTSource, NTLog);

                return EventLog.SourceExists(NTSource);
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format("Error while checking for the given event source: {0}", ex.Message));
                return false;
            }
        }

        private void LogEvent(string msg, EventLogEntryType sev, int eventId, bool isDebug = false)
        {
            try
            {
                msg = String.Format(isDebug ? "[{0}] (DEBUG) {1}" : "[{0}] {1}", GetFullMethodPath(4), msg);
                _log.Source = NTSource;
                _log.WriteEntry(msg, sev, eventId);
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format("Error while logging event: {0}", ex.Message));
            }
        }

        // Logging methods
        public override void LogDebug(string msg, int eventId = 0, bool fromLocale = false)
        {
            LogEvent(msg, EventLogEntryType.Information, eventId, true);
        }

        public override void LogInfo(string msg, int eventId = 0, bool fromLocale = false)
        {
            LogEvent(msg, EventLogEntryType.Information, eventId);
        }

        public override void LogWarning(string msg, int eventId = 0, bool fromLocale = false)
        {
            LogEvent(msg, EventLogEntryType.Warning, eventId);
        }

        public override void LogError(string msg, int eventId = 0, bool fromLocale = false)
        {
            LogEvent(msg, EventLogEntryType.Error, eventId);
        }

    }
}
