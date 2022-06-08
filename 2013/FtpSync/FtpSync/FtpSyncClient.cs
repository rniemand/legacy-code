using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using AlexPilotti.FTPS.Client;
using AlexPilotti.FTPS.Common;
using RnUtils.Helpers;
using RnUtils.Logging;

namespace FtpSync
{
    public class FtpSyncClient
    {
        public bool Connected { get; private set; }
        public List<string> IgnoredDirs { get; private set; }
        public List<FtpFileInfo> FileCache { get; private set; }
        public string CurrentDir { get; private set; }

        public FtpSyncClientConfig Config { get; private set; }


        // FTP commands
        public FtpSyncClient(FtpSyncClientConfig oConfig)
        {
            try
            {
                RnLogger.Debug("Creating a new instance of FtpCopyClient");

                Config = oConfig;
                Config.CreateFtpsClient();
                Config.CreateDbConnection();
                Config.Save();

                IgnoredDirs = new List<string>();
                FileCache = new List<FtpFileInfo>();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void Connect()
        {
            if (Connected) return;

            try
            {
                RnLogger.Debug("Creating FTP connection to '{0}'", Config.FtpHost);
                var esslMode = Config.UseClearText ? ESSLSupportMode.ClearText : ESSLSupportMode.All;
                var cred = new NetworkCredential(Config.FtpUserName, Config.FtpPassword);
                
                if (Config.AcceptAllCerts)
                {
                    RnLogger.Debug("Accepting all remote certificates...");
                    Config.FtpsClient.Connect(Config.FtpHost, cred, esslMode, FtpsClientHelper.ValidateTestServerCertificate);
                }
                else
                {
                    Config.FtpsClient.Connect(Config.FtpHost, cred, esslMode);
                }

                Connected = true;
                RnLogger.Debug("We are now connected to '{0}'", Config.FtpHost);
            }
            catch (Exception ex)
            {
                ex.LogException();
                Connected = false;
            }
        }

        private bool SetFtpDir(string path)
        {
            Config.FtpsClient.SetCurrentDirectory(path);
            CurrentDir = Config.FtpsClient.GetCurrentDirectory();
            return path.StripSlash("/") == Config.FtpsClient.GetCurrentDirectory();
        }



        // Directory functions
        private bool CheckLocalDir()
        {
            RnLogger.Debug("Attempting to create the local copy dir '{0}'", Config.LocalDir);
            return RnIO.CreateDirectory(Config.LocalDir);
        }

        public void AddIgnoredDir(string dirPath)
        {
            if (!IgnoredDirs.Contains(dirPath))
                IgnoredDirs.Add(dirPath);
        }

        public bool IsIgnoredDir(string dir)
        {
            return IgnoredDirs.Contains(dir);
        }

        private static bool CheckLocalDirExists(string targetFilePath)
        {
            try
            {
                var dir = Path.GetDirectoryName(targetFilePath);
                if (dir != null)
                {
                    return RnIO.CreateDirectory(dir);
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return false;
        }

        

        // Path methods
        private string ToLocalPath(string filePath)
        {
            return Config.LocalDir + filePath.Substring(Config.RemoteDir.Length).Replace("/", @"\");
        }

        
        
        // File cache methods
        private void BuildLocalFileCache()
        {
            // Scan the target dir building up a local file cache
            BuildLocalFileCacheScanDir(Config.LocalDir);
            RnLogger.Info("Finished building local file cache (Files: {0}) (Folders: {1})", FileCache.Count, 0);
        }

        private void BuildLocalFileCacheScanDir(string dirPath)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    RnLogger.Error("Cannot find local dir '{0}'", dirPath);
                    return;
                }

                var dir = new DirectoryInfo(dirPath);
                var files = dir.GetFiles();
                var dirs = dir.GetDirectories();

                // Update the local file cache
                if (files.Length > 0)
                {
                    foreach (var currentFile in files)
                    {
                        FileCache.Add(new FtpFileInfo(currentFile, Config));
                    }
                }

                // Recurse local file cache adding all found files to the cache
                if (dirs.Length > 0)
                {
                    foreach (DirectoryInfo currentDir in dirs)
                    {
                        BuildLocalFileCacheScanDir(currentDir.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }



        // File methods
        private void CopyRemoteDirLocally(string remoteDirPath, bool recurse = false)
        {
            var dirContents = Config.FtpsClient.GetDirectoryList(remoteDirPath);
            CurrentDir = remoteDirPath;

            foreach (DirectoryListItem dirItem in dirContents)
            {
                var currentItem = String.Format("{0}{1}", remoteDirPath.AppendSlash("/"), dirItem.Name);

                if (dirItem.IsDirectory)
                {
                    if (!IsIgnoredDir(currentItem) && recurse)
                        CopyRemoteDirLocally(currentItem + "/", true);
                }
                else if (!dirItem.IsDirectory && !dirItem.IsSymLink)
                {
                    CopyFileLocally(currentItem);
                }
                else
                {
                    Console.WriteLine("EH :: {0}", currentItem);
                }
            }
        }

        private void CopyFileLocally(string remotePath)
        {
            try
            {
                var localPath = ToLocalPath(remotePath);
                RnLogger.Debug("[FILE] [CHECK-LOCAL] Checking for local file: {0}", localPath);
                
                // ensure that the local dir was found
                if (!CheckLocalDirExists(localPath))
                {
                    RnLogger.Error("Local dir '{0}' was not found!", localPath);
                    return;
                }

                if (File.Exists(localPath))
                {
                    // compare local file to remote file (look for changes)
                    var localSize = new FileInfo(localPath).Length;
                    var remoteSize = Config.FtpsClient.GetFileTransferSize(remotePath).CastLong();

                    if (remoteSize > localSize)
                    {
                        RnLogger.Debug("Remote file '{0}' changed (Original: {1}) (Current: {2})", remotePath,
                                          localSize, remoteSize);
                        if (!RnIO.DeleteFile(localPath))
                        {
                            RnLogger.Warning("Could not delete '{0}', file was not updated", localPath);
                            return;
                        }

                        RnLogger.Debug("[FILE] [UPDATE] Updating changed file ({0})", localPath);
                        Config.FtpsClient.GetFile(remotePath, localPath, FtpSyncLogging.TransferCallback);
                    }

                    // todo: sync the other way...

                    return;
                }

                RnLogger.Debug("[DOWNLOAD] [START] Downloading new file ({0})", localPath);
                Config.FtpsClient.GetFile(remotePath, localPath, FtpSyncLogging.TransferCallback);    
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private List<FtpFileInfo> GetMissingFileList()
        {
            var fileList = new List<FtpFileInfo>();

            try
            {
                const string sql = @"SELECT
                    rowId,
                    *
                FROM ftp_fileinfo
                WHERE configId = @configId
                      AND RemoteFileExists = 1      
                      AND LocalFileExists = 0
                LIMIT {0}";

                // Generate our select statement
                var cmd = Config.Db.GenerateSqLiteCommand(sql.QuickFormat(Config.MaxDownloadBatchSize));
                cmd.Parameters.AddWithValue("configId", Config.DbRowId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fileList.Add(new FtpFileInfo(reader, Config));
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return fileList;
        }

        private string FormatDbQueryString(string sqlQueryString, params object[] replace)
        {
            try
            {
                // Replace common placeholders
                sqlQueryString = sqlQueryString.Replace("@configId", Config.DbRowId.ToString(CultureInfo.InvariantCulture));

                // After replacing common placeholders, replace any strings
                if (replace.Length > 0)
                    sqlQueryString = String.Format(sqlQueryString, replace);

                RnLogger.Debug("[FormatDbQueryString] {0}", sqlQueryString);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return sqlQueryString;
        }

        private List<FtpFileInfo> GetFileList(string sqlQueryString, List<FtpFileInfo> fileList = null, params object[] replace)
        {
            if (fileList == null)
                fileList = new List<FtpFileInfo>();

            try
            {
                // Fire off the query, generate our file list
                var dbQueryStr = FormatDbQueryString(sqlQueryString, replace);
                var cmd = Config.Db.GenerateSqLiteCommand(dbQueryStr);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fileList.Add(new FtpFileInfo(reader, Config));
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return fileList;
        }




        // public commands
        public void CopyLocal(bool recurseDirs = false)
        {
            // todo: phase this out
            BuildLocalFileCache();
            Connect();

            if (!CheckLocalDir())
            {
                RnLogger.Error("Cannot copy locally ({0}) is missing", Config.LocalDir);
                return;
            }

            if (!SetFtpDir(Config.RemoteDir))
            {
                RnLogger.Error("FTP client cannot switch to '{0}'", Config.RemoteDir);
                return;
            }

            var baseDir = CurrentDir;
            CopyRemoteDirLocally(CurrentDir, recurseDirs);
            CurrentDir = baseDir;
        }

        public void UpdateRemoteFilesInfo(string remoteDir = "")
        {
            // todo: move out so we can close afterwards
            Connect();

            if (string.IsNullOrEmpty(remoteDir))
                remoteDir = Config.RemoteDir;

            var dirContents = Config.FtpsClient.GetDirectoryList(remoteDir);
            CurrentDir = remoteDir;

            foreach (DirectoryListItem dirItem in dirContents)
            {
                var currentItem = String.Format("{0}{1}", remoteDir.AppendSlash("/"), dirItem.Name);

                if (dirItem.IsDirectory)
                {
                    if (!IsIgnoredDir(currentItem))
                    {
                        UpdateRemoteFilesInfo(currentItem);
                    }
                }
                else if (!dirItem.IsDirectory && !dirItem.IsSymLink)
                {
                    var oFile = new FtpFileInfo(currentItem, Config);
                    oFile.RefreshRemoteFileInfo();
                    oFile.RefreshLocalFileInfo();
                    oFile.Save();
                }
                else
                {
                    Console.WriteLine("EH :: {0}", currentItem);
                }
            }
        }

        public void DownloadFiles()
        {
            try
            {
                RnLogger.Debug("Looking for new files to download");

                // todo: add limit on files
                const string sqlQuery = @"SELECT rowId, *
                FROM ftp_fileinfo
                WHERE configId = @configId
                AND LocalFileExists = 0
                AND RemoteFileExists = 1
            
                UNION
                
                SELECT rowId, *
                FROM ftp_fileinfo
                WHERE configId = @configId
                AND RemoteSize != LocalSize
                AND RemoteSize > LocalSize
                AND RemoteFileExists = 1";

                var fileList = GetFileList(sqlQuery);
                var totalFileCount = fileList.Count;

                if (totalFileCount > 0)
                {
                    var currentFileNo = 1;
                    RnLogger.Info("[BATCH] [START] Downloading '{0}' files", totalFileCount);
                    foreach (var remoteFile in fileList)
                    {
                        RnLogger.Debug("[BATCH] [PROGRESS] Downloading file {0} of {1}", currentFileNo, totalFileCount);
                        remoteFile.DownloadFile();
                        currentFileNo++;
                    }
                    RnLogger.Info("[BATCH] [COMPLETED] Finished downloading '{0}' files", totalFileCount);
                }

                Config.FtpsClient.Close();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void BatchVerifyLocalFiles()
        {
            // todo: check actual local files

            // Step 1: Refresh all the local files for our current configId
            const string sqlCommand = @"SELECT rowId, *
            FROM ftp_fileInfo
            WHERE LocalCheckedTime <= '{0}'
            AND configId = @configId
            ORDER BY LocalCheckedTime DESC
            LIMIT {1}";

            var localFiles = GetFileList(
                sqlCommand, null, DateTime.Now.AddMinutes(-Config.VerifyLocalFilesThresholdMin).CastLongDateString(),
                Config.VerifyLocalFilesBatchSize);
            var localFileCount = localFiles.Count;

            if (localFileCount > 0)
            {
                var currentFileNum = 1;
                foreach (var localFile in localFiles)
                {
                    RnLogger.Debug("[FILE] [INTEGRITY] Local File Integrity Check {0} of {1}", currentFileNum,
                                    localFileCount);
                    localFile.RefreshLocalFileInfo();
                    localFile.Save();
                    currentFileNum++;
                }
            }

        }

        public void RunLocalFileIntegrityCheck2()
        {
            var filesToCheck = new List<string>();
            var dir = new DirectoryInfo(Config.LocalDir);
            
            // Generate the SQL select statement
            const string sqlQuery = @"SELECT '{0}' || LocalFileName as 'LocalFileFullName'
            FROM ftp_fileInfo
            WHERE configId = @configId
            AND (LocalCheckedTime <= '{1}' OR LocalCheckedTime IS NULL)
            LIMIT {2}";
            var cmd = Config.Db.GenerateSqLiteCommand(FormatDbQueryString(
                sqlQuery, Config.LocalDir,
                DateTime.Now.AddMinutes(-Config.IntegrityCheckLocalThresholdMin).CastLongDateString(),
                Config.IntegrityCheckLocalBatchSize));

            // Populate the allowed file list
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    filesToCheck.Add(reader["LocalFileFullName"].ToString());

            // Look for local files to check
            var files = dir.GetFiles("*.*", SearchOption.AllDirectories);
            var fileCount = filesToCheck.Count;

            foreach (var localFilePath in filesToCheck)
            {
                var path = localFilePath;
                var tmpFile = files.FirstOrDefault(f => f.FullName.LowerTrim().Equals(path.LowerTrim()));

                if (tmpFile == null)
                {
                    var localFileMissingPath = localFilePath.Replace(Config.LocalDir, "");
                    const string sqlMissing =
                        @"UPDATE ftp_fileInfo SET LocalFileExists = 0 WHERE LocalFileName = '{0}' AND configId = @configId";
                    var tmpCmd = Config.Db.GenerateSqLiteCommand(FormatDbQueryString(sqlMissing, localFileMissingPath));
                    Config.Db.ExecuteNonQuery(tmpCmd);
                }
                else
                {
                    
                }
                

            }


            if (files.Length > 0)
            {
                var processedCounter = 1;
                foreach (var file in files.Where(file => filesToCheck.Contains(file.FullName)))
                {
                    RnLogger.Debug("[INTEGRITY] [LOCAL] Processing file {0} of {1}", processedCounter, fileCount);




                    processedCounter++;
                }
            }




            Console.WriteLine(filesToCheck.Count);





            Console.WriteLine("here");
            Console.ReadLine();

            /*
             SELECT '' || LocalFileName as 'LocalFileFullName'
             FROM ftp_fileInfo
             WHERE LocalCheckedTime <= ''
             */

            if (fileCount > 0)
            {
                var currentFileNum = 1;
                RnLogger.Debug("[FILE] [INTEGRITY] Checking {0} local files", fileCount);
                foreach (var file in files)
                {
                    RnLogger.Debug("[FILE] [INTEGRITY] File {0} of {1}", currentFileNum, fileCount);
                    var oDbFile = new FtpFileInfo(file, Config);
                    oDbFile.Save();
                    currentFileNum++;
                }
                RnLogger.Debug("[FILE] [INTEGRITY] Completed on {0} local files", fileCount);
            }
            
        }




        // todo: local integrity check
        // todo: skippable folders
        // todo: refresh thresholds on folders
        // todo: file watchdog mode
        // todo: Add change counter



        

        
    }
}
