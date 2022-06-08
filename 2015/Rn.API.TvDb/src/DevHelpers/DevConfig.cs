using System;
using System.IO;

namespace DevHelpers
{
    public class DevConfig
    {
        private static readonly Lazy<DevConfig> Lazy = new Lazy<DevConfig>(() => new DevConfig());
        
        public static DevConfig Instance { get { return Lazy.Value; } }

        public string ApiKey { get; set; }
        public string MirrorPath { get; set; }
        public string XmlMirror { get; set; }
        public string BannerMirror { get; set; }
        public string ZipMirror { get; set; }

        private DevConfig()
        {
            // this is presuming you followed the comments found in
            // the TVDB_DEV_KEY.sample file
            ApiKey = File.ReadAllText("./../../../TVDB_DEV_KEY");

            // todo: load this dynamically
            // Use the default mirror path for now
            MirrorPath = "http://thetvdb.com";

            // todo: dynamically set these
            // Configure the individual mirror paths
            XmlMirror = MirrorPath;
            BannerMirror = MirrorPath;
            ZipMirror = MirrorPath;
        }
    }
}
