using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Core.Helpers;
using Rn.Core.Logging;

namespace Rn.TvDb
{
    public class TvdbSearchResult
    {
        public int SeriesId { get; private set; }
        public string Language { get; private set; }
        public string SeriesName { get; private set; }
        public string Banner { get; private set; }
        public string Overview { get; private set; }
        public DateTime FirstAired { get; private set; }
        public string ImdbId { get; private set; }
        public string Zap2ItId { get; private set; }
        public int Id { get; private set; }

        public TvdbSearchResult(XmlNode n)
        {
            SeriesId = n.GetNodeValueInt("seriesid");
            Language = n.GetNodeValue("language");
            SeriesName = n.GetNodeValue("SeriesName");
            Banner = n.GetNodeValue("banner");
            Overview = n.GetNodeValue("Overview");
            ImdbId = n.GetNodeValue("IMDB_ID");
            Zap2ItId = n.GetNodeValue("zap2it_id");
            Id = n.GetNodeValueInt("id");
            FirstAired = n.GetNodeValueDateTime("FirstAired", "yyyy-MM-dd");
            Locale.LogEvent("rn.core.common", "0003", "TvdbSearchResult");
        }
    }
}
