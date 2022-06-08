using System;
using System.Globalization;
using System.Xml;
using RnCore.Helpers;

namespace RnCore.Logging
{
    public class LoggerBase
    {
        public LoggerType Type { get; private set; }
        public string Name { get; private set; }
        public bool Enabled { get; private set; }
        public LoggerSeverity LoggingLevel { get; private set; }
        public bool CanLog { get; internal set; }
        public string DateFormat { get; private set; }
        public string LoggingFormat { get; private set; }


        public LoggerBase(XmlNode n, LoggerType type)
        {
            try
            {
                Type = type;
                CanLog = false;
                Name = n.GetAttr("Name", "default");
                Enabled = n.GetAttrBool("Enabled");
                LoggingLevel = RnLogger.AsLoggerSeverity(n.GetAttr("Level", "warning"));
                DateFormat = n.GetAttr("DateFormat", "Y-m-d H:i:s");
                LoggingFormat = n.GetAttr("LoggingFormat", "[{d}] {S} [{F}] ({i}) {m}");
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }


        protected string FormatMessage(string msg, int eventId, LoggerSeverity sev)
        {
            var formatted = LoggingFormat;

            formatted = formatted.Replace("{d}", RnDate.FormatDate(DateFormat));
            formatted = formatted.Replace("{s}", sev.ToString().ToLower());
            formatted = formatted.Replace("{S}", sev.ToString().ToUpper());
            formatted = formatted.Replace("{f}", RnLogger.GetFullMethodPath(6, true));
            formatted = formatted.Replace("{F}", RnLogger.GetFullMethodPath(6));
            formatted = formatted.Replace("{i}", eventId.ToString(CultureInfo.InvariantCulture));
            formatted = formatted.Replace("{m}", msg);

            return formatted;
        }

        


        public virtual void LogDebug(string message, int eventId = 0)
        {
            RnLocale.LogEvent("rn", "rn.0007");
        }

        public virtual void LogInfo(string message, int eventId = 0)
        {
            RnLocale.LogEvent("rn", "rn.0007");
        }

        public virtual void LogWarning(string message, int eventId = 0)
        {
            RnLocale.LogEvent("rn", "rn.0007");
        }

        public virtual void LogError(string message, int eventId = 0)
        {
            RnLocale.LogEvent("rn", "rn.0007");
        }
    }
}
