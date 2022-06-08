using System;
using System.Net;
using Rn.Core.Configuration;
using Rn.Core.Logging;
using Rn.Core.Helpers;

namespace Rn.TvDb.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.LoadXmlConfig("RnConfig");
            Config.GetXmlConfig().CreateLoggers();

            
            var tdb = new TVDB(Config.GetXmlConfig());

            //var results = tdb.RunSearch("futurama"); // 73871
            //tdb.GetSearchResultInfo(results[0]);

            //var actors = tdb.GetActors(73871);
            var banners = tdb.GetBanners(73871);
            var episodes = tdb.GetEpisodes(73871, 1);

            foreach (TvDbBanner b in banners)
            {
                b.DownloadBanner();
            }


            Console.WriteLine("");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("And thats all folks");
            Console.ReadLine();
        }
    }
}
