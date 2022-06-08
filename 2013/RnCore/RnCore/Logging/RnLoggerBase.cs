using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnCore.Logging
{
    abstract class RnLoggerBase
    {
        public string LoggerName { get; private set; }
        public LoggerSeverity Severity { get; private set; }
        public LoggerType LoggerType { get; private set; }

        protected RnLoggerBase(string name, LoggerSeverity sev, LoggerType type)
        {
            LoggerName = name;
            Severity = sev;
            LoggerType = type;

            Console.WriteLine("RnLoggerBase :: New Logger ({0}) ({1}) ({2})", LoggerType, Severity, LoggerName);
        }



        // Methods that need to be implimented
        public virtual void LogDebug(string message, int eventId)
        {
            Console.WriteLine("NOT IMPLIMENTED :: LogDebug();");
        }

        public virtual void LogInfo(string message, int eventId)
        {
            Console.WriteLine("NOT IMPLIMENTED :: LogInfo();");
        }

        public virtual void LogWarning(string message, int eventId)
        {
            Console.WriteLine("NOT IMPLIMENTED :: LogWarning();");
        }

        public virtual void LogError(string message, int eventId)
        {
            Console.WriteLine("NOT IMPLIMENTED :: LogError();");
        }

        public virtual void LogFatal(string message, int eventId)
        {
            Console.WriteLine("NOT IMPLIMENTED :: LogFatal();");
        }




    }
}
