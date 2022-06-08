using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Xml;
using Rn.Core.Helpers;
using Rn.Core.Logging;

namespace Rn.TvDb
{
    public class TvDbEpisode
    {
        public TvDbConfig Config { get; private set; }
        public int Id { get; private set; }
        public double CombinedEpisodeNumber { get; private set; }
        public int CombinedSeason { get; private set; }
        public int DvdChapter { get; private set; }
        public int DvdDiscId { get; private set; }
        public double DvdEpisodeNumber { get; private set; }
        public int DvdSeason { get; private set; }
        public string Director { get; private set; }
        public int EpImgFlag { get; private set; }
        public string EpisodeName { get; private set; }
        public int EpisodeNumber { get; private set; }
        public DateTime FirstAired { get; private set; }
        public string[] GuestStars { get; private set; }
        public string ImdbId { get; private set; }
        public string Language { get; private set; }
        public string Overview { get; private set; }
        public string ProductionCode { get; private set; }
        public double Rating { get; private set; }
        public int RatingCount { get; private set; }
        public int SeasonNumber { get; private set; }
        public string[] Writers { get; private set; }
        public int AbsoluteNumber { get; private set; }
        public int AirsAfterSeason { get; private set; }
        public int AirsBeforeEpisode { get; private set; }
        public int AirsBeforeSeason { get; private set; }
        public string EpisodeImageUrl { get; private set; }
        // todo - cast this to a date time
        public string LastUpdated { get; private set; }
        public int SeasonId { get; private set; }
        public int SeriesId { get; private set; }

        public string SeasonPadded { get; private set; }
        public string EpisodePadded { get; private set; }
        public string UrlEpisodeInfo { get; private set; }
        public string DisplayName { get; private set; }

        public TvDbEpisode(XmlNode n, TvDbConfig config)
        {
            Config = config;

            Id = n.GetNodeValueInt("id");
            CombinedEpisodeNumber = n.GetNodeValueDouble("Combined_episodenumber");
            DvdChapter = n.GetNodeValueInt("DVD_chapter");
            DvdDiscId = n.GetNodeValueInt("DVD_discid");
            DvdEpisodeNumber = n.GetNodeValueDouble("DVD_episodenumber");
            DvdSeason = n.GetNodeValueInt("DVD_season");
            Director = n.GetNodeValue("Director");
            EpImgFlag = n.GetNodeValueInt("EpImgFlag");
            EpisodeName = n.GetNodeValue("EpisodeName");
            EpisodeNumber = n.GetNodeValueInt("EpisodeNumber");
            FirstAired = n.GetNodeValueDateTime("FirstAired", "yyyy-MM-dd");
            GuestStars = n.GetNodeValue("GuestStars").Split(',');
            ImdbId = n.GetNodeValue("IMDB_ID");
            Language = n.GetNodeValue("Language");
            Overview = n.GetNodeValue("Overview");
            ProductionCode = n.GetNodeValue("ProductionCode");
            Rating = n.GetNodeValueDouble("Rating");
            RatingCount = n.GetNodeValueInt("RatingCount");
            SeasonNumber = n.GetNodeValueInt("SeasonNumber");
            Writers = n.GetNodeValue("Writer").Split(',');
            AbsoluteNumber = n.GetNodeValueInt("absolute_number");
            AirsAfterSeason = n.GetNodeValueInt("airsafter_season");
            AirsBeforeEpisode = n.GetNodeValueInt("airsbefore_episode");
            AirsBeforeSeason = n.GetNodeValueInt("airsbefore_season");
            EpisodeImageUrl = n.GetNodeValue("filename");
            LastUpdated = n.GetNodeValue("lastupdated");
            SeasonId = n.GetNodeValueInt("seasonid");
            SeriesId = n.GetNodeValueInt("seriesid");
            
            SetPaths();
            SetDisplayName();

            SeasonPadded = SeasonNumber.PadNumber();
            EpisodePadded = EpisodeNumber.PadNumber();

            Locale.LogEvent("rn.core.common", "0003",
                            String.Format("TvDbEpisode ({0}x{1})", SeasonNumber, EpisodeNumber));
        }

        private void SetPaths()
        {
            EpisodeImageUrl = String.Format("{0}banners/{1}", Config.BannerMirror.Url, EpisodeImageUrl);
            UrlEpisodeInfo = String.Format("{0}api/{1}/episodes/{2}/{3}.xml", Config.BannerMirror.Url, Config.ApiKey, Id,
                                           Language);
        }

        private void SetDisplayName()
        {
            DisplayName = String.Format("{0}x{1}", SeasonNumber.PadNumber(), EpisodeNumber.PadNumber());
        }



        public string GetZipPath()
        {
            return String.Format(@"{0}{1}.{2}.zip", Config.DirShowThumbs, SeriesId, SeasonNumber);
        }

        public void DownloadThumb(bool forceUpdate = false)
        {
            ZipHelper.AddFileFromUrl(GetZipPath(), EpisodeImageUrl, GetThumbName(), forceUpdate);
        }

        public string GetThumbName()
        {
            var fileExt = IOHelper.GetExtension(EpisodeImageUrl);
            return String.Format("{0}x{1}.{2}", SeasonPadded, EpisodePadded, fileExt);
        }

        public bool HasThumbnail()
        {
            return ZipHelper.ContainsEntry(GetZipPath(), GetThumbName());
        }

        public Stream GetThumbnailStream()
        {
            if (!HasThumbnail()) DownloadThumb();
            return ZipHelper.ExtractFileStream(GetZipPath(), GetThumbName());
        }

    }
}
