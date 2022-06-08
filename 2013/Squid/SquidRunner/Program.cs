using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RnCore.Config;

namespace SquidRunner
{
    // http://wiki.squid-cache.org/SquidFaq/SquidLogs#store.log

    class Program
    {
        static void Main(string[] args)
        {
            RnConfig.Instance.LoadConfig("./RnConfig.xml");
            RnConfig.DefaultConfig.RegisterLoggers();

            var reader = new AccessLogReader(@"\\192.168.0.5\d$\squid\var\logs\access.log");

            Console.ReadLine();


            const string logFileDir = @"\\192.168.0.5\d$\squid\var\logs\";
            var fsw = new FileSystemWatcher(logFileDir)
                {
                    Filter = "*.*",
                    IncludeSubdirectories = false,
                    NotifyFilter = NotifyFilters.Size
                };


            fsw.Changed += fsw_Changed;
            fsw.Created += fsw_Created;
            fsw.Deleted += fsw_Deleted;
            fsw.Renamed += fsw_Renamed;


            // RXP: ^([^\s]+).*?([^\s]+).*?([^\s]+).*?([^\s]+).*?([^\s]+).*?([^\s]+).*?([^\s]+).*?([^\s]+).*?([^\s]+).*?([^\s]+)$

            // http://www.linofee.org/~jel/proxy/Squid/accesslog.shtmlhttp://www.linofee.org/~jel/proxy/Squid/accesslog.shtml
            // 1362595295.078   244000      192.168.0.1     TCP_MISS/200        70493   CONNECT     drive.google.com:443                                                        -       DIRECT/74.125.233.39    -
            // 1362595377.506   2897        192.168.0.1     TCP_MISS/200        29741   GET         http://trafficserver.apache.org/images/admin/netscape_common_format.jpg     -       DIRECT/192.87.106.229   image/jpeg
            // 1362595826.402   342         192.168.0.9     TCP_REFRESH_HIT/304 494     GET         http://i.imgur.com/mC5VeAvb.jpg                                             -       DIRECT/93.184.215.248   -
            // Timestamp        Elapsed     Client          Action/Code         Size    Method      URI                                                                         Ident   Hierarchy/From          Content






            fsw.EnableRaisingEvents = true;



            Console.WriteLine();
            Console.WriteLine("All Done");
            Console.ReadLine();
        }

        static void fsw_Renamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine("renamed");
        }

        static void fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("Log file deleted");
        }

        static void fsw_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("Log file created");
        }

        static void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.FullPath);
            Console.WriteLine("Log file changed");
        }
    }
}
