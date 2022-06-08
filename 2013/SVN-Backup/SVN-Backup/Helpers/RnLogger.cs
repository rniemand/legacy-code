using System;

namespace SVN_Backup.Helpers
{
    public static class RnLogger
    {

        public static void LogException(this Exception ex, int eventId = 100)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[!!] {0}", ex.Message);
                Console.ResetColor();
            }
            catch (Exception)
            {
                // Pointless
            }
        }

        public static void LogError(string msg, params object[] replace)
        {
            try
            {
                if (replace.Length > 0)
                    msg = String.Format(msg, replace);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR] {0}", msg);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public static void LogWarning(string msg, params object[] replace)
        {
            try
            {
                if (replace.Length > 0)
                    msg = String.Format(msg, replace);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[WARN] {0}", msg);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public static void LogInfo(string msg, params object[] replace)
        {
            try
            {
                if (replace.Length > 0)
                    msg = String.Format(msg, replace);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[INFO] {0}", msg);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public static void LogDebug(string msg, params object[] replace)
        {
            try
            {
                if (replace.Length > 0)
                    msg = String.Format(msg, replace);

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("[DEBUG] {0}", msg);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

    }
}