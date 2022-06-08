using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SyncLib;

namespace SyncToolTester
{
    class Program
    {
        static void Main(string[] args)
        {

            SyncConfig.GetActiveSyncs();


            Console.WriteLine("Completed");
            Console.ReadKey();
        }
    }
}
