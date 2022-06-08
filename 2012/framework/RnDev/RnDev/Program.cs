using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rn.Logging;

namespace RnDev
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.AddLogFileLogger(@"c:\Rn.Test.log", 10, 10, "default", Severity.Debug);
            
            Logger.LogError("Error Message");
            Logger.LogWarning("Warning Message");
            Logger.LogInfo("Informational Msg");
            Logger.LogDebug("Debug Message");

            Console.WriteLine("All done, press ENTER to quit!");
            Console.ReadLine();
        }
    }
}
