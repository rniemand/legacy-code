using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Core.Helpers;
using Rn.Core.Logging;

namespace Rn.TvDb
{
    public class TvDbBanner
    {
        public TvDbConfig Config { get; private set; }
        public int Id { get; private set; }
        public string BannerPath { get; private set; }
        // todo - make this a enum
        public string BannerType { get; private set; }
        // todo - make this an enum
        public string BannerType2 { get; private set; }
        public string[] Colors { get; private set; }
        public string Language { get; private set; }
        public double Rating { get; private set; }
        public int RatingCount { get; private set; }
        public bool SeriesName { get; private set; }
        public string ThumbnailPath { get; private set; }
        public string VignettePath { get; private set; }

        public int ShowId { get; private set; }


        public TvDbBanner(XmlNode n, TvDbConfig config, int showId)
        {
            Config = config;
            ShowId = showId;

            Id = n.GetNodeValueInt("id");
            BannerPath = n.GetNodeValue("BannerPath");
            BannerType = n.GetNodeValue("BannerType");
            BannerType2 = n.GetNodeValue("BannerType2");
            Colors = n.GetNodeValue("Colors").Split('|');
            Language = n.GetNodeValue("Language");
            RatingCount = n.GetNodeValueInt("RatingCount");
            if (n.HasNodeValue("SeriesName"))
                SeriesName = n.GetNodeValueBool("SeriesName");
            ThumbnailPath = n.GetNodeValue("ThumbnailPath");
            VignettePath = n.GetNodeValue("VignettePath");
            if (n.HasNodeValue("Rating"))
                Rating = n.GetNodeValueDouble("Rating");

            SetFullPaths();
            Locale.LogEvent("rn.core.common", "0003", "TvDbBanner");
        }

        private void SetFullPaths()
        {
            BannerPath = String.Format("{0}banners/{1}", Config.BannerMirror.Url, BannerPath);
        }

        public string GetBannerZipPath()
        {
            return String.Format(@"{0}{1}-{2}.zip", Config.DirShowBanners, ShowId, BannerType);
        }

        public void DownloadBanner(bool forceUpdate = false)
        {
            var fileExt = IOHelper.GetExtension(BannerPath);
            var nameInZip = String.Format(@"{0}\{1}.{2}", BannerType2, Id, fileExt);
            ZipHelper.AddFileFromUrl(GetBannerZipPath(), BannerPath, nameInZip, forceUpdate);
        }

    }
}
