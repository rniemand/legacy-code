using System;
using Rn.Core.Configuration;
using Rn.Notifications;
using Rn.Core.Helpers;

namespace Rn.Core.TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.LoadXmlConfig("RnConfig");
            Config.GetXmlConfig().CreateLoggers();


            const string u = "...";
            var n = new Pushover("...");


            n.SendMessage(u, "Someone is watching you", "Look Behind You");


            Console.WriteLine("");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Working in the Rn.Core lib");
            Console.ReadLine();
        }
    }
}
