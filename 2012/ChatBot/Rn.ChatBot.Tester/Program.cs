using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rn.ChatBot.Service;
using Rn.Core.Configuration;
using Rn.Core.Logging;
using Rn.Core.Helpers;

namespace Rn.ChatBot.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var svc = new ChatBotSvc();
            svc.StartService();


            Console.WriteLine("--- Chat bot is now running ---");
            Console.ReadKey();
        }
    }
}
