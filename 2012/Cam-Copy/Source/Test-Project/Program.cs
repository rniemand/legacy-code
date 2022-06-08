using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using CamCopy.Svc;
using Rn.Core.Configuration;
using Rn.Core.Logging;

namespace CamCopy.Test
{
    class Program
    {
        private static List<CamDumpFolder> _watchDirs;

        static void Main(string[] args)
        {
            var svc = new CamCopySvc();
            svc.SvcStart();

            Console.WriteLine("All Done");
            Console.ReadLine();
        }
    }
}
