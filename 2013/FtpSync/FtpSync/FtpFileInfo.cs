using System;
using System.Data.SQLite;
using System.IO;
using RnUtils.DB.RnSQLite;
using RnUtils.Helpers;
using RnUtils.Logging;
using RnUtils.Sync.Enums;

namespace FtpSync
{
    public class FtpFileInfo
    {
        // Remote file properties
        public bool RemoteFileExists { get; private set; }
        public string RemoteFilePath { get; private set; }
        public DateTime RemoteModifiedTime { get; private set; }
        public DateTime RemoteCheckedTime { get; private set; }
        public long RemoteSize { get; private set; }

        // Local file properties
        public bool LocalFileExists { get; private set; }
        public string LocalFileName { get; private set; }
        public string LocalFileDir { get; private set; }
        public string LocalFileFullPath { get; private set; }
        public DateTime LocalModifiedTime { get; private set; }
        public DateTime LocalCheckedTime { get; private set; }
        public long LocalSize { get; private set; }

        // State properties
        public long ConfigId { get; private set; }
        public long FtpFileId { get; private set; }
        public FtpFileConstructorSource ConstructorSource { get; private set; }
        public FtpSyncClientConfig Config { get; private set; }


        #region Constructors
        public FtpFileInfo(FileSystemInfo file, FtpSyncClientConfig config)
        {
            try
            {
                SetDefaultValues();
                Config = config;

                ConstructorSource = FtpFileConstructorSource.LocalFile;
                LocalFileExists = file.Exists;
                LocalFileFullPath = file.FullName;
                LocalFileName = file.Name;

                WorkFileNames();
                RefreshLocalFileInfo();
                LoadInfoFromDb();

                if (Config.FetchRemoteFileInfoOnInitialScan)
                    RefreshRemoteFileInfo();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public FtpFileInfo(string remoteFileName, FtpSyncClientConfig config)
        {
            try
            {
                SetDefaultValues();
                Config = config;

                RemoteFilePath = remoteFileName;
                WorkFileNames();
                LoadInfoFromDb();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public FtpFileInfo(SQLiteDataReader reader, FtpSyncClientConfig config)
        {
            try
            {
                SetDefaultValues();
                Config = config;

                ConstructorSource = FtpFileConstructorSource.RemoteFile;
                MapSqliteDataReader(reader);
                WorkFileNames();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        } 
        #endregion


        private void SetDefaultValues()
        {
            LocalCheckedTime = new DateTime(1970, 1, 1);
            LocalModifiedTime = new DateTime(1970, 1, 1);
            RemoteCheckedTime = new DateTime(1970, 1, 1);
            RemoteModifiedTime = new DateTime(1970, 1, 1);
            
            ConstructorSource = FtpFileConstructorSource.RemoteFile;
        }


        // File commands
        public void RefreshLocalFileInfo()
        {
            try
            {
                LocalFileExists = File.Exists(LocalFileFullPath);
                LocalCheckedTime = DateTime.Now;

                if (LocalFileExists)
                {
                    var fileInfo = new FileInfo(LocalFileFullPath);
                    LocalModifiedTime = fileInfo.LastWriteTime;
                    LocalSize = fileInfo.Length;
                }
                else
                {
                    LocalModifiedTime = new DateTime(1970, 1, 1);
                    LocalSize = 0;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void RefreshRemoteFileInfo()
        {
            if (Config.CanRefreshInfo(RemoteCheckedTime))
                return;
            RemoteCheckedTime = DateTime.Now;

            // Ensure remote file existance
            if (!Config.FtpsClient.EnsureClientConnected(Config))
                return;

            if (String.IsNullOrEmpty(RemoteFilePath))
            {
                RemoteFileExists = false;
                RemoteSize = 0;
                RemoteModifiedTime = new DateTime(1970, 1, 1);
                return;
            }

            try
            {
                // Check if the remote file still exists
                var remoteFileModTime = Config.FtpsClient.GetFileModificationTime(RemoteFilePath);
                if (remoteFileModTime == null)
                {
                    RemoteFileExists = false;
                    RemoteModifiedTime = new DateTime(1970, 1, 1);
                    RemoteFilePath = String.Empty;
                    RemoteSize = 0;
                }
                else
                {
                    RemoteFileExists = true;
                    RemoteModifiedTime = remoteFileModTime.CastDateTime();
                    RemoteSize = Config.FtpsClient.GetFileTransferSize(RemoteFilePath).CastLong();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void DownloadFile()
        {
            // Ensure that we have the correct folders to work with
            if (String.IsNullOrEmpty(LocalFileDir)) return;
            if (!Directory.Exists(LocalFileDir) && !RnIO.CreateDirectory(LocalFileDir))
                return;

            // Download the remote file
            DoDownloadFile();
            Save();
        }

        private void DoDownloadFile()
        {
            try
            {
                RnLogger.Debug("[DOWNLOAD] [START] (Source: {0}) (Destination: {1})", RemoteFilePath, LocalFileFullPath);
                Config.FtpsClient.EnsureClientConnected(Config);
                Config.FtpsClient.GetFile(RemoteFilePath, LocalFileFullPath, FtpSyncLogging.TransferCallback);
                RefreshLocalFileInfo();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }



        private void WorkFileNames()
        {
            try
            {
                if (ConstructorSource == FtpFileConstructorSource.LocalFile)
                {
                    RemoteFilePath = Config.RemoteDir + LocalFileName.Replace(@"\", @"/");
                }

                if (ConstructorSource == FtpFileConstructorSource.RemoteFile)
                {
                    LocalFileFullPath = String.Format(
                        @"{0}{1}", Config.LocalDir, RemoteFilePath.Substring(1).Replace("/", @"\"));
                }


                if (!String.IsNullOrEmpty(LocalFileFullPath))
                {
                    LocalFileDir = Path.GetDirectoryName(LocalFileFullPath).AppendSlash();
                    LocalFileName = LocalFileFullPath.Replace(Config.LocalDir, "");
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void MapSqliteDataReader(SQLiteDataReader reader)
        {
            try
            {
                FtpFileId = reader.GetLongColumn("rowId");
                RemoteFileExists = reader.GetBoolColumn("RemoteFileExists");
                RemoteFilePath = reader.GetStringColumn("RemoteFilePath");
                RemoteModifiedTime = reader.GetDateTimeColumn("RemoteModifiedTime");
                RemoteCheckedTime = reader.GetDateTimeColumn("RemoteCheckedTime");
                RemoteSize = reader.GetLongColumn("RemoteSize");
                LocalFileExists = reader.GetBoolColumn("LocalFileExists");
                LocalFileName = reader.GetStringColumn("LocalFileName");
                LocalModifiedTime = reader.GetDateTimeColumn("LocalModifiedTime");
                LocalSize = reader.GetLongColumn("LocalSize");
                LocalCheckedTime = reader.GetDateTimeColumn("LocalCheckedTime");
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }




        public void Save()
        {
            try
            {
                var sql = @"UPDATE ftp_fileInfo
                SET
                    RemoteFileExists = @RemoteFileExists,
                    RemoteFilePath = @RemoteFilePath,
                    RemoteModifiedTime = @RemoteModifiedTime,
                    RemoteSize = @RemoteSize,
                    LocalFileExists = @LocalFileExists,
                    LocalFileName = @LocalFileName,
                    LocalModifiedTime = @LocalModifiedTime,
                    LocalSize = @LocalSize,
                    configId = @configId,
                    RemoteCheckedTime = @RemoteCheckedTime,
                    LocalCheckedTime = @LocalCheckedTime
                WHERE
                    rowid = " + FtpFileId;

                if (FtpFileId == 0)
                {
                    sql = @"INSERT INTO ftp_fileInfo
                       (configId, RemoteFileExists, RemoteFilePath, RemoteModifiedTime, RemoteSize, LocalFileExists, LocalFileName, LocalModifiedTime, LocalSize, RemoteCheckedTime, LocalCheckedTime)
                VALUES
                       (@configId, @RemoteFileExists, @RemoteFilePath, @RemoteModifiedTime, @RemoteSize, @LocalFileExists, @LocalFileName, @LocalModifiedTime, @LocalSize, @RemoteCheckedTime, @LocalCheckedTime)";
                }

                var cmd = Config.Db.GenerateSqLiteCommand(sql);
                cmd.Parameters.AddWithValue("configId", Config.DbRowId);
                cmd.Parameters.AddWithValue("RemoteFileExists", RemoteFileExists.CastInt());
                cmd.Parameters.AddWithValue("RemoteFilePath", RemoteFilePath);
                cmd.Parameters.AddWithValue("RemoteModifiedTime", RemoteModifiedTime);
                cmd.Parameters.AddWithValue("RemoteSize", RemoteSize);
                cmd.Parameters.AddWithValue("LocalFileExists", LocalFileExists.CastInt());
                cmd.Parameters.AddWithValue("LocalFileName", LocalFileName);
                cmd.Parameters.AddWithValue("LocalModifiedTime", LocalModifiedTime);
                cmd.Parameters.AddWithValue("LocalSize", LocalSize);
                cmd.Parameters.AddWithValue("RemoteCheckedTime", RemoteCheckedTime);
                cmd.Parameters.AddWithValue("LocalCheckedTime", LocalCheckedTime);

                Config.Db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void LoadInfoFromDb()
        {
            try
            {
                const string sqlCmd = @"SELECT rowid, *
                FROM ftp_fileinfo
                WHERE configId = @configId
                AND RemoteFilePath = @RemoteFilePath";

                var cmd = Config.Db.GenerateSqLiteCommand(sqlCmd);
                cmd.Parameters.AddWithValue("configId", Config.DbRowId);
                cmd.Parameters.AddWithValue("RemoteFilePath", RemoteFilePath);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows || !reader.Read()) return;
                    ConfigId = reader.GetLongColumn("configId");
                    RemoteModifiedTime = reader.GetDateTimeColumn("RemoteModifiedTime");
                    RemoteFileExists = reader.GetBoolColumn("RemoteFileExists");
                    RemoteSize = reader.GetLongColumn("RemoteSize");
                    RemoteCheckedTime = reader.GetDateTimeColumn("RemoteCheckedTime");
                    LocalCheckedTime = reader.GetDateTimeColumn("LocalCheckedTime");
                    FtpFileId = reader.GetLongColumn("rowid");
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }


    }

}
