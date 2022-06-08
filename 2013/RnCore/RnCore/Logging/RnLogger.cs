using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RnCore.Config;
using RnCore.Helpers;
using RnCore.Logging.Outputs;

namespace RnCore.Logging
{
    public class RnLogger
    {
        private static readonly RnLogger instance = new RnLogger();
        private List<RnLoggerBase> _loggers = null;


        // Class Constructor
        public static RnLogger Loggers
        {
            get { return instance; }
        }

        private RnLogger()
        {
            _loggers = new List<RnLoggerBase>();
        }


        public void RegisterConfigLoggers(string configName = "default")
        {
            try
            {
                var config = RnConfig.Instance.GetConfig(configName);
                if (config == null) return;

                var loggers = config.GetRootNode("Loggers");
                if (loggers.ChildNodes.Count == 0) return;

                foreach (XmlNode n in loggers)
                    CreateLogger(n);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }



        public void CreateLogger(XmlNode n)
        {
            try
            {
                var loggerType = AsLoggerType(n.GetAttributeString("Type"));

                switch (loggerType)
                {
                    case LoggerType.Console:
                        RegisterConsoleLogger(n);
                        return;

                    default:
                        LogWarning("Unknown logger type '{0}'", 105, loggerType);
                        return;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public bool HasLogger(LoggerType loggerType, string loggerName)
        {
            try
            {
                return _loggers.Count != 0 && _loggers.Any(l => l.LoggerType == loggerType && l.LoggerName == loggerName);
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool RegisterConsoleLogger(XmlNode n)
        {
            try
            {
                var name = n.GetAttributeString("Name");
                var severity = AsLoggerSeverity(n.GetAttributeString("Severity"));

                if (HasLogger(LoggerType.Console, name))
                {
                    LogWarning("A logger of type '{0}' with the name '{1}' already exists", 106, "Console", name);
                    return true;
                }

                _loggers.Add(new RnConsoleLogger(name, severity));
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }



        /* ************************************************************************
         * Logging Methods
         ************************************************************************ */
        public void LogDebug(string message, int eventId = 100, params object[] replace)
        {
            try
            {
                if (_loggers.Count == 0)
                    return;

                if (replace.Length > 0)
                    message = String.Format(message, replace);

                message = String.Format("[{0}] {1}", RnLoggerHelper.GetFrameName(), message);
                foreach (var l in _loggers.Where(l => LoggerSeverity.Debug >= l.Severity))
                    l.LogDebug(message, eventId);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void LogInfo(string message, int eventId = 100, params object[] replace)
        {
            try
            {
                if (_loggers.Count == 0)
                    return;

                if (replace.Length > 0)
                    message = String.Format(message, replace);

                message = String.Format("[{0}] {1}", RnLoggerHelper.GetFrameName(), message);
                foreach (var l in _loggers.Where(l => LoggerSeverity.Informational >= l.Severity))
                    l.LogInfo(message, eventId);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void LogWarning(string message, int eventId = 100, params object[] replace)
        {
            try
            {
                if (_loggers.Count == 0)
                    return;

                if (replace.Length > 0)
                    message = String.Format(message, replace);

                message = String.Format("[{0}] {1}", RnLoggerHelper.GetFrameName(), message);
                foreach (var l in _loggers.Where(l => LoggerSeverity.Warning >= l.Severity))
                    l.LogWarning(message, eventId);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void LogError(string message, int eventId = 100, params object[] replace)
        {
            try
            {
                if (_loggers.Count == 0)
                    return;

                if (replace.Length > 0)
                    message = String.Format(message, replace);

                message = String.Format("[{0}] {1}", RnLoggerHelper.GetFrameName(), message);
                foreach (var l in _loggers.Where(l => LoggerSeverity.Error >= l.Severity))
                    l.LogError(message, eventId);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void LogFatal(string message, int eventId = 100, params object[] replace)
        {
            try
            {
                if (_loggers.Count == 0)
                    return;

                if (replace.Length > 0)
                    message = String.Format(message, replace);

                message = String.Format("[{0}] {1}", RnLoggerHelper.GetFrameName(), message);
                foreach (var l in _loggers.Where(l => LoggerSeverity.Fatal >= l.Severity))
                    l.LogFatal(message, eventId);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void LogException(string message)
        {
            try
            {
                if (_loggers.Count == 0)
                    return;

                message = String.Format("[{0}] {1}", RnLoggerHelper.GetFrameName(), message);
                foreach (var l in _loggers.Where(l => LoggerSeverity.Error >= l.Severity))
                    l.LogError(message, 100);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }



        public static LoggerSeverity AsLoggerSeverity(object o)
        {
            try
            {
                switch (o.ToString().ToLower().Trim())
                {
                    case "debug":
                        return LoggerSeverity.Debug;

                    case "informational":
                    case "info":
                        return LoggerSeverity.Informational;

                    case "warn":
                    case "warning":
                        return LoggerSeverity.Warning;

                    case "ërror":
                        return LoggerSeverity.Error;

                    case "fatal":
                        return LoggerSeverity.Fatal;

                    default:
                        return LoggerSeverity.Debug;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                return LoggerSeverity.Debug;
            }
        }

        public static LoggerType AsLoggerType(object o)
        {
            try
            {
                switch (o.ToString().ToLower().Trim())
                {
                    case "console":
                    case "con":
                        return LoggerType.Console;

                    case "log":
                    case "logfile":
                        return LoggerType.LogFile;

                    default:
                        return LoggerType.Unknown;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                return LoggerType.Unknown;
            }
        }


    }
}
