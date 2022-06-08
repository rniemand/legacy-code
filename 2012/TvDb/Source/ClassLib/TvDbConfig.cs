using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rn.Core.Configuration;

namespace Rn.TvDb
{
    public class TvDbConfig
    {
        public string ApiKey { get; set; }
        public string Language { get; set; }
        public TvDBMirror XmlMirror { get; set; }
        public TvDBMirror BannerMirror { get; set; }
        public TvDBMirror ZipMirror { get; set; }

        // Folders
        public string DirCache { get; set; }
        public string DirShowInfos { get; set; }
        public string DirShowThumbs { get; set; }
        public string DirShowBanners { get; set; }
    }
}
