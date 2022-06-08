using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using AlexPilotti.FTPS.Client;
using AlexPilotti.FTPS.Common;
using FtpSync.Db;
using RnCore.Helpers;
using RnCore.Logging;

namespace FtpSync
{
    public class FtpCopyClient
    {
        public bool Connected { get; private set; }
        public List<string> IgnoredDirs { get; private set; }
        public List<FtpFileInfo> FileCache { get; private set; }
        public string CurrentDir { get; private set; }

        private readonly FtpCopyClientConfig _config;
        private readonly FTPSClient _client;


        // FTP commands
        public FtpCopyClient(FtpCopyClientConfig config)
        {
            try
            {
                RnLogger.LogDev("Creating a new instance of FtpCopyClient");

                _config = config;
                
                _client = new FTPSClient();
                _client.LogCommand += FtpsLoggingHelper.LogClientCommand;
                _client.LogServerReply += FtpsLoggingHelper.LogServerReply;

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
                RnLogger.LogDebug("Creating FTP connection to '{0}'", _config.FtpHost);
                var esslMode = _config.UseClearText ? ESSLSupportMode.ClearText : ESSLSupportMode.All;
                var cred = new NetworkCredential(_config.FtpUserName, _config.FtpPassword);
                
                if (_config.AcceptAllCerts)
                {
                    RnLogger.LogDebug("Accepting all remote certificates...");
                    _client.Connect(_config.FtpHost, cred, esslMode, FtpsClientHelper.ValidateTestServerCertificate);
                }
                else
                {
                    _client.Connect(_config.FtpHost, cred, esslMode);
                }

                Connected = true;
                RnLogger.LogDebug("We are now connected to '{0}'", _config.FtpHost);
            }
            catch (Exception ex)
            {
                ex.LogException();
                Connected = false;
            }
        }

        private bool SetFtpDir(string path)
        {
            _client.SetCurrentDirectory(path);
            CurrentDir = _client.GetCurrentDirectory();
            return path.StripSlash("/") == _client.GetCurrentDirectory();
        }



        // Directory functions
        private bool CheckLocalDir()
        {
            RnLogger.LogDebug("Attempting to create the local copy dir '{0}'", _config.LocalDir);
            return RnIO.CreateDirectory(_config.LocalDir);
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
            return _config.LocalDir + filePath.Substring(_config.RemoteDir.Length).Replace("/", @"\");
        }

        
        
        // File cache methods
        private void BuildLocalFileCache()
        {
            // Scan the target dir building up a local file cache
            BuildLocalFileCacheScanDir(_config.LocalDir);
            RnLogger.LogInfo("Finished building local file cache (Files: {0}) (Folders: {1})", FileCache.Count, 0);
        }

        private void BuildLocalFileCacheScanDir(string dirPath)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    RnLogger.LogError("Cannot find local dir '{0}'", dirPath);
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
                        FileCache.Add(new FtpFileInfo(currentFile, _client, _config));
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
            var dirContents = _client.GetDirectoryList(remoteDirPath);
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
                RnLogger.LogDev("[FILE] [CHECK-LOCAL] Checking for local file: {0}", localPath);
                
                // ensure that the local dir was found
                if (!CheckLocalDirExists(localPath))
                {
                    RnLogger.LogError("Local dir '{0}' was not found!", localPath);
                    return;
                }

                if (File.Exists(localPath))
                {
                    // compare local file to remote file (look for changes)
                    var localSize = new FileInfo(localPath).Length;
                    var remoteSize = _client.GetFileTransferSize(remotePath).CastLong();

                    if (remoteSize > localSize)
                    {
                        RnLogger.LogDebug("Remote file '{0}' changed (Original: {1}) (Current: {2})", remotePath,
                                          localSize, remoteSize);
                        if (!RnIO.DeleteFile(localPath))
                        {
                            RnLogger.LogWarn("Could not delete '{0}', file was not updated", localPath);
                            return;
                        }

                        RnLogger.LogDebug("[FILE] [UPDATE] Updating changed file ({0})", localPath);
                        _client.GetFile(remotePath, localPath, FtpsLoggingHelper.TransferCallback);
                    }

                    // todo: sync the other way...

                    return;
                }

                RnLogger.LogDebug("[DOWNLOAD] [START] Downloading new file ({0})", localPath);
                _client.GetFile(remotePath, localPath, FtpsLoggingHelper.TransferCallback);    
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
                var cmd = FtpSyncDb.NewSQLiteCommand(sql.QuickFormat(_config.MaxDownloadBatchSize));
                cmd.Parameters.AddWithValue("configId", _config.DbRowId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fileList.Add(new FtpFileInfo(reader, _client, _config));
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return fileList;
        }

        private List<FtpFileInfo> GetFileList(string sqlQueryString, List<FtpFileInfo> fileList = null)
        {
            if (fileList == null)
                fileList = new List<FtpFileInfo>();

            try
            {
                // Replace common placeholders
                sqlQueryString = sqlQueryString.Replace("@configId", _config.DbRowId.ToString(CultureInfo.InvariantCulture));

                // Fire off the query, generate our file list
                var cmd = FtpSyncDb.NewSQLiteCommand(sqlQueryString);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fileList.Add(new FtpFileInfo(reader, _client, _config));
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
                RnLogger.LogError("Cannot copy locally ({0}) is missing", _config.LocalDir);
                return;
            }

            if (!SetFtpDir(_config.RemoteDir))
            {
                RnLogger.LogError("FTP client cannot switch to '{0}'", _config.RemoteDir);
                return;
            }

            var baseDir = CurrentDir;
            CopyRemoteDirLocally(CurrentDir, recurseDirs);
            CurrentDir = baseDir;
        }

        public void RefreshRemoteFiles(string remoteDir = "")
        {
            // todo: move out so we can close afterwards
            Connect();

            if (string.IsNullOrEmpty(remoteDir))
                remoteDir = _config.RemoteDir;

            var dirContents = _client.GetDirectoryList(remoteDir);
            CurrentDir = remoteDir;

            foreach (DirectoryListItem dirItem in dirContents)
            {
                var currentItem = String.Format("{0}{1}", remoteDir.AppendSlash("/"), dirItem.Name);

                if (dirItem.IsDirectory)
                {
                    if (!IsIgnoredDir(currentItem))
                    {
                        RefreshRemoteFiles(currentItem);
                    }
                }
                else if (!dirItem.IsDirectory && !dirItem.IsSymLink)
                {
                    var oFile = new FtpFileInfo(currentItem, _client, _config);
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

        public void DownloadNewFiles()
        {
            try
            {
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
                    RnLogger.LogInfo("[BATCH] [START] Downloading '{0}' files", totalFileCount);
                    foreach (var remoteFile in fileList)
                    {
                        RnLogger.LogDev("[BATCH] [PROGRESS] Downloading file {0} of {1}", currentFileNo, totalFileCount);
                        remoteFile.DownloadFile();
                        currentFileNo++;
                    }
                    RnLogger.LogInfo("[BATCH] [COMPLETED] Finished downloading '{0}' files", totalFileCount);
                }

                _client.Close();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void RunLocalFileIntegrityCheck()
        {
            // todo: check actual local files

            // Step 1: Refresh all the local files for our current configId
            // todo: add date constraint here
            var localFiles = GetFileList(@"SELECT rowId, * FROM ftp_fileinfo WHERE configId = @configId");
            var localFileCount = localFiles.Count;

            if (localFileCount > 0)
            {
                var currentFileNum = 1;
                foreach (var localFile in localFiles)
                {
                    RnLogger.LogDev("[FILE] [INTEGRITY] Local File Integrity Check {0} of {1}", currentFileNum,
                                    localFileCount);
                    localFile.RefreshLocalFileInfo();
                    localFile.Save();
                    currentFileNum++;
                }
            }

        }

        public void RunLocalFileIntegrityCheck2()
        {
            var dir = new DirectoryInfo(_config.LocalDir);
            var files = dir.GetFiles("*.*", SearchOption.AllDirectories);
            var fileCount = files.Length;

            if (fileCount > 0)
            {
                var currentFileNum = 1;
                RnLogger.LogDev("[FILE] [INTEGRITY] Checking {0} local files", fileCount);
                foreach (var file in files)
                {
                    RnLogger.LogDev("[FILE] [INTEGRITY] File {0} of {1}", currentFileNum, fileCount);
                    var oDbFile = new FtpFileInfo(file, _client, _config);
                    oDbFile.Save();
                    currentFileNum++;
                }
                RnLogger.LogDev("[FILE] [INTEGRITY] Completed on {0} local files", fileCount);
            }
            
        }




        // todo: local integrity check
        // todo: skippable folders
        // todo: refresh thresholds on folders
        // todo: file watchdog mode
        // todo: Add change counter



        

        
    }
}
