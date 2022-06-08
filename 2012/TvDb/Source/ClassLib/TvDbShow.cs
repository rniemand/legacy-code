using System;
using System.Collections.Generic;
using System.Xml;
using Rn.Core.Helpers;
using Rn.Core.Logging;

namespace Rn.TvDb
{
    public class TvDbShow
    {
        public int Id { get; private set; }
        public string[] Actors { get; private set; }
        public string AirsDayOfWeek { get; private set; }
        public string AirsTime { get; private set; }
        public string ContentRating { get; private set; }
        public DateTime FirstAired { get; private set; }
        public string[] Genre { get; private set; }
        public string ImdbId { get; private set; }
        public string Language { get; private set; }
        public string Network { get; private set; }
        public string NetworkId { get; private set; }
        public string Overview { get; private set; }
        public double Rating { get; private set; }
        public int RatingCount { get; private set; }
        public int Runtime { get; private set; }
        public int SeriesId { get; private set; }
        public string SeriesName { get; private set; }
        // todo - make this an enum
        public string Status { get; private set; }
        public string Added { get; private set; }
        public string AddedBy { get; private set; }
        public string Banner { get; private set; }
        public string Fanart { get; private set; }
        // todo - get this back from apoch
        public string LastUpdated { get; private set; }
        public string Poster { get; private set; }
        public string Zap2ItId { get; private set; }

        public TvDbConfig Config { get; private set; }
        public bool InfoLoaded { get; private set; }
        public string ZipSeriesInfo { get; private set; }


        public TvDbShow()
        {
            InfoLoaded = false;
        }

        public TvDbShow(XmlNode n, TvDbConfig config)
        {
            Config = config;

            Id = n.GetNodeValueInt("id");
            Actors = n.GetNodeValue("Actors").Split('|');
            AirsDayOfWeek = n.GetNodeValue("Airs_DayOfWeek");
            AirsTime = n.GetNodeValue("Airs_Time");
            ContentRating = n.GetNodeValue("ContentRating");
            FirstAired = n.GetNodeValueDateTime("FirstAired", "yyyy-MM-dd");
            Genre = n.GetNodeValue("Genre").Split('|');
            ImdbId = n.GetNodeValue("IMDB_ID");
            Language = n.GetNodeValue("Language");
            Network = n.GetNodeValue("Network");
            NetworkId = n.GetNodeValue("NetworkID");
            Overview = n.GetNodeValue("Overview");
            Rating = n.GetNodeValueDouble("Rating");
            RatingCount = n.GetNodeValueInt("RatingCount");
            Runtime = n.GetNodeValueInt("Runtime");
            SeriesId = n.GetNodeValueInt("SeriesID");
            SeriesName = n.GetNodeValue("SeriesName");
            Status = n.GetNodeValue("Status");
            Added = n.GetNodeValue("added");
            AddedBy = n.GetNodeValue("addedBy");
            Banner = n.GetNodeValue("banner");
            Fanart = n.GetNodeValue("fanart");
            LastUpdated = n.GetNodeValue("lastupdated");
            Poster = n.GetNodeValue("poster");
            Zap2ItId = n.GetNodeValue("zap2it_id");
            InfoLoaded = true;
            SetPaths();

            Locale.LogEvent("rn.core.common", "0003", String.Format("TvDbShow ({0})", SeriesName));
        }



        private void SetPaths()
        {
            ZipSeriesInfo = String.Format(@"{0}{1}.zip", Config.DirShowInfos, Id);
        }


        public int GetMaxSeasonNo()
        {
            var maxSeasonNo = 1;

            try
            {
                var xml = new XmlDocument();
                xml.LoadXml(ZipHelper.ExtractFile(ZipSeriesInfo, String.Format("{0}.xml", Config.Language)));
                var nodes = xml.SelectNodes("/Data/Episode/SeasonNumber");

                if (nodes == null || nodes.Count == 0)
                    return maxSeasonNo;

                foreach (XmlNode n in nodes)
                    if (n.InnerText.AsInt() > maxSeasonNo)
                        maxSeasonNo = n.InnerText.AsInt();
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }

            return maxSeasonNo;
        }

        public List<TvDbShowSeason> GetSeasons()
        {
            var seasons = new List<TvDbShowSeason>();
            var maxSeason = GetMaxSeasonNo() + 1;
            
            var xml = new XmlDocument();
            xml.LoadXml(ZipHelper.ExtractFile(ZipSeriesInfo, String.Format("{0}.xml", Config.Language)));


            for (var i = 1; i < maxSeason; i++)
            {
                seasons.Add(new TvDbShowSeason(Id, i, xml, Config));
            }


            foreach (TvDbShowSeason s in seasons)
            {
                Logger.LogWarning(String.Format("Season: {0}", s.SeasonNo));
            }

            return seasons;
        }


    }
}
