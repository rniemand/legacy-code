using System;
using AlertProcessor.Classes;
using Rn.Configuration;
using Rn.DB.MsSql;
using Rn.Logging;

namespace AlertProcessor
{
    class Program
    {
        // todo - add the ability to customize the columns exported by the application
        // todo - add the ability to specify a configuration file to use from the command line

        static void Main(string[] args)
        {
            // Load the application configuration file
            ConfigObj.LoadConfigFile("./AppConfig.xml", "AlertProcessor");
            ConfigObj.GetConfig().CreateLoggers();
            MsSqlFactory.SetConfigName("default");
            Logger.LogInfo("Staring up the 'AlertProcessor' application.");

            // Process user defined options
            GlobalObj.LoadRxPatterns();
            GlobalObj.LoadAlertFilters();
            GlobalObj.LoadAlerts();
            GlobalObj.ProcessAlerts();
            GlobalObj.ExportAlerts();

            Console.WriteLine("All done..");
            Console.ReadLine();
        }
    }
}
