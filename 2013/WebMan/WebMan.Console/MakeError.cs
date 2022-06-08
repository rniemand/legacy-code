using System;
using RnCore.Logging;

namespace Rn.WebMan.ConsoleApp
{
    class MakeError
    {
        public static void DOIt()
        {
            try
            {
                RnLogger.Loggers.LogDebug("Calling this before we die");
                int.Parse("ds");
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
    }
}
