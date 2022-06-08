using System;
using DevHelpers;
using Rn.API.TVDB;
using Rn.API.TVDB.Models;

namespace RnDev.TvDB.DevConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create simple client config
            var clientConfig = new TVDBClientConfig
            {
                APIKey = DevConfig.Instance.ApiKey,
                BannerMirror = DevConfig.Instance.BannerMirror,
                XmlMirror = DevConfig.Instance.XmlMirror,
                ZipMirror = DevConfig.Instance.ZipMirror
            };

            // Spin up the client
            var client = new TVDBClient(clientConfig);
            var searchResults = client.FindShow("house");


            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("= All done.");
            Console.WriteLine("======================================");
            Console.ReadLine();
        }
    }
}
