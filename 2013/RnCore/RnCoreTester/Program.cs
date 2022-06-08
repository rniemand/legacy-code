using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RnCore.Config;
using RnCore.Logging;

namespace RnCoreTester
{
    class Program
    {
        static void Main(string[] args)
        {
            RnConfig.Instance.LoadConfig("./RnConfig.xml");
            RnConfig.DefaultConfig.RegisterLoggers();

            RnLogger.Loggers.LogDebug("Hello World", 100);


            Console.WriteLine("All Done, press ENTER to quit");
            Console.ReadLine();
        }
    }
}
