using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace VidPub.Web.Infrastructure.Logging
{
    public class NLogger : ILogger
    {
        private readonly Logger _logger;

        public NLogger(Logger logger)
        {
            _logger = logger;
        }

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warn(message);
        }

        public void LogError(string message)
        {
            _logger.Error(message);
        }

        public void LogException(Exception ex)
        {
            _logger.LogException(LogLevel.Error, ex.Message, ex);
        }
    }
}