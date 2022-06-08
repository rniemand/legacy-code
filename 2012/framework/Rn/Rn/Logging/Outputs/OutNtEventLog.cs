using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.Logging.Outputs
{
    public class OutNtEventLog : LoggerOutput
    {
        public string TargetLogName { get; internal set; }
        public string LoggingSource { get; internal set; }

        public OutNtEventLog(string logName, string loggingSource, string loggerName = "default", Severity loggingLevel = Severity.Warning)
            : base(loggerName, loggingLevel, LoggerType.NtLog)
        {
            TargetLogName = logName;
            LoggingSource = loggingSource;

            Console.WriteLine("Need to complete the construction of this class");
        }


        public override void LogError(string message, string callingMethod, int eventId = 0)
        {
        }

        public override void LogWarning(string message, string callingMethod, int eventId = 0)
        {
        }

        public override void LogInfo(string message, string callingMethod, int eventId = 0)
        {
        }

        public override void LogDebug(string message, string callingMethod, int eventId = 0)
        {
        }

        public override void LogVerbose(string message, string callingMethod, int eventId = 0)
        {
        }

    }
}
