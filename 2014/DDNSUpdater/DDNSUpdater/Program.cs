using System;
using RnDdns.Service;

namespace RnDdns.Dev
{
    class Program
    {
        static void Main(string[] args)
        {
            var svc = new DdnsUpdateService();
            svc.StartService();

            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
