using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.Logging
{
    public static class LoggingHelper
    {
        // =====> Working with "Severity"
        public static Severity ToSeverity(this int i)
        {
            switch (i)
            {
                case 1:
                    return Severity.Error;
                case 2:
                    return Severity.Warning;
                case 3:
                    return Severity.Information;
                case 4:
                    return Severity.Debug;
                default:
                    return Severity.Warning;
            }
        }

        public static int ToInt(this Severity s)
        {
            switch (s)
            {
                case Severity.Error:
                    return 1;
                case Severity.Warning:
                    return 2;
                case Severity.Information:
                    return 3;
                case Severity.Debug:
                    return 4;
                default:
                    return 2;
            }
        }

        public static string ToString(this Severity s)
        {
            switch (s)
            {
                default:
                    return "Unknown";
                case Severity.Error:
                    return "Error";
                case Severity.Warning:
                    return "Warning";
                case Severity.Information:
                    return "Informational";
                case Severity.Debug:
                    return "Debug";
            }
        }

        // =====> Working with "LoggerType"
        public static LoggerType ToLoggerType(this int i)
        {
            switch (i)
            {
                case 2:
                    return LoggerType.Console;
                case 3:
                    return LoggerType.CsvFile;
                case 4:
                    return LoggerType.LogFile;
                case 5:
                    return LoggerType.NtLog;
                default:
                    return LoggerType.Unknown;
            }
        }

        public static int ToInt(this LoggerType t)
        {
            switch (t)
            {
                case LoggerType.Console:
                    return 2;
                case LoggerType.CsvFile:
                    return 3;
                case LoggerType.LogFile:
                    return 4;
                case LoggerType.NtLog:
                    return 5;
                default:
                    return 1;
            }
        }
    }
}
