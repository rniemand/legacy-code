using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Core.Helpers;
using Rn.Core.Logging;

namespace Rn.TvDb
{
    public class TvDbShowSeason
    {
        public string DisplayName { get; private set; }
        public int SeriesId { get; private set; }
        public int SeasonNo { get; private set; }
        public XmlDocument ShowXml { get; private set; }
        public TvDbConfig Config { get; private set; }

        public TvDbShowSeason(int showId, int seasonNo, XmlDocument xml, TvDbConfig config)
        {
            SeriesId = showId;
            SeasonNo = seasonNo;
            ShowXml = xml;
            Config = config;
            DisplayName = String.Format("Season {0}", SeasonNo);
        }

        public List<TvDbEpisode> GetEpisodes()
        {
            var episodes = new List<TvDbEpisode>();

            try
            {
                var zipPath = String.Format("{0}{1}.zip", Config.DirShowInfos, SeriesId);
                var xml = new XmlDocument();
                xml.LoadXml(ZipHelper.ExtractFile(zipPath, String.Format("{0}.xml", Config.Language)));
                var nodes = xml.SelectNodes(String.Format("/Data/Episode[SeasonNumber = '{0}']", SeasonNo));

                if (nodes == null || nodes.Count == 0)
                {
                    Locale.LogEvent("tvdb", "0008", SeasonNo, SeriesId, Config.Language);
                    return episodes;
                }

                episodes.AddRange(from XmlNode n in nodes select new TvDbEpisode(n, Config));
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "002", ex.Message);
            }

            return episodes;
        }

    }
}
