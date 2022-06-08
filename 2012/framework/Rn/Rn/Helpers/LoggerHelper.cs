using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rn.Logging;

namespace Rn.Helpers
{
    public static class LoggerHelper
    {
        public static Severity ToLoggingSeverity(this string s, Severity defaultValue = Severity.Warning)
        {
            if(String.IsNullOrEmpty(s))
                return defaultValue;

            switch (s.ToLower().Trim())
            {
                case "error":
                    return Severity.Error;

                case "warning":
                case "warn":
                    return Severity.Warning;

                case "informational":
                case "info":
                    return Severity.Information;

                case "debug":
                    return Severity.Debug;

                default:
                    return defaultValue;
            }
        }

        public static LoggerType ToLoggerType(this string s)
        {
            switch (s.ToLower().Trim())
            {
                case "console":
                    return LoggerType.Console;

                case "csvfile":
                    return LoggerType.CsvFile;

                case "logfile":
                    return LoggerType.LogFile;

                case "ntlog":
                    return LoggerType.NtLog;

                default:
                    return LoggerType.Unknown;
            }
        }
    }
}
