using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.Logging.Outputs
{
    public class OutCsvFile : LoggerOutput
    {
        public string LogFilePath { get; internal set; }
        public int LogFileRollSize { get; internal set; }
        public int LogsToKeep { get; internal set; }

        public OutCsvFile(string logFilePath, int logRollSize = 10, int keepLogs = 10, string loggerName = "default", Severity loggingLevel = Severity.Debug)
            : base(loggerName, loggingLevel, LoggerType.CsvFile)
        {
            // Map basic information
            LogFilePath = logFilePath;
            LogFileRollSize = logRollSize;
            LogsToKeep = keepLogs;

            Console.WriteLine("We need to complete this method");
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
