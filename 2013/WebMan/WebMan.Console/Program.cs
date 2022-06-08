using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using Rn.WebManLib.Utils;
using RnCore.Config;
using RnCore.Helpers;
using RnCore.Logging;
using RnCore.Utils;

namespace Rn.WebMan.ConsoleApp
{
    class Program
    {
        public static string BaseUrl { get; private set; }

        static void Main(string[] args)
        {
            // http://www.techbubbles.com/wcf/hosting-the-wcf-service-in-net-executable/

            // Setup basic environment we need to run
            RnConfig.Instance.LoadConfig("./RnConfig.xml");
            RnLogger.Loggers.RegisterConfigLoggers();
            ApiUsers.Instance.LoadApiXmlFile(String.Format("{0}ApiKeys.xml", RnIO.BasePath));


            // Generate the services base URL
            BaseUrl = String.Format("http://{0}:8080/{{0}}/", GetIpv4Address());
            RunServices();


            // Wait to be closed
            Console.WriteLine("Application is now hosted...");
            Console.WriteLine("Press ENTER to quit");
            Console.ReadLine();
        }


        

        private static void RunServices()
        {
            Console.Clear();
            Console.WriteLine("Attempting to start on: {0}", BaseUrl);

            HostServices();
            HostFileBrowser();
            HostEventLog();
        }

        private static void HostServices()
        {
            var uri = new Uri(String.Format(BaseUrl, "Services"));
            var host = new ServiceHost(typeof(Rn.WebManLib.Services), uri);
            var mdb = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true
            };
            host.Description.Behaviors.Add(mdb);
            //host.AddDefaultEndpoints();
            //host.AddServiceEndpoint(typeof(IStockService), new BasicHttpBinding(), "");
            //var endpoint = new ServiceEndpoint()
            //host.AddServiceEndpoint()
            host.Open();
        }

        private static void HostFileBrowser()
        {
            var uri = new Uri(String.Format(BaseUrl, "FileBrowser"));
            var host = new ServiceHost(typeof(Rn.WebManLib.FileBrowser), uri);
            var mdb = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true
            };
            host.Description.Behaviors.Add(mdb);
            //host.AddDefaultEndpoints();
            //host.AddServiceEndpoint(typeof(IStockService), new BasicHttpBinding(), "");
            //var endpoint = new ServiceEndpoint()
            //host.AddServiceEndpoint()
            host.Open();
        }

        private static void HostEventLog()
        {
            var uri = new Uri(String.Format(BaseUrl, "EventLog"));
            var host = new ServiceHost(typeof(Rn.WebManLib.WmEventLog), uri);
            var mdb = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true
            };
            host.Description.Behaviors.Add(mdb);
            //host.AddDefaultEndpoints();
            //host.AddServiceEndpoint(typeof(IStockService), new BasicHttpBinding(), "");
            //var endpoint = new ServiceEndpoint()
            //host.AddServiceEndpoint()
            host.Open();
        }

        private static string GetIpv4Address()
        {
            try
            {
                var addresses = Dns.GetHostAddresses(Dns.GetHostName());

                foreach (var a in addresses.Where(a => a.AddressFamily == AddressFamily.InterNetwork))
                    return a.ToString();

                return "127.0.0.1";
            }
            catch (Exception ex)
            {
                ex.LogException();
                return "127.0.0.1";
            }
        }


    }
}
