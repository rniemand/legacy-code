using System;
using Rn.Core.Helpers;

namespace Rn.Core.Logging.Outputs
{
    public class LogConsole : LogOutput
    {
        public LogConsole(string name, LogSeverity sev)
            : base(name, LoggerType.Console, sev)
        {
            LoggerReady = true;
        }

        private void LogConsoleMsg(string msg, LogSeverity sev, int eventId)
        {
            if (eventId == 0)
                Console.WriteLine(String.Format(
                    "[{0}] ({1}) {2} : {3}",
                    DateHelper.GetTimeString(),
                    GetFullMethodPath(4, true),
                    sev.AsString().ToUpper(),
                    msg));
            else
                Console.WriteLine(String.Format(
                    "[{0}] ({4}) ({1}) {2} : {3}",
                    DateHelper.GetTimeString(),
                    GetFullMethodPath(4, true),
                    sev.AsString().ToUpper(),
                    msg,
                    eventId));

            Console.ResetColor();
        }


        public override void LogDebug(string msg, int eventId = 0, bool fromLocale = false)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            LogConsoleMsg(msg, LogSeverity.Debug, eventId);
        }

        public override void LogInfo(string msg, int eventId = 0, bool fromLocale = false)
        {
            Console.ForegroundColor = ConsoleColor.White;
            LogConsoleMsg(msg, LogSeverity.Info, eventId);
        }

        public override void LogWarning(string msg, int eventId = 0, bool fromLocale = false)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            LogConsoleMsg(msg, LogSeverity.Warning, eventId);
        }

        public override void LogError(string msg, int eventId = 0, bool fromLocale = false)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            LogConsoleMsg(msg, LogSeverity.Error, eventId);
        }

        

    }
}
