using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.Core.Logging
{
    public enum LoggerType
    {
        Console,
        NT,
        LogFile,
        CSV,
        Syslog,
        SQL,
        Unknown
    }

    public static class LoggerTypeEn
    {
        public static LoggerType AsLoggerType(this string s)
        {
            switch (s.ToLower().Trim())
            {
                case "console":
                    return LoggerType.Console;

                case "nt":
                case "windows":
                    return LoggerType.NT;

                case "log":
                case "logfile":
                    return LoggerType.LogFile;

                case "syslog":
                    return LoggerType.Syslog;

                case "sql":
                case "db":
                    return LoggerType.SQL;

                default:
                    return LoggerType.Unknown;
            }
        }
    }
}
