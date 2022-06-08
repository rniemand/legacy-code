using System.ServiceProcess;
using RnDDNS;

namespace RnDdns.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new DdnsUpdateService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
