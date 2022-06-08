using System;
using System.Data.SQLite;
using FtpSync.Db;
using RnCore.Logging;
using RnCore.Helpers;

namespace FtpSync
{
    public class FtpCopyClientConfig
    {
        // Public methods
        public string FtpHost { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPassword { get; set; }
        public string RemoteDir { get; set; }
        public string LocalDir { get; set; }
        public bool AcceptAllCerts { get; set; }
        public bool UseClearText { get; set; }
        public bool FetchRemoteFileInfoOnInitialScan { get; set; }
        
        public int RefreshInfoThresholdMin { get; set; }
        public int MaxDownloadBatchSize { get; set; }

        public FtpCopyClientConfig()
        {
            RefreshInfoThresholdMin = 86400;
            MaxDownloadBatchSize = 500;
        }

        // DB helpers
        private string _configName;
        public string ConfigName
        {
            get { return _configName; }
            set
            {
                _configName = value;
                SyncWithDb();
            }
        }
        public long DbRowId { get; private set; }


        private void SyncWithDb()
        {
            DbRowId = 0;
            RnLogger.LogDebug("Checking configuration with local DB store.");
            LoadConfigFromDb();
        }

        private void LoadConfigFromDb()
        {
            try
            {
                RnLogger.LogDev("Looking up configuration info for '{0}'", ConfigName);
                const string sqlQuery = @"SELECT rowid FROM ftp_config WHERE configName = '{0}'";
                DbRowId = FtpSyncDb.GetSingleResultAsLong(sqlQuery.QuickFormat(ConfigName));
                if (DbRowId == 0) SaveConfigToDb();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void SaveConfigToDb()
        {
            try
            {
                RnLogger.LogInfo("Saving new configuration to DB ({0})", ConfigName);
                var cmd = new SQLiteCommand(@"INSERT INTO ftp_config (configName) VALUES (@configName);");
                cmd.Parameters.AddWithValue("configName", ConfigName);
                DbRowId = FtpSyncDb.InsertAndGetRowId(cmd);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

    }
}
