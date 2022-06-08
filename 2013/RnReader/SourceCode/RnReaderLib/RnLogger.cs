using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnReaderLib
{
    public static class RnLogger
    {

        public static void LogException(this Exception ex)
        {
            try
            {
                // todo: log this
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[ERROR] {0}", ex.Message);
                Console.ResetColor();
            }
            catch (Exception)
            {
                // Pointless
            }
        }

        public static void LogDebug(string s, params object[] replace)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("[DEBUG] {0}", replace.Length > 0 ? String.Format(s, replace) : s);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public static void LogInfo(string s, params object[] replace)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("[INFO] {0}", replace.Length > 0 ? String.Format(s, replace) : s);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public static void LogWarn(string s, params object[] replace)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("[WARN] {0}", replace.Length > 0 ? String.Format(s, replace) : s);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public static void LogError(string s, params object[] replace)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR] {0}", replace.Length > 0 ? String.Format(s, replace) : s);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

    }
}
