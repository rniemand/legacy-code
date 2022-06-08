using System;
using System.IO;
using System.Linq;
using NzbScraper.DAL;
using NzbScraper.Services;

namespace NzbScraper.Dev
{
    class Program
    {
        static void Main(string[] args)
        {

            var processor = new BinSearchRssProcessor("test_data.xml");
            var items = processor
                .GetItems()
                .Where(item => item.PercentageAvailable > 50)
                .Where(item => !string.IsNullOrWhiteSpace(item.MoreInfoUrl))
                .ToList();

            //foreach (var item in items)
            //{
            //    Console.WriteLine("Downloading: {0}", item.PostId);

            //    SimpleWebClient.DownloadFile(
            //        item.DownloadUrl,
            //        string.Format("{0}.nzb", item.PostId),
            //        true);
            //}



            Console.WriteLine();
            Console.WriteLine("==========================");
            Console.WriteLine("Done.");
            Console.WriteLine("==========================");
            Console.ReadLine();
        }

        static void DownloadSampleFile()
        {
            var url = "http://rss.binsearch.net/rss.php?max=250&g=alt.binaries.tv";
            string text = SimpleWebClient.DownloadPageUsingGzip(url);
            File.WriteAllText("test_data.xml", text);
        }
    }
}
