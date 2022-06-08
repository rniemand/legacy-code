using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Rn.Core.Logging
{
    public abstract class LogOutput
    {
        public string Name { get; private set; }
        public LoggerType Type { get; private set; }
        public LogSeverity Severity { get; private set; }
        public DateTime TimeAdded { get; private set; }
        public long LoggedEvents { get; private set; }
        public bool LoggerReady { get; internal set; }


        protected LogOutput(string name, LoggerType type, LogSeverity sev)
        {
            Name = name;
            Type = type;
            Severity = sev;
            TimeAdded = DateTime.UtcNow;
            LoggerReady = false;
            LoggedEvents = 0;
        }

        internal bool CanLog(LogSeverity sev)
        {
            if(!LoggerReady) return false;
            return (sev >= Severity);
        }

        internal string GetFullMethodPath(int framesBack, bool asShort = false)
        {
            var f = new StackTrace().GetFrame(framesBack).GetMethod();

            if (asShort)
                return String.Format("{0}.{1}", (f.ReflectedType).Name, f.Name);

            return String.Format("{0}.{1}.{2}", (f.ReflectedType).Namespace, (f.ReflectedType).Name, f.Name);
        }


        // Logging methods
        public virtual void LogDebug(string msg, int eventId = 0, bool fromLocale = false)
        {
            throw new NotImplementedException("LogDebug() not implemented!");
        }

        public virtual void LogInfo(string msg, int eventId = 0, bool fromLocale = false)
        {
            throw new NotImplementedException("LogInfo() not implemented!");
        }

        public virtual void LogWarning(string msg, int eventId = 0, bool fromLocale = false)
        {
            throw new NotImplementedException("LogWarning() not implemented!");
        }

        public virtual void LogError(string msg, int eventId = 0, bool fromLocale = false)
        {
            throw new NotImplementedException("LogCritical() not implemented!");
        }

    }
}
