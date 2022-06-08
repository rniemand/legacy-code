using System;
using System.Configuration;
using FtpSync.Old;
using RnUtils.Configuration;
using RnUtils.Helpers;


namespace FtpSync
{
    class Program
    {

        static void Main(string[] args)
        {
            var dbFilePath = ConfigurationManager.AppSettings.GetStringSetting("RnSync.DB.DBFilePath", "./RnSync.db");
            var oDbConnection = new RnSyncDbConnection(dbFilePath.MakeRelative());

            
            Console.WriteLine(oDbConnection.DbConnected);



            Console.WriteLine();
            Console.WriteLine("All done...");
            Console.ReadLine();
        }


        static FtpSyncClientConfig ConfigSingleDir()
        {
            return new FtpSyncClientConfig
                {
                    FtpHost = "richard-niemand.name",
                    FtpUserName = "backup@richard-niemand.name",
                    FtpPassword = "...",
                    LocalDir = @"Z:\richard-niemand.name\wp-admin\css\",
                    RemoteDir = "/wp-admin/css/",
                    AcceptAllCerts = true,
                    UseClearText = true,
                    FetchRemoteFileInfoOnInitialScan = false,
                    ConfigName = "richard-niemand.name (dev)",
                    DbFilePath = @"z:\richard-niemand.name.dev.db"
                };
        }

        static FtpSyncClientConfig ConfigFull()
        {
            return new FtpSyncClientConfig
                {
                    FtpHost = "richard-niemand.name",
                    FtpUserName = "backup@richard-niemand.name",
                    FtpPassword = "...",
                    LocalDir = @"Z:\richard-niemand.name\",
                    RemoteDir = "/",
                    AcceptAllCerts = true,
                    UseClearText = true,
                    FetchRemoteFileInfoOnInitialScan = false,
                    ConfigName = "richard-niemand.name",
                    DbFilePath = @"z:\richard-niemand.name.db"
                };
        }

        static void OriginalVibe()
        {
            var config = ConfigFull();
            var client = new FtpSyncClient(config);

            client.AddIgnoredDir("/richard");
            client.AddIgnoredDir("/cgi-bin");

            //client.UpdateRemoteFilesInfo();
            //client.DownloadFiles();
            //client.BatchVerifyLocalFiles();
            client.RunLocalFileIntegrityCheck2();
        }


        /*
         private void CreateDbTables()
        {
            try
            {
                var sql = File.ReadAllText("./CreateDb.sql");
                using (var cmd = new SQLiteCommand(sql,_con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
         * if (!CheckTableExists("ftp_fileInfo"))
         */

    }
}
