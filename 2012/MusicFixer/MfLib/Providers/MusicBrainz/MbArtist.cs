using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Xml;
using MfLib.Database;
using MfLib.Enums;
using RnCore.Db.Sqlite;
using RnCore.Helpers;

namespace MfLib.Providers.MusicBrainz
{
    public class MbArtist
    {
        public double Score { get; private set; }
        public ArtistType Type { get; private set; }
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string SortName { get; private set; }
        public string Country { get; private set; }
        public long DbRowId { get; private set; }

        
        public MbArtist(XmlNode n)
        {
            try
            {
                Score = n.GetAttrDbl("score");
                Type = ToArtistType(n.GetAttr("type"));
                Id = n.GetAttr("id");
                Name = (n.HasChildNode("name") ? n.GetChildNode("name").InnerText : "");
                SortName = (n.HasChildNode("sort-name") ? n.GetChildNode("sort-name").InnerText : "");
                Country = (n.HasChildNode("country") ? n.GetChildNode("country").InnerText : "");
                DbRowId = 0;
                // todo - LATER - extract <life-span> node - NOT urgent now...
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }

        public MbArtist(string artistsName)
        {
            // Try to load the artist from the DB
            if (artistsName.RxIsMatch(".*?-.*?-.*?-.*?-.*"))
                LoadFromDbViaId(artistsName);
            else
                LoadFromDbViaName(artistsName);
            if (DbRowId > 0) return;

            // Look for an exact match from MusicBrainz
            var artist = MusicBrainzHelper.GetBestMatchingArtist(artistsName);
            if (artist == null || artist.Score < 100)
            {
                RnLocale.LogEvent("mf", "mf.0007", artistsName);
                return;
            }

            MapArtistInfo(artist);
        }


        private void MapArtistInfo(MbArtist a)
        {
            Score = a.Score;
            Type = a.Type;
            Id = a.Id;
            Name = a.Name;
            SortName = a.SortName;
            Country = a.Country;
            DbRowId = 0;
        }

        private void MapDbRow(SQLiteDataReader r)
        {
            try
            {
                Id = r.GetString("artistId");
                Type = ToArtistType(r.GetString("artistType"));
                Name = r.GetString("artistName");
                SortName = r.GetString("artistSortName");
                Country = r.GetString("artistCountry");
                DbRowId = r.GetInt("DbRowId");
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }

        private void RefreshDbRowId()
        {
            try
            {
                var sql = MusicDb.GetXmlSpCmd("MbArtist", "get_artist_dbrowid", "1.0.0", Id);
                DbRowId = MusicDb.DbObject.GetSingleResultInt(sql);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }




        public static ArtistType ToArtistType(string s)
        {
            switch (s.ToLower().Trim())
            {
                case "group":
                    return ArtistType.Group;

                case "person":
                    return ArtistType.Person;

                default:
                    RnLocale.LogEvent("mf", "mf.0002", s);
                    return ArtistType.Unknown;
            }
        }
        
        public List<MbRelease> GetReleases()
        {
            var releases = new List<MbRelease>();

            try
            {
                // 1 - Try to get releases from DB
                var cmd = MusicDb.GetXmlSpCmd("MbArtist", "get_releases", "1.0.0", Id);
                using (var reader = MusicDb.DbObject.ExecuteReader(cmd))
                {
                    while (reader.Read())
                        releases.Add(new MbRelease(reader));
                    reader.Close();
                }

                // 2 - Try to get releases from the WWW
                if (releases.Count == 0)
                {
                    RnLocale.LogEvent("mf", "mf.0016", Id);
                    MusicBrainzHelper.GetReleases(Id, releases);
                }
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message); 
            }

            return releases;
        }

        public MbRelease GetRelease(string name, string status = "Official", string type = "album", string country = "US")
        {
            try
            {
                // todo: add to logger

                // (1) Check for release information in the DB
                var cmd = MusicDb.GetXmlSpCmd("MbRelease", "getRelease_Aid_Title", "1.0.0", Id, name);
                using (var reader = MusicDb.DbObject.ExecuteReader(cmd))
                    while (reader.Read())
                        return new MbRelease(reader);

                // (2) Look up the release information via the WWW
                var filter = new Dictionary<string, string>
                           {
                               {"arid", Id},
                               {"release", name},
                               {"status", status},
                               {"type", type},
                               {"country", country}
                           };

                return MusicBrainzHelper.GetRelease(filter, Id);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message); 
                return null;
            }
        }




        public void Save()
        {
            try
            {
                RnLocale.LogEvent("mf", "mf.0008", Name, Id);
                RefreshDbRowId();
                string sql;

                if(DbRowId == 0)
                {
                    // Insert the missing artist into the DB
                    sql = MusicDb.GetXmlSpCmd(
                        "MbArtist", "save_artist", "1.0.0",
                        Type, Id, Name, SortName, Country
                        );
                    MusicDb.DbObject.ExecuteNonQuery(sql);
                    RefreshDbRowId();
                    return;
                }

                // Update the existing artist information
                sql = MusicDb.GetXmlSpCmd(
                    "MbArtist", "update_artis", "1.0.0",
                    Type, Name, SortName, Country, Id
                    );
                MusicDb.DbObject.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }

        private void LoadFromDbViaName(string artistName)
        {
            try
            {
                // Query DB
                const string tmpCmd = @"SELECT ROWID, * FROM tb_artists WHERE artistName = '{0}'";
                var cmd = String.Format(tmpCmd, artistName);
                
                // Load to memory
                using (var reader = MusicDb.DbObject.ExecuteReader(cmd))
                    while (reader.Read())
                    {
                        Score = 100;
                        Type = ToArtistType(reader.GetString("artistType"));
                        Id = reader.GetString("artistId");
                        Name = reader.GetString("artistName");
                        SortName = reader.GetString("artistSortName");
                        Country = reader.GetString("artistCountry");
                        DbRowId = reader.GetInt("ROWID");
                        break;
                    }
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message); 
            }
        }

        private void LoadFromDbViaId(string artistId)
        {
            // Try loading the artist information from thde DB
            var sql = MusicDb.GetXmlSpCmd("MbArtist", "get_artist", "1.0.0", artistId);
            using (var reader = MusicDb.DbObject.ExecuteReader(sql))
                if (reader.HasRows)
                {
                    reader.Read();
                    MapDbRow(reader);
                    return;
                }

            Console.WriteLine("We could not find the artist information in the DB");
        }


    }
}
