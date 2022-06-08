using System;
using System.IO;
using System.Text.RegularExpressions;
using Moq;
using Rn.API.ISBNFinder;
using Rn.API.ISBNFinder.Enums;
using Rn.Common.Interfaces;
using RnDev.ISBNFinder.MockResponses;
using Match = System.Text.RegularExpressions.Match;

namespace RnDev.ISBNFinder.DevConsole
{
    class CachedResponse
    {
        public bool Enabled { get; set; }
        public string Url { get; set; }
        public string ResponseType { get; set; }
        public bool UseRx { get; set; }
        public string Body { get; set; }

        public void SetProperty(string key, string value)
        {
            switch (key.ToLower().Trim())
            {
                case "enabled":
                    Enabled = bool.Parse(value);
                    return;
                case "url":
                    Url = value;
                    return;
                case "userx":
                    UseRx = bool.Parse(value);
                    return;
                case "response_type":
                    ResponseType = value;
                    return;
                default:
                    // todo: log
                    System.Console.WriteLine("UNHANDLED :: {0}", key);
                    return;
            }
        }

        public void SetBody(string body)
        {
            var rx = "\\{\\|((.|\\r|\\n|\\r\\n)*?)\\|\\}.*?$(\\r|\\n|\\r\\n|)";
            Body = Regex.Replace(body, rx, "", RegexOptions.Multiline);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var webClient = new Mock<IWebClient>();

            SetupReaponses(webClient, LookupApi.ISBNDB);

            var finder = new BookFinder(webClient.Object);

            finder.Find("9781406328073");

            


            /*
             * 9781845394417        - THE YAWNING GAME.
             * 9781406328073        - The Bravest Ever Bear
             * 9781842393185        - [MIXED] Reynard's Wonderful Plan
             * 9780099404675        - Eat Your Peas
             */

            System.Console.WriteLine();
            System.Console.WriteLine("============================");
            System.Console.WriteLine("= DONE");
            System.Console.WriteLine("============================");
            System.Console.ReadKey();
        }

        static void SetupReaponses(Mock<IWebClient> mockClient, LookupApi selectedApi)
        {
            var responseDir = ResourcePaths.GetCachePath(selectedApi);
            if (!Directory.Exists(responseDir))
            {
                // todo: better logging
                throw new Exception(string.Format("Cannot find '{0}'", responseDir));
            }

            var di = new DirectoryInfo(responseDir);
            var files = di.GetFiles("*.txt");

            if (files.Length <= 0)
            {
                // todo: better logging
                throw new Exception(string.Format("No files ound in '{0}'", responseDir));
            }

            foreach (FileInfo file in files)
            {
                RegisterWebClientResponse(mockClient, file.FullName);
                return;
                // todo: allow more than 1 file loading
            }
        }

        static void RegisterWebClientResponse(Mock<IWebClient> mockClient, string filePath)
        {
            var fileContents = File.ReadAllText(filePath);
            var rxp = "\\{\\|((.|\\r|\\n|\\r\\n)*?)\\|\\}";
            var configSection = Regex.Match(fileContents, rxp, RegexOptions.Multiline);

            if (configSection.Length <= 0)
            {
                // todo: better logging
                throw new Exception("FIX ME");
            }

            var linesRx = "^(.*?)=(.*?)$";
            var lines = Regex.Matches(configSection.Groups[1].Value, linesRx, RegexOptions.Multiline);

            if (lines.Count <= 0)
            {
                // todo: better logging
                throw new Exception("FIX ME");
            }

            var resp = new CachedResponse();

            foreach (Match line in lines)
            {
                var key = line.Groups[1].Value.ToLower().Trim();
                var value = line.Groups[2].Value.Trim();
                resp.SetProperty(key, value);
            }

            resp.SetBody(fileContents);


            mockClient.Setup(x => x.DownloadAsString(It.IsAny<string>()))
                .Returns(File.ReadAllText(filePath));
        }
    }
}
