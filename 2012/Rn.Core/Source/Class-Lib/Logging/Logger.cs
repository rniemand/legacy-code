using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Rn.Core.Helpers;
using Rn.Core.Logging.Outputs;

namespace Rn.Core.Logging
{
    public static class Logger
    {
        private static readonly List<LogOutput> Loggers;


        static Logger()
        {
            Loggers = new List<LogOutput>();
        }

        public static bool HasLogger(string name, LoggerType type)
        {
            return Loggers.Count != 0 && Loggers.Any(l => l.Type == type && l.Name == name);
        }

        public static void AddLogger(XmlNode n)
        {
            // LoggerType

            if (n == null)
            {
                LogWarning("Cannot create a logger from a NULL XmlNode!");
                return;
            }

            // Is the logger enabled?
            if (!n.GetAttributeBool("Enabled")) return;

            // Checking for the logger
            var lName = n.GetAttribute("Name");
            var lType = n.GetAttribute("Type").AsLoggerType();
            var lSev = n.GetAttribute("Severity").AsLogSeverity();

            // Check to see if we dont already have this logger
            if(HasLogger(lName,lType))
            {
                LogWarning(String.Format("There is already a logger of type '{0}' with the name '{1}'", lType, lName));
                return;
            }

            switch (n.GetAttribute("Type").AsLoggerType())
            {
                case LoggerType.CSV:
                    Loggers.Add(new LogCSV(lName, lSev));
                    return;

                case LoggerType.Console:
                    Loggers.Add(new LogConsole(lName, lSev));
                    return;

                case LoggerType.LogFile:
                    Loggers.Add(new LogFile(lName, lSev, n));
                    return;

                case LoggerType.NT:
                    Loggers.Add(new LogNT(lName, lSev, n));
                    break;

                case LoggerType.SQL:
                    Loggers.Add(new LogSQL(lName, lSev));
                    return;

                case LoggerType.Syslog:
                    Loggers.Add(new LogSyslog(lName, lSev));
                    return;

                default:
                    LogWarning(String.Format("Unknown logger type '{0}'", n.GetAttribute("Type")));
                    return;
            }
        }


        #region Logging Methods
        public static void LogDebug(string message, int eventId = 0, bool fromLocale = false)
        {
            if (Loggers.Count == 0) return;

            foreach (var l in Loggers.Where(l => l.CanLog(LogSeverity.Debug)))
                l.LogDebug(message, eventId, fromLocale);
        }

        public static void LogInfo(string message, int eventId = 0, bool fromLocale = false)
        {
            if (Loggers.Count == 0) return;

            foreach (var l in Loggers.Where(l => l.CanLog(LogSeverity.Info)))
                l.LogInfo(message, eventId, fromLocale);
        }

        public static void LogWarning(string message, int eventId = 0, bool fromLocale = false)
        {
            if (Loggers.Count == 0) return;

            foreach (var l in Loggers.Where(l => l.CanLog(LogSeverity.Warning)))
                l.LogWarning(message, eventId, fromLocale);
        }

        public static void LogError(string message, int eventId = 0, bool fromLocale = false)
        {
            if (Loggers.Count == 0) return;

            foreach (var l in Loggers.Where(l => l.CanLog(LogSeverity.Error)))
                l.LogError(message, eventId, fromLocale);
        }

        public static void LogEvent(LocaleStringResource sr)
        {
            switch (sr.Severity)
            {
                case LogSeverity.Error:
                    LogError(sr.Value, sr.EventId, true);
                    return;

                case LogSeverity.Info:
                    LogInfo(sr.Value, sr.EventId, true);
                    return;

                case LogSeverity.Warning:
                    LogWarning(sr.Value, sr.EventId, true);
                    return;

                case LogSeverity.Debug:
                    LogDebug(sr.Value, sr.EventId, true);
                    return;

                default:
                    LogWarning(String.Format(
                        "Unknown event severity '{0}' for the string resource '{1}' in locale file '{2}', please correct this",
                        sr.Severity, sr.Name, sr.LocaleFile), 107);
                    LogDebug(sr.Value, sr.EventId, true);
                    return;
            }
        } 
        #endregion

    }
}
