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
    public class MbRelease
    {
        public int DbRowId { get; private set; }
        public string Id { get; private set; }
        public string Title { get; private set; }
        public AlbumStatus Status { get; private set; }
        public AlbumQuality Quality { get; private set; }
        public string Language { get; private set; }
        public string Script { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public string Country { get; private set; }
        public string Barcode { get; private set; }
        public string ArtistId { get; private set; }


        //=========================================================>
        // Constructors
        public MbRelease(XmlNode n, string artistId)
        {
            try
            {
                DbRowId = 0;
                Id = n.GetAttr("id");
                Title = n.GetChildNodeString("title");
                Status = ToAlbumStatus(n.GetChildNodeString("status"));
                Quality = ToAlbumQuality(n.GetChildNodeString("quality"));
                Language = n.GetChildNodeString("text-representation/language");
                Script = n.GetChildNodeString("text-representation/script");
                Country = n.GetChildNodeString("country");
                Barcode = n.GetChildNodeString("barcode");
                ReleaseDate = GetReleaseDate(n.GetChildNodeString("date"));
                ArtistId = artistId;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }

        public MbRelease(SQLiteDataReader r)
        {
            LoadFromSqliteDr(r);
        }

        public MbRelease(string releaseId)
        {
            // Try to load the release information from the DB
            var sql = MusicDb.GetXmlSpCmd("MbRelease", "get_release", "1.0.0", releaseId);
            using (var reader = MusicDb.DbObject.ExecuteReader(sql))
                if (reader.HasRows)
                {
                    reader.Read();
                    LoadFromSqliteDr(reader);
                    return;
                }

            Console.WriteLine(releaseId);

            // todo: complete this
            Console.WriteLine("We need to look up this release information from musicbrainz");
        }





        private void LoadFromSqliteDr(SQLiteDataReader r)
        {
            DbRowId = r.GetInt("DbRowId");
            Id = r.GetString("releaseId");
            Title = r.GetString("releaseTitle");
            Status = ToAlbumStatus(r.GetString("releaseStatus"));
            Quality = ToAlbumQuality(r.GetString("releaseQuality"));
            Language = r.GetString("releaseLang");
            Script = r.GetString("releaseScript");
            // todo - LATER - get the release date/time
            Country = r.GetString("releaseCountry");
            Barcode = r.GetString("releaseBarcode");
            ArtistId = r.GetString("artistId");
        }


        // these might need to move later on
        public static DateTime GetReleaseDate(string dateString)
        {
            var releaseYear = DateTime.Parse("1970-01-01 00:00:00");

            try
            {
                if (!String.IsNullOrEmpty(dateString))
                {
                    if (dateString.Length == 4)
                        dateString = String.Format("{0}-01-01", dateString);
                    releaseYear = DateTime.Parse(dateString);
                }
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }

            return releaseYear;
        }

        public static AlbumStatus ToAlbumStatus(string status)
        {
            switch (status.ToLower().Trim())
            {
                case "official":
                    return AlbumStatus.Official;

                case "promotion":
                    return AlbumStatus.Promotion;

                case "":
                    return AlbumStatus.NotSet;

                default:
                    RnLocale.LogEvent("mf", "mf.0005", "AlbumStatus", status);
                    return AlbumStatus.Unknown;
            }
        }

        public static AlbumQuality ToAlbumQuality(string quality)
        {
            switch (quality.ToLower().Trim())
            {
                case "normal":
                    return AlbumQuality.Normal;

                case "notset":
                    return AlbumQuality.NotSet;

                default:
                    RnLocale.LogEvent("mf", "mf.0005", "AlbumQuality", quality);
                    return AlbumQuality.Unknown;
            }
        }





        private void RefreshFromDbUsingId()
        {
            try
            {
                var cmd = MusicDb.GetXmlSpCmd("MbRelease", "check_for_release", "1.0.0", Id);
                DbRowId = MusicDb.DbObject.GetSingleResultInt(cmd);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }

        public List<MbRecording> GetRecordings()
        {
            RnLocale.LogEvent("mf", "mf.0015", Id);
            var recordings = new List<MbRecording>();

            try
            {
                // Try to get the recordings from the DB
                var sql = MusicDb.GetXmlSpCmd("MbRelease", "get_recordings", "1.0.0", Id);
                using (var reader = MusicDb.DbObject.ExecuteReader(sql))
                {
                    while (reader.Read())
                        recordings.Add(new MbRecording(reader, Id));
                    reader.Close();
                }

                // If we dont have any recordings, get them from MusicBrainz
                if (recordings.Count == 0)
                {
                    RnLocale.LogEvent("mf", "mf.0014", Id);
                    MusicBrainzHelper.GetTracksForRelease(Id, recordings);
                }
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }

            return recordings;
        }


        public void Save()
        {
            try
            {
                string cmd;

                if (DbRowId == 0)
                {
                    // INSERT new release information
                    cmd = MusicDb.GetXmlSpCmd(
                        "MbRelease", "insert_release", "1.0.0",
                        RnDate.FormatDate("Y-m-d h:i:s.000", ReleaseDate),
                        Id, Status, Quality, Language, Script, Country,
                        Barcode, Title, ArtistId);
                }
                else
                {
                    // UPDATE the current release information
                    cmd = MusicDb.GetXmlSpCmd(
                        "MbRelease", "update_release", "1.0.0",
                        RnDate.FormatDate("Y-m-d h:i:s.000", ReleaseDate),
                        Status, Quality, Language, Script, Country,
                        Barcode, Title, DbRowId);
                }

                MusicDb.DbObject.ExecuteNonQuery(cmd);
                RefreshFromDbUsingId();
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }


    }
}
