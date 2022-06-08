using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ionic.Zip;
using Ionic.Zlib;
using SharpSvn;
using SharpSvn.Security;
using RnCore.Logging;
using RnCore.Helpers;

namespace SvnTest
{
    internal class Program
    {
        private static SvnClient _svnClient;
        private static RnSvnConfig _cfg;

        private static void Main(string[] args)
        {
            // http://sharpsvn.open.collab.net/
            // http://docs.sharpsvn.net/current/
            // http://sharpsvn.open.collab.net/servlets/ProjectProcess;jsessionid=596239A38D69EB1CFB22537381677728?pageID=3794

            _svnClient = new SvnClient();
            //SvnUI.Bind(svnClient, new UsernamePasswordDialog());
            _svnClient.Notify += svnClient_Notify;

            _cfg = new RnSvnConfig
                {
                    SvnUri = "svn://192.168.0.5/code",
                    //SvnUri = "svn://192.168.0.5/code/c-sharp/MyApps",
                    DirCheckout = @"\\192.168.0.5\d$\svn-checkout\",
                    //DirCheckout = @"C:\Users\Richard\Desktop\bob\",
                    DirExport = @"c:\rn-snv-tmp\",
                    ZipEnabled = true
                };

            UpdateRepo();
            UpdateCompleted();

            Console.WriteLine("======================================");
            Console.WriteLine("# All Done");
            Console.WriteLine("======================================");
            Console.ReadLine();
        }


        private static void svnClient_Notify(object sender, SvnNotifyEventArgs e)
        {
            if (e.Action == SvnNotifyAction.UpdateCompleted)
            {
                Console.WriteLine("Update Completed...");
            }
            if (e.Action == SvnNotifyAction.UpdateAdd)
            {
                Console.WriteLine("[ADDED] {0}", e.FullPath);
            }
            if (e.Action == SvnNotifyAction.UpdateDelete)
            {
                Console.WriteLine("[DELETED] {0}", e.FullPath);
            }
            if (e.Action == SvnNotifyAction.UpdateUpdate)
            {
                Console.WriteLine("[CHANGE] {0}", e.FullPath);
            }
        }

        private static void UpdateRepo()
        {
            try
            {
                if (RnIO.DirExists(_cfg.DirCheckout, true))
                    _svnClient.CheckOut(new SvnUriTarget(_cfg.SvnUri), _cfg.DirCheckout);
                
                // todo: log here
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private static void UpdateCompleted()
        {
            try
            {
                RnIO.DeleteDirectory(_cfg.DirExport, true);

                _svnClient.Export(_cfg.DirCheckout, _cfg.DirExport);

                Console.WriteLine("Adding to zip...");
                var tmpZipName = String.Format("{0}{1}.zip", Path.GetTempPath(), Path.GetRandomFileName());
                var pZip = GenerateZipName();

                if (!RnIO.DeleteFile(pZip))
                {
                    RnLogger.LogError("Cannot delete ZIP file '{0}'", pZip);
                    return;
                }

                using (var z = new ZipFile(tmpZipName))
                {
                    z.CompressionLevel = CompressionLevel.BestCompression;
                    z.SaveProgress += z_SaveProgress;
                    z.AddDirectory(_cfg.DirExport);
                    z.Save();
                }

                RnIO.MoveFile(tmpZipName, pZip, true);
                RnIO.DeleteDirectory(_cfg.DirExport);
                Console.WriteLine("Done...");
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        static void z_SaveProgress(object sender, SaveProgressEventArgs e)
        {
            try
            {
                if (e.EventType == ZipProgressEventType.Saving_AfterWriteEntry)
                {
                    var perc = Math.Round((((double) e.EntriesSaved/e.EntriesTotal)*100), 2);
                    Console.WriteLine("[ZIP %] {0}", perc);    
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private static string GenerateZipName()
        {
            var sb = new StringBuilder();

            try
            {
                sb.Append(@"\\192.168.0.5\m$\SVN-Snapshots\");
                sb.Append(String.Format(@"{0}\", DateTime.Now.Year));
                sb.Append(String.Format(@"Q{0}\", DateTime.Now.GetYearQuater()));
                sb.Append(String.Format(
                    "{0}-{1}-{2}.zip",
                    DateTime.Now.Year,
                    DateTime.Now.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
                    DateTime.Now.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0')
                              ));
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return sb.ToString();
        }

    }
}
