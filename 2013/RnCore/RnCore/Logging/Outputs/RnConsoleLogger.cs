using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnCore.Logging.Outputs
{
    class RnConsoleLogger : RnLoggerBase
    {

        public RnConsoleLogger(string name, LoggerSeverity severity)
            : base(name, severity, LoggerType.Console)
        {
            // holder for now...
        }


        public override void LogDebug(string message, int eventId)
        {
            Console.WriteLine(
                "{0} [DEBUG] ({1}) {2}",
                DateTime.Now.ToString(CultureInfo.InvariantCulture), eventId, message);
        }

        public override void LogInfo(string message, int eventId)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(
                "{0} [INFO ] ({1}) {2}",
                DateTime.Now.ToString(CultureInfo.InvariantCulture), eventId, message);
            Console.ResetColor();
        }

        public override void LogWarning(string message, int eventId)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                "{0} [WARN ] ({1}) {2}",
                DateTime.Now.ToString(CultureInfo.InvariantCulture), eventId, message);
            Console.ResetColor();
        }

        public override void LogError(string message, int eventId)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                "{0} [ERROR] ({1}) {2}",
                DateTime.Now.ToString(CultureInfo.InvariantCulture), eventId, message);
            Console.ResetColor();
        }

        public override void LogFatal(string message, int eventId)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(
                "{0} [FATAL] ({1}) {2}",
                DateTime.Now.ToString(CultureInfo.InvariantCulture), eventId, message);
            Console.ResetColor();
        }


    }
}
