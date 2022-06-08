using System;
using System.Data.SQLite;
using System.Xml;
using MfLib.Database;
using RnCore.Helpers;
using RnCore.Db.Sqlite;

namespace MfLib.Providers.MusicBrainz
{
    public class MbRecording
    {
        public string Id { get; private set; }
        public string ReleaseId { get; private set; }
        public string ArtistId { get; private set; }
        public string Title { get; private set; }
        public double Length { get; private set; }
        public int TrackNo { get; private set; }
        public MbRelease ReleaseInfo { get; private set; }
        public MbArtist ArtistInfo { get; private set; }
        public int DbRowId { get; private set; }



        public MbRecording(XmlNode n, string releaseId)
        {
            try
            {
                ReleaseId = releaseId;
                Id = n.GetAttr("id");
                Title = n.GetChildNodeString("title");
                Length = n.GetChildNodeDbl("length");

                // Load additional information for this track
                ReleaseInfo = new MbRelease(ReleaseId);
                ArtistInfo = new MbArtist(ReleaseInfo.ArtistId);
                ArtistId = ArtistInfo.Id;
                LoadTrackInfo(n);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }

        public MbRecording(SQLiteDataReader r, string releaseId)
        {
            try
            {
                ReleaseId = releaseId;

                // Map generic DB information
                TrackNo = r.GetInt("recTrackNo");
                Length = r.GetDbl("recLength");
                Id = r.GetString("recId");
                ArtistId = r.GetString("artistId");
                Title = r.GetString("recTitle");

                // Load additional information for this track
                ReleaseInfo = new MbRelease(ReleaseId);
                ArtistInfo = new MbArtist(ReleaseInfo.ArtistId);
                ArtistId = ArtistInfo.Id;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }




        private void LoadTrackInfo(XmlNode n)
        {
            try
            {
                // Find our release in the XML
                var xpath = String.Format("release-list/release[@id='{0}']", ReleaseId);
                var node = n.SelectSingleNode(xpath);
                if (node == null)
                {
                    RnLocale.LogEvent("mf", "mf.0010", ReleaseId);
                    return;
                }

                // Get the track information
                var trackNode = node.SelectSingleNode("medium-list/medium/track-list/track");
                if (trackNode == null)
                {
                    RnLocale.LogEvent("mf", "mf.0011", Id);
                    return;
                }

                TrackNo = trackNode.GetChildNodeInt("number");
                Title = trackNode.GetChildNodeString("title");
                Length = trackNode.GetChildNodeDbl("length");
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
                var sql = MusicDb.GetXmlSpCmd("MbRecording", "look_for_recording", "1.0.0", Id);
                DbRowId = MusicDb.DbObject.GetSingleResultInt(sql);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }


        public void Save()
        {
            try
            {
                RnLocale.LogEvent("mf", "mf.0009", ArtistInfo.Name, Title, Id);
                RefreshDbRowId();
                string sql;

                if (DbRowId == 0)
                {
                    // Insert NEW recording information into the DB
                    sql = MusicDb.GetXmlSpCmd(
                        "MbRecording", "save_recording", "1.0.0",
                        TrackNo,
                        Length,
                        Id,
                        ReleaseId,
                        ArtistId,
                        Title
                        );

                    MusicDb.DbObject.ExecuteNonQuery(sql);
                    RefreshDbRowId();
                    return;
                }

                // Update the EXISTING recording information in the DB
                sql = MusicDb.GetXmlSpCmd(
                    "MbRecording", "update_recording", "1.0.0",
                    TrackNo,
                    Length,
                    ReleaseId,
                    ArtistId,
                    Title,
                    Id
                    );
                MusicDb.DbObject.ExecuteNonQuery(sql);

            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }

    }
}
