using System;
using AlexPilotti.FTPS.Client;
using AlexPilotti.FTPS.Common;
using RnUtils.Helpers;
using RnUtils.Logging;

namespace FtpSync.Old
{
    public static class FtpSyncLogging
    {

        public static void LogClientCommand(object sender, LogCommandEventArgs args)
        {
            // Console Out
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(args.CommandText);
            Console.ResetColor();

            // Logger Out
            RnLogger.Debug("[FTP] [COMMAND] {0}", args.CommandText);
        }

        public static void LogServerReply(object sender, LogServerReplyEventArgs args)
        {
            // Console Out
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[{0}] {1}", args.ServerReply.Code, args.ServerReply.Message);
            Console.ResetColor(); 

            // Logger Out
            RnLogger.Debug("[FTP] [REPLY] ({0}) {1}", args.ServerReply.Code, args.ServerReply.Message);
        }

        public static void TransferCallback(FTPSClient sender, ETransferActions action, string localFile, string remoteFile, ulong fileTransmittedBytes, ulong? fileTransferSize, ref bool cancel)
        {
            // Log file download progress
            if (action == ETransferActions.FileDownloadingStatus)
            {
                if (fileTransferSize != null && fileTransmittedBytes > 0)
                {
                    var perc = Math.Floor(fileTransmittedBytes / fileTransferSize.CastDouble() * 100);
                    OnFileDownloadProgressChange(remoteFile, localFile, fileTransferSize.CastUlong(), perc,
                                                 ref cancel);
                }
                else
                {
                    OnFileDownloadProgressChange(remoteFile, localFile, fileTransferSize.CastUlong(), 0,
                                                 ref cancel);
                }
            }

            // Log file download completed
            if (action == ETransferActions.FileDownloaded)
            {
                RnLogger.Debug("[DOWNLOAD] [COMPLETE] {0} ({1} Bytes)", localFile, fileTransferSize.CastLong());
                OnFileDownloadComplete(remoteFile, localFile, fileTransferSize.CastUlong());
            }
        }



        private static void OnFileDownloadProgressChange(string remotefilename, string localfilename, ulong filesize, double percentcomplete, ref bool cancel)
        {
            var mod = filesize >= 1048576 ? 5 : 20;

            if (percentcomplete%mod == 0)
            {
                RnLogger.Debug("[DOWNLOAD] [PROGRESS] ({0}%) {1}", percentcomplete, localfilename);
                Console.WriteLine("[DOWNLOAD] [PROGRESS] ({0}%) {1}", percentcomplete, localfilename);
            }
        }

        private static void OnFileDownloadComplete(string remotename, string localname, ulong filesize)
        {
            var msg = String.Format("[DOWNLOAD] [COMPLETE] (Src: {0}) (Dst: {1}) (Size: {2})", remotename, localname,
                                    filesize);
            Console.WriteLine(msg);
            RnLogger.Info(msg);
        }

    }
}
