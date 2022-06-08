using System;
using System.Text;
using Rn.Helpers;

namespace Rn.Logging.Outputs
{
    public class OutConsole : LoggerOutput
    {

        public OutConsole(string loggerName = "default", Severity loggingLevel = Severity.Debug)
            : base(loggerName, loggingLevel, LoggerType.Console)
        {
            // Holder for now
        }

        private string FormatLine(string message, Severity lvl, int eventId = 0)
        {
            var sb = new StringBuilder();

            sb.Append(String.Format("[{0}] {1}", DateHelper.GetFullDate(), lvl.ToString().ToUpper()));
            if (eventId != 0) sb.Append(String.Format(" ({0})", eventId));
            sb.Append(String.Format(": {0}", message));

            return sb.ToString();
        }


        public override void LogError(string message, string callingMethod, int eventId = 0)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(FormatLine(message, Severity.Error, eventId));
            Console.ResetColor();
        }

        public override void LogWarning(string message, string callingMethod, int eventId = 0)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(FormatLine(message, Severity.Warning, eventId));
            Console.ResetColor();
        }

        public override void LogInfo(string message, string callingMethod, int eventId = 0)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(FormatLine(message, Severity.Information, eventId));
            Console.ResetColor();
        }

        public override void LogDebug(string message, string callingMethod, int eventId = 0)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(FormatLine(message, Severity.Debug, eventId));
            Console.ResetColor();
        }

    }
}
