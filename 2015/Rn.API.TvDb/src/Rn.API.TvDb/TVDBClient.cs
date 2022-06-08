using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Rn.API.TVDB.Models;
using Rn.Common.Interfaces;
using Rn.Common.Logging;
using Rn.Common.Web;

namespace Rn.API.TVDB
{
    public class TVDBClient
    {
        private TVDBClientConfig _config;
        private readonly IWebClient _webClient;
        private readonly ILogger _logger;

        public DateTime ServerTimeAsLocal { get; set; }
        public DateTime ServerTime { get; set; }


        // http://thetvdb.com/wiki/index.php?title=Programmers_API

        public TVDBClient(TVDBClientConfig config)
            : this(config, new SimpleWebClient())
        { }

        public TVDBClient(TVDBClientConfig config, IWebClient webclient)
            : this(config, webclient, new ConsoleLogger())
        { }

        public TVDBClient(TVDBClientConfig config, IWebClient webclient, ILogger logger)
        {
            _config = config;
            _webClient = webclient;
            _logger = logger;

            RefreshServersTime();
        }

        // Public methods
        public void RefreshServersTime()
        {
            _logger.Debug("TVDBClient.RefreshServersTime() :: Refreshing servers time");

            var timeResponse = _webClient.DownloadAsString(TVDBUrls.TimeUrl);

            // todo: find better way of doing this..
            var xDoc = new XmlDocument();
            xDoc.LoadXml(timeResponse);
            var time = int.Parse(xDoc.SelectSingleNode("/Items/Time").InnerText);

            var dtTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            ServerTimeAsLocal = dtTime.AddSeconds(time).ToLocalTime();
            ServerTime = dtTime.AddSeconds(time);
        }

        public IEnumerable<TVDBSeries> FindShow(string searchTerm)
        {
            var searchResults = new List<TVDBSeries>();
            var searchUrl = string.Format(TVDBUrls.SeriesSearch, searchTerm)
                .Replace("<XML_MIRROR>", _config.XmlMirror);
            var searchResultsRaw = _webClient.DownloadAsString(searchUrl);

            var xDoc = new XmlDocument();
            xDoc.LoadXml(searchResultsRaw);
            var seriesNodeCollection = xDoc.SelectNodes("/Data/Series");

            if (seriesNodeCollection == null)
                return searchResults;

            if (seriesNodeCollection.Count == 0)
                return searchResults;

            // todo: refactor!!!!
            foreach (XmlNode node in seriesNodeCollection)
            {
                var nBannerUrl = node.SelectSingleNode("banner");
                var nFirstAired = node.SelectSingleNode("FirstAired");
                var nImdbId = node.SelectSingleNode("IMDB_ID");
                var nId = node.SelectSingleNode("id");
                var nLanguage = node.SelectSingleNode("language");
                var nSeriesName = node.SelectSingleNode("SeriesName");
                var nNetwork = node.SelectSingleNode("Network");
                var nOverview = node.SelectSingleNode("Overview");
                var nSeriesId = node.SelectSingleNode("seriesid");
                var nZap2It = node.SelectSingleNode("zap2it_id");

                var show = new TVDBSeries
                {
                    BannerUrl = nBannerUrl == null ? string.Empty : nBannerUrl.InnerText,
                    IMDBId = nImdbId == null ? string.Empty : nImdbId.InnerText,
                    Id = nId == null ? 0 : int.Parse(nId.InnerText),
                    Language = nLanguage == null ? string.Empty : nLanguage.InnerText,
                    Name = nSeriesName == null ? string.Empty : nSeriesName.InnerText,
                    Network = nNetwork == null ? string.Empty : nNetwork.InnerText,
                    Overview = nOverview == null ? string.Empty : nOverview.InnerText,
                    SeriesId = nSeriesId == null ? 0 : int.Parse(nSeriesId.InnerText),
                    Zap2itId = nZap2It == null ? string.Empty : nZap2It.InnerText
                };

                if (nFirstAired != null)
                    show.FirstAired = DateTime.Parse(nFirstAired.InnerText);

                searchResults.Add(show);
            }

            return searchResults;
        }
    }
}
