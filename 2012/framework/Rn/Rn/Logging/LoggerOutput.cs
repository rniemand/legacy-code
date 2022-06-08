using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.Logging
{
    public abstract class LoggerOutput
    {
        public string LoggerName { get; internal set; }
        public Severity LoggingLevel { get; internal set; }
        public LoggerType Type { get; internal set; }

        /// <summary>
        /// Creates a new instance of the LoggerOutput virtual class
        /// </summary>
        /// <param name="loggerName">The name of the logger (to help identify it)</param>
        /// <param name="loggingLevel">The level at which this logger logs at</param>
        /// <param name="type">The type of logger that you are creating</param>
        protected LoggerOutput(string loggerName = "default", Severity loggingLevel = Severity.Warning, LoggerType type = LoggerType.Unknown)
        {
            LoggerName = loggerName;
            LoggingLevel = loggingLevel;
            Type = type;
        }

        #region To Be Implemented By All Outputs
        public virtual void LogError(string message, string callingMethod, int eventId = 0)
        {
            throw new NotImplementedException();
        }

        public virtual void LogWarning(string message, string callingMethod, int eventId = 0)
        {
            throw new NotImplementedException();
        }

        public virtual void LogInfo(string message, string callingMethod, int eventId = 0)
        {
            throw new NotImplementedException();
        }

        public virtual void LogDebug(string message, string callingMethod, int eventId = 0)
        {
            throw new NotImplementedException();
        }

        public virtual void LogVerbose(string message, string callingMethod, int eventId = 0)
        {
            throw new NotImplementedException();
        } 

        public virtual void RemoveLogger()
        {
            throw new NotImplementedException();
        }
        #endregion
        
        public bool CanLogEvent(Severity sev)
        {
            return LoggingLevel >= sev;
        }

    }
}
