using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using MfLib.Providers.MusicBrainz;
using RnCore.Helpers;

namespace MfLib.Providers
{
    public static class MusicBrainzHelper
    {
        public static string BaseUrl { get; private set; }

        static MusicBrainzHelper()
        {
            BaseUrl = "http://musicbrainz.org/ws/2";
        }


        private static XmlDocument FetchWebXml(string url, bool debugToFile = false, string filePath = "./currentXml.xml")
        {
            try
            {
                RnLocale.LogEvent("mf", "mf.0012", url);
                
                // stupid hack for missing NameSpaces
                var doc = new XmlDocument();
                var content = RnWeb.GetUrl(url);

                // todo - LATER - use rx
                content = content.Replace("xmlns=\"http://musicbrainz.org/ns/mmd-2.0#\"", "");
                content = content.Replace("xmlns:ext=\"http://musicbrainz.org/ns/ext#-2.0\"", "");
                content = content.Replace("ext:", "");
                doc.LoadXml(content);

                if (debugToFile)
                    RnIO.WriteAllText(filePath, content);

                return doc;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return null;
            }
        }

        private static void DumpToXml(XmlDocument x)
        {
            File.WriteAllText(@"c:\Current.xml", x.OuterXml);
        }


        // =====================================================================
        // Artists
        // =====================================================================
        public static List<MbArtist> SearchArtist(string artistName)
        {
            var artists = new List<MbArtist>();

            try
            {
                // Run the search on MusicBrainz
                var url = String.Format("{0}/artist/?query=artist:{1}", BaseUrl, HttpUtility.UrlEncode(artistName));
                var xmlDoc = FetchWebXml(url);
                if (xmlDoc == null)
                {
                    RnLocale.LogEvent("mf", "mf.0001", artistName, url);
                    return artists;
                }

                // Get the artists
                var nodes = xmlDoc.SelectNodes("/metadata/artist-list/artist");
                if (nodes == null || nodes.Count == 0)
                {
                    RnLocale.LogEvent("mf", "mf.0001", artistName, url);
                    return artists;
                }

                artists.AddRange(from XmlNode n in nodes select new MbArtist(n));
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }

            return artists;
        }

        public static MbArtist GetBestMatchingArtist(string artistName)
        {
            MbArtist bestMatch = null;

            try
            {
                var artists = SearchArtist(artistName);
                if (artists.Count == 0) return null;

                foreach (var a in artists)
                {
                    if (bestMatch == null)
                        bestMatch = a;
                    if (a.Score > bestMatch.Score)
                        bestMatch = a;
                }
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }

            return bestMatch;
        }


        // =====================================================================
        // Releases
        // =====================================================================
        public static List<MbRelease> GetReleases(string artistId, List<MbRelease> releases = null)
        {
            if (releases == null)
                releases = new List<MbRelease>();
            
            try
            {
                // Fetch the release information from MusicBrainz
                var url = String.Format("{0}/artist/{1}?inc=releases", BaseUrl, artistId);
                var xml = FetchWebXml(url, true);
                if (xml == null) return releases;

                // Ensure that we have something to work with here
                var nodes = xml.SelectNodes("/metadata/artist/release-list/release");
                if (nodes == null || nodes.Count == 0)
                {
                    RnLocale.LogEvent("mf", "mf.0013", artistId);
                    return releases;
                }

                // Append the results...
                releases.AddRange(from XmlNode n in nodes select new MbRelease(n, artistId));
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }

            return releases;
        }

        public static MbRelease GetRelease(Dictionary<string, string> filter, string artistId)
        {
            try
            {
                var url = String.Format("{0}/release/?query={1}", BaseUrl, FlattenQueryFilter(filter));
                var xml = FetchWebXml(url);
                var nodes = xml.SelectNodes("/metadata/release-list/release[@score='100']");

                if (nodes == null || nodes.Count == 0)
                {
                    // todo: add to logger
                    Console.WriteLine("Could not find a match");
                    return null;
                }

                return new MbRelease(nodes[0], artistId);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return null;
            }
        }


        // =====================================================================
        // Recordings
        // =====================================================================
        public static List<MbRecording> GetTracksForRelease(string reId, List<MbRecording> recordings = null, bool save = false)
        {
            if (recordings == null)
                recordings = new List<MbRecording>();

            try
            {
                // http://www.musicbrainz.org/ws/2/recording/?query=reid:105b9bf5-015b-44ff-9a9c-d743f3d08206
                var url = String.Format("{0}/recording/?query=reid:{1}", BaseUrl, reId);
                var xml = FetchWebXml(url);
                if (xml == null) return recordings;

                const string xpath = "/metadata/recording-list/recording";
                var nodes = xml.SelectNodes(xpath);
                if (nodes == null || nodes.Count == 0)
                    return recordings;

                recordings.AddRange(from XmlNode n in nodes select new MbRecording(n, reId));
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }

            return recordings;
        }




        public static string FlattenQueryFilter(Dictionary<string, string> filter)
        {
            try
            {
                var count = 0;
                var sb = new StringBuilder();

                foreach (var p in filter)
                {
                    if (count != 0) sb.Append(" AND ");
                    sb.Append(String.Format("{0}:{1}", p.Key, p.Value));
                    count++;
                }

                return HttpUtility.UrlEncode(sb.ToString());
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return string.Empty;
            }
        }

    }
}
