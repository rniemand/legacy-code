using System;
using log4net;
using log4net.Config;

namespace RnDdns.Common.Helpers
{
    public class RnLogger
    {
        private static readonly Lazy<RnLogger> Lazy = new Lazy<RnLogger>(() => new RnLogger());

        public static RnLogger Instance { get { return Lazy.Value; } }

        private readonly ILog _logger;

        private RnLogger()
        {
            XmlConfigurator.Configure();
            _logger = LogManager.GetLogger("RnDdns");
        }

        // todo: add string formatting overload to this method
        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        // todo: add string formatting overload to this method
        public void Info(string message)
        {
            _logger.Info(message);
        }

        // todo: add string formatting overload to this method
        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        // todo: add string formatting overload to this method
        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(string message, Exception ex)
        {
            _logger.Error(message, ex);
        }
    }
}
