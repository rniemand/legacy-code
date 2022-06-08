using System;
using System.Data.SQLite;
using AlexPilotti.FTPS.Client;
using RnUtils.DB.RnSQLite;
using RnUtils.Helpers;
using RnUtils.Logging;

namespace FtpSync
{
    public class FtpSyncClientConfig
    {
        // Public properties
        public string ConfigName { get; set; }
        public string FtpHost { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPassword { get; set; }
        public string RemoteDir { get; set; }
        public string LocalDir { get; set; }
        public bool AcceptAllCerts { get; set; }
        public bool UseClearText { get; set; }
        public bool FetchRemoteFileInfoOnInitialScan { get; set; }
        public string DbFilePath { get; set; }

        public int RefreshInfoThresholdMin { get; set; }
        public int MaxDownloadBatchSize { get; set; }
        public int VerifyLocalFilesBatchSize { get; set; }
        public int VerifyLocalFilesThresholdMin { get; set; }
        public int IntegrityCheckLocalBatchSize { get; set; }
        public int IntegrityCheckLocalThresholdMin { get; set; }

        // Helper objects
        public FTPSClient FtpsClient { get; private set; }
        public RnSQLiteDb Db { get; private set; }

        
        public FtpSyncClientConfig()
        {
            RefreshInfoThresholdMin = 86400;
            MaxDownloadBatchSize = 500;
            VerifyLocalFilesBatchSize = 100;
            VerifyLocalFilesThresholdMin = 1; // todo: change
            IntegrityCheckLocalBatchSize = 50;
            IntegrityCheckLocalThresholdMin = 1; // todo: chnage
        }

        public void CreateFtpsClient()
        {
            try
            {
                RnLogger.Info("Creating a new instance of FTPSClient.");
                FtpsClient = new FTPSClient();
                FtpsClient.LogCommand += FtpSyncLogging.LogClientCommand;
                FtpsClient.LogServerReply += FtpSyncLogging.LogServerReply;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void CreateDbConnection()
        {
            if (Db != null) return;
            if (String.IsNullOrEmpty(DbFilePath))
            {
                RnLogger.Error("You need to specify a DB file path!");
                return;
            }
            Db = new RnSQLiteDb(DbFilePath);
        }

        public void Save()
        {
            SyncWithDb();
        }





        // DB helpers
        public long DbRowId { get; private set; }


        // todo: relook at this...
        private void SyncWithDb()
        {
            DbRowId = 0;
            RnLogger.Debug("Checking configuration with local DB store.");
            LoadConfigFromDb();
        }

        private void LoadConfigFromDb()
        {
            if (Db == null) return;

            try
            {
                RnLogger.Debug("Looking up configuration info for '{0}'", ConfigName);
                const string sqlQuery = @"SELECT rowid FROM ftp_config WHERE configName = '{0}'";
                DbRowId = Db.GetSingleResultAsLong(sqlQuery.QuickFormat(ConfigName));
                if (DbRowId == 0) SaveConfigToDb();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void SaveConfigToDb()
        {
            if (Db == null) return;

            try
            {
                RnLogger.Info("Saving new configuration to DB ({0})", ConfigName);
                var cmd = new SQLiteCommand(@"INSERT INTO ftp_config (configName) VALUES (@configName);");
                cmd.Parameters.AddWithValue("configName", ConfigName);
                DbRowId = Db.InsertAndGetRowId(cmd);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

    }
}
