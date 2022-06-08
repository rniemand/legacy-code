using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnCore.Logging
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

    }
}
