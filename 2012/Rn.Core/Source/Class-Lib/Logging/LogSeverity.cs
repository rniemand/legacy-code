using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.Core.Logging
{
    public enum LogSeverity
    {
        Sql,
        Debug,
        Info,
        Warning,
        Error
    }

    public static class LogSeverityEn
    {
        public static LogSeverity AsLogSeverity(this string s)
        {
            switch (s.ToLower().Trim())
            {
                case "debug":
                case "d":
                    return LogSeverity.Debug;

                case "info":
                case "information":
                case "i":
                    return LogSeverity.Info;

                case "warning":
                case "warn":
                case "w":
                    return LogSeverity.Warning;

                case "error":
                case "critical":
                case "e":
                    return LogSeverity.Error;

                case "sql":
                    return LogSeverity.Sql;

                default:
                    return LogSeverity.Info;
            }
        }

        public static string AsString(this LogSeverity s)
        {
            switch (s)
            {
                case LogSeverity.Debug:
                    return "Debug";
                case LogSeverity.Error:
                    return "Error";
                case LogSeverity.Sql:
                    return "SQL";
                case LogSeverity.Warning:
                    return "Warning";
                default:
                    return "Info";
            }
        }
    }
}
