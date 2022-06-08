using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Core.Configuration;
using Rn.Core.Helpers;
using Rn.Core.Logging;

namespace Rn.TvDb
{
    public class TVDB
    {
        public TvDbConfig Config { get; internal set; }
        private readonly List<TvDBMirror> _mirrors;
        private readonly XmlConfig _config;

        public TVDB(XmlConfig config)
        {
            Config = new TvDbConfig();
            _config = config;
            _mirrors = new List<TvDBMirror>();

            Config.ApiKey = _config.GetConfigKey("TVDB.API.Key");
            Config.Language = _config.GetConfigKey("TVDB.Language");
            
            Config.DirCache = config.GetConfigKey("TVDB.CacheDir");
            Config.DirShowInfos = String.Format(@"{0}show-infos\{1}\", Config.DirCache, Config.Language);
            Config.DirShowThumbs = String.Format(@"{0}show-thumbs\{1}\", Config.DirCache, Config.Language);
            Config.DirShowBanners = String.Format(@"{0}show-banners\{1}\", Config.DirCache, Config.Language);
            
            IOHelper.CreateDir(Config.DirCache);
            IOHelper.CreateDir(Config.DirShowInfos);
            IOHelper.CreateDir(Config.DirShowThumbs);
            IOHelper.CreateDir(Config.DirShowBanners);
            
            //RefreshMirrors();
        }


        public void RefreshMirrors()
        {
            // todo - set mirror attributes based on the following
            // http://thetvdb.com/wiki/index.php/API:mirrors.xml

            try
            {
                _mirrors.Clear();
                Config.XmlMirror = null;
                Config.BannerMirror = null;
                Config.ZipMirror = null;

                var url = String.Format("http://www.thetvdb.com/api/{0}/mirrors.xml", Config.ApiKey);
                var mirrorXml = WebHelper.GetUrlAsXml(url);
                var mirrors = mirrorXml.SelectNodes("/Mirrors/Mirror");

                if (mirrors == null || mirrors.Count == 0)
                {
                    Locale.LogEvent("tvdb", "0002");
                    return;
                }

                foreach (XmlNode n in mirrors)
                    _mirrors.Add(new TvDBMirror(n));

                Config.XmlMirror = _mirrors[0];
                Config.BannerMirror = _mirrors[0];
                Config.ZipMirror = _mirrors[0];

                Logger.LogWarning(_mirrors[0].Url);

                Locale.LogEvent("tvdb", "0001", mirrors.Count);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }
        }

        public List<TvdbSearchResult> RunSearch(string showName)
        {
            var results = new List<TvdbSearchResult>();

            try
            {
                var url = String.Format("http://www.thetvdb.com/api/GetSeries.php?seriesname={0}", showName);
                var xml = WebHelper.GetUrlAsXml(url);

                if (xml == null)
                {
                    Locale.LogEvent("tvdb", "0004", showName);
                    return results;
                }

                var rNodes = xml.SelectNodes("/Data/Series");
                if (rNodes == null || rNodes.Count == 0)
                    return results;

                results.AddRange(from XmlNode n in rNodes select new TvdbSearchResult(n));
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "002", ex.Message);
            }

            return results;
        }

        public bool DownloadShowInfo(TvdbSearchResult result)
        {
            try
            {
                var url = String.Format(
                    "{0}api/{1}/series/{2}/all/{3}.zip",
                    Config.ZipMirror.Url, Config.ApiKey, result.Id, Config.Language);
                var destination = String.Format("{0}{1}.zip", Config.DirShowInfos, result.Id);

                // todo - add a config entry to update this show after a certain time period
                WebHelper.DownloadFile(url, destination);
                return true;
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return false;
            }
        }

        public List<TvDbActor> GetActors(int showId)
        {
            var actors = new List<TvDbActor>();

            try
            {
                var zipPath = String.Format("{0}{1}.zip", Config.DirShowInfos, showId);
                var xml = new XmlDocument();
                xml.LoadXml(ZipHelper.ExtractFile(zipPath, "actors.xml"));
                var nodes = xml.SelectNodes("/Actors/Actor");

                if (nodes == null || nodes.Count == 0)
                {
                    Locale.LogEvent("tvdb", "0005", showId, Config.Language);
                    return actors;
                }

                actors.AddRange(from XmlNode n in nodes select new TvDbActor(n));
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "002", ex.Message);
            }

            return actors;
        }

        public List<TvDbBanner> GetBanners(int showId)
        {
            var banners = new List<TvDbBanner>();

            try
            {
                var zipPath = String.Format("{0}{1}.zip", Config.DirShowInfos, showId);
                var xml = new XmlDocument();
                xml.LoadXml(ZipHelper.ExtractFile(zipPath, "banners.xml"));
                var nodes = xml.SelectNodes("/Banners/Banner");

                if (nodes == null || nodes.Count == 0)
                {
                    Locale.LogEvent("tvdb", "0006", showId, Config.Language);
                    return banners;
                }

                banners.AddRange(from XmlNode n in nodes select new TvDbBanner(n, Config, showId));
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "002", ex.Message);
            }

            return banners;
        }

        public TvDbShow GetShowInfo(TvdbSearchResult result)
        {
            try
            {
                DownloadShowInfo(result);
                var zipPath = String.Format("{0}{1}.zip", Config.DirShowInfos, result.Id);
                var xml = new XmlDocument();
                xml.LoadXml(ZipHelper.ExtractFile(zipPath, String.Format("{0}.xml", Config.Language)));
                var node = xml.SelectSingleNode("/Data/Series");

                if (node == null)
                {
                    Locale.LogEvent("tvdb", "0009", zipPath);
                    return new TvDbShow();
                }

                return new TvDbShow(node, Config);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return new TvDbShow();
            }
        }


        public List<TvDbEpisode> GetEpisodes(int showId)
        {
            var episodes = new List<TvDbEpisode>();

            try
            {
                var zipPath = String.Format("{0}{1}.zip", Config.DirShowInfos, showId);
                var xml = new XmlDocument();
                xml.LoadXml(ZipHelper.ExtractFile(zipPath, String.Format("{0}.xml", Config.Language)));
                var nodes = xml.SelectNodes("/Data/Episode");

                if (nodes == null || nodes.Count == 0)
                {
                    Locale.LogEvent("tvdb", "0007", showId, Config.Language);
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

        public List<TvDbEpisode> GetEpisodes(int showId, int seasonNo)
        {
            var episodes = new List<TvDbEpisode>();

            try
            {
                var zipPath = String.Format("{0}{1}.zip", Config.DirShowInfos, showId);
                var xml = new XmlDocument();
                xml.LoadXml(ZipHelper.ExtractFile(zipPath, String.Format("{0}.xml", Config.Language)));
                var nodes = xml.SelectNodes(String.Format("/Data/Episode[SeasonNumber = '{0}']", seasonNo));

                if (nodes == null || nodes.Count == 0)
                {
                    Locale.LogEvent("tvdb", "0008", seasonNo, showId, Config.Language);
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
