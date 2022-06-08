using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RnCore.Enums
{
    public class Logger
    {

        public static LoggerType AsLoggerType(object o)
        {
            try
            {
                switch (o.ToString().ToLower().Trim())
                {
                    case "log":
                    case "logfile":
                        return LoggerType.LogFile;

                    case "console":
                    case "con":
                        return LoggerType.Console;

                    case "nt":
                    case "eventlog":
                        return LoggerType.EventLog;

                    default:
                        return LoggerType.Unknown;
                }
            }
            catch (Exception ex)
            {
                // todo: add to logger
                Console.WriteLine(ex.Message);
                return LoggerType.Unknown;
            }
        }

        public static LoggerSeverity AsLoggerSeverity(object o)
        {
            switch (o.ToString().ToLower().Trim())
            {
                case "info":
                case "informational":
                case "i":
                    return LoggerSeverity.Informational;

                case "w":
                case "warn":
                case "warning":
                    return LoggerSeverity.Warning;

                case "e":
                case "err":
                case "error":
                    return LoggerSeverity.Error;

                default:
                    return LoggerSeverity.Debug;
            }
        }

    }
}
