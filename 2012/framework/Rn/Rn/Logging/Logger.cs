using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Logging.Outputs;
using Rn.Helpers;

namespace Rn.Logging
{
    public static class Logger
    {
        public static List<LoggerOutput> Loggers { get; internal set; }

        static Logger()
        {
            Loggers = new List<LoggerOutput>();
        }


        // =====> Working With Registered Loggers
        public static string GetClassName(bool shortName = false, int framesBack = 2)
        {
            var methodInfo = new StackTrace().GetFrame(framesBack).GetMethod();

            if (shortName)
                return String.Format("{0}.{1}", (methodInfo.DeclaringType).FullName, methodInfo.Name);

            return String.Format("{0}.{1}", (methodInfo.DeclaringType).Name, methodInfo.Name);
        }

        public static bool LoggerExists(string loggerName, LoggerType type)
        {
            loggerName = loggerName.ToLower().Trim();
            return Loggers.Count != 0 && Loggers.Any(logger => logger.Type == type && logger.LoggerName == loggerName);
        }

        public static void RemoveLogger(string loggerName, LoggerType type)
        {
            if (!LoggerExists(loggerName, type)) return;
            loggerName = loggerName.ToLower().Trim();

            for (var i = Loggers.Count - 1; i >= 0; i--)
            {
                if (Loggers[i].LoggerName == loggerName && Loggers[i].Type == type)
                {
                    Loggers[i].RemoveLogger();
                    Loggers.RemoveAt(i);
                }
            }
        }


        // =====> Event Logging Methods
        public static void LogError(string message, int eventId = 0)
        {
            if (Loggers.Count == 0) return;

            foreach (var logger in Loggers.Where(logger => logger.CanLogEvent(Severity.Error)))
                logger.LogError(message, GetClassName(), eventId);
        }

        public static void LogWarning(string message, int eventId = 0)
        {
            if (Loggers.Count == 0) return;

            foreach (var logger in Loggers.Where(logger => logger.CanLogEvent(Severity.Warning)))
                logger.LogWarning(message, GetClassName(), eventId);
        }

        public static void LogInfo(string message, int eventId = 0)
        {
            if (Loggers.Count == 0) return;

            foreach (var logger in Loggers.Where(logger => logger.CanLogEvent(Severity.Information)))
                logger.LogInfo(message, GetClassName(), eventId);
        }

        public static void LogDebug(string message, int eventId = 0)
        {
            if (Loggers.Count == 0) return;

            foreach (var logger in Loggers.Where(logger => logger.CanLogEvent(Severity.Debug)))
                logger.LogDebug(message, GetClassName(), eventId);
        }


        // =====> Registering Loggers
        public static void CreateLogger(XmlNode node)
        {
            if (node == null) return;

            // Get basic logger information
            var type = node.GetAttribute("Type", "none").ToLoggerType();
            var enabled = node.GetAttribute("Enabled", false);
            var severity = node.GetAttribute("LoggingSeverity", "Warning").ToLoggingSeverity();
            var name = node.GetAttribute("Name", "default");
            if (!enabled) return;

            switch (type)
            {
                case LoggerType.Console:
                    AddConsoleLogger(node, severity, name);
                    return;

                case LoggerType.CsvFile:
                    AddCsvFileLogger(node, severity, name);
                    return;

                case LoggerType.LogFile:
                    AddLogFileLogger(node, severity, name);
                    return;

                case LoggerType.NtLog:
                    AddNtLogLogger(node, severity, name);
                    return;

                default:
                    return;
            }
        }

        #region Console Loggers
        public static void AddConsoleLogger(string name = "default", Severity lvl = Severity.Debug)
        {
            if (LoggerExists(name, LoggerType.Console))
                return;

            Loggers.Add(new OutConsole(name, lvl));
        }

        private static void AddConsoleLogger(XmlNode n, Severity sev, string name = "default")
        {
            if (LoggerExists(name, LoggerType.Console))
                return;

            Loggers.Add(new OutConsole(name, sev));
        } 
        #endregion

        #region CSV File Loggers
        public static void AddCsvFileLogger()
        {
            // todo - implement this logger type (CSV Logger)
        }

        private static void AddCsvFileLogger(XmlNode n, Severity sev, string name = "default")
        {
            // todo - implement this logger type (CSV Logger)
        } 
        #endregion

        #region Log File Loggers
        public static void AddLogFileLogger(string path, int maxLogSize = 10, int keepLogFiles = 10, string name = "default", Severity lvl = Severity.Debug)
        {
            if (LoggerExists(name, LoggerType.LogFile))
                return;

            Loggers.Add(new OutLogFile(path, maxLogSize, keepLogFiles, name, lvl));
        }

        private static void AddLogFileLogger(XmlNode n, Severity sev, string name = "default")
        {
            // Get additional logger configuration
            var filePath = n.GetAttribute("FilePath", @"./default.log");
            var maxLogSize = n.GetAttribute("MaxLogSize", 10);
            var keepLogs = n.GetAttribute("KeepLogs", 10);

            // Create the logger
            AddLogFileLogger(filePath, maxLogSize, keepLogs, name, sev);
        } 
        #endregion

        #region NT Log Loggers
        public static void AddNtLogLogger()
        {

        }

        private static void AddNtLogLogger(XmlNode n, Severity sev, string name = "default")
        {

        } 
        #endregion


    }
}
