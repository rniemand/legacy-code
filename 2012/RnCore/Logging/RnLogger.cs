using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using RnCore.Helpers;
using RnCore.Logging.Outputs;

namespace RnCore.Logging
{
    public static class RnLogger
    {
        private static readonly List<LoggerBase> Loggers;


        static RnLogger()
        {
            Loggers = new List<LoggerBase>();
        }



        // =====> Handle the creation of loggers
        public static void CreateLogger(XmlNode n)
        {
            if (!n.GetAttrBool("Enabled"))
                return;

            switch (AsLoggerType(n.GetAttr("Type", "log")))
            {
                case LoggerType.LogFile:
                    LoggerCreateLogFile(n);
                    break;

                default:
                    UnknownLogger(n);
                    return;
            }
        }

        private static void UnknownLogger(XmlNode n)
        {
            RnLocale.LogEvent("rn", "rn.0008",n.GetAttr("Type"));
        }

        private static void LoggerCreateLogFile(XmlNode n)
        {
            if (HasLogger(n.GetAttr("Name"), LoggerType.LogFile))
            {
                RnLocale.LogEvent("rn", "rn.0009", "LogFile", n.GetAttr("Name"));
                return;
            }

            Loggers.Add(new LogFile(n));
        }


        public static LoggerType AsLoggerType(object o)
        {
            try
            {
                switch (o.ToString().ToLower().Trim())
                {
                    case "log":
                    case "logfile":
                        return LoggerType.LogFile;

                    case "console":
                    case "con":
                        return LoggerType.Console;

                    case "nt":
                    case "eventlog":
                        return LoggerType.EventLog;

                    default:
                        return LoggerType.Unknown;
                }
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return LoggerType.Unknown;
            }
        }

        public static LoggerSeverity AsLoggerSeverity(object o)
        {
            switch (o.ToString().ToLower().Trim())
            {
                case "info":
                case "informational":
                case "i":
                    return LoggerSeverity.Informational;

                case "w":
                case "warn":
                case "warning":
                    return LoggerSeverity.Warning;

                case "e":
                case "err":
                case "error":
                    return LoggerSeverity.Error;

                default:
                    return LoggerSeverity.Debug;
            }
        }


        // =====> Misc functionality
        public static bool HasLogger(string name, LoggerType type)
        {
            return Loggers.Count != 0 && Loggers.Any(l => l.Type == type && l.Name == name);
        }

        public static string GetFullMethodPath(int framesBack, bool asShort = false)
        {
            try
            {
                var f = new StackTrace().GetFrame(framesBack).GetMethod();

                if (asShort)
                    return String.Format("{0}.{1}", (f.ReflectedType).Name, f.Name);

                return String.Format("{0}.{1}.{2}", (f.ReflectedType).Namespace, (f.ReflectedType).Name, f.Name);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return "NULL";
            }
        }



        // =====> Used for logging events to all loggers
        public static void LogDebug(string message, int eventId = 0)
        {
            if (Loggers.Count == 0) return;

            foreach (var l in Loggers)
                l.LogDebug(message, eventId);
        }

        public static void LogInfo(string message, int eventId = 0)
        {
            if (Loggers.Count == 0) return;

            foreach (var l in Loggers)
                l.LogInfo(message, eventId);
        }

        public static void LogWarning(string message, int eventId = 0)
        {
            if (Loggers.Count == 0) return;

            foreach (var l in Loggers)
                l.LogWarning(message, eventId);
        }

        public static void LogError(string message, int eventId = 0)
        {
            if (Loggers.Count == 0) return;

            foreach (var l in Loggers)
                l.LogError(message, eventId);
        }
    }
}
