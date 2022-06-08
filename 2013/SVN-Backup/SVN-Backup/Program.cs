using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Ionic.Zip;
using Ionic.Zlib;
using SVN_Backup.Config;
using SVN_Backup.Helpers;
using SharpSvn;

namespace SVN_Backup
{
    internal class Program
    {
        private static SvnClient _svnClient;
        private static RnSvnConfig _cfg;

        private static void Main(string[] args)
        {
            CheckForConfigFile();
            if (!LoadFromConfig())
            {
                Thread.Sleep(3000);
                Environment.Exit(-2);
            }

            _svnClient = new SvnClient();
            _svnClient.Notify += svnClient_Notify;

            UpdateRepo();
            // Generate the zip file if its enabled
            if (_cfg.ZipEnabled) GenerateZipFile();

            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("# All Done");
            Console.WriteLine("======================================");
            Console.ReadLine();
        }

        private static void svnClient_Notify(object sender, SvnNotifyEventArgs e)
        {
            if (e.Action == SvnNotifyAction.UpdateCompleted)
                Console.WriteLine("Update Completed...");
            if (e.Action == SvnNotifyAction.UpdateAdd)
                Console.WriteLine("[ADDED] {0}", RnIO.GetFileName(e.Path));
            if (e.Action == SvnNotifyAction.UpdateDelete)
                Console.WriteLine("[DELETED] {0}", RnIO.GetFileName(e.Path));
            if (e.Action == SvnNotifyAction.UpdateUpdate)
                Console.WriteLine("[CHANGE] {0}", RnIO.GetFileName(e.Path));
        }

        private static void UpdateRepo()
        {
            try
            {
                Console.WriteLine();
                Console.WriteLine("============================================");
                Console.WriteLine("= Updating Repo...");
                Console.WriteLine("============================================");
                Console.WriteLine();

                if (RnIO.DirExists(_cfg.DirCheckout, true))
                {
                    _svnClient.CheckOut(new SvnUriTarget(_cfg.SvnUri), _cfg.DirCheckout);
                    return;
                }

                RnLogger.LogError("Unable to update repo, target dir '{0}' not found!", _cfg.DirCheckout);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private static void GenerateZipFile()
        {
            try
            {
                // Ensure that we have a ZIP file name to work with
                var tmpZipName = String.Format("{0}{1}.zip", Path.GetTempPath(), Path.GetRandomFileName());
                var pZip = GenerateZipName();

                if (String.IsNullOrEmpty(pZip))
                {
                    RnLogger.LogError(
                        "Cannot create the ZIP file, unable to generate ZIP name. Please check your config!");
                    return;
                }

                RnIO.DeleteDirectory(_cfg.DirExport, true);
                Console.WriteLine();
                Console.WriteLine("============================================");
                Console.WriteLine("= Beginning Export...");
                Console.WriteLine("============================================");
                Console.WriteLine();
                _svnClient.Export(_cfg.DirCheckout, _cfg.DirExport);

                Console.WriteLine();
                Console.WriteLine("============================================");
                Console.WriteLine("= Generating ZIP file...");
                Console.WriteLine("============================================");
                Console.WriteLine();

                // Target file name check
                if (!RnIO.DeleteFile(pZip))
                {
                    RnLogger.LogError("Cannot delete ZIP file '{0}'", pZip);
                    return;
                }

                // Make the zip, use a temp name
                using (var z = new ZipFile(tmpZipName))
                {
                    z.CompressionLevel = CompressionLevel.BestCompression;
                    z.SaveProgress += z_SaveProgress;
                    z.AddDirectory(_cfg.DirExport);
                    z.Save();
                }

                // If successful, move the zip to new name, do cleanup
                RnIO.MoveFile(tmpZipName, pZip, true);
                RnIO.DeleteDirectory(_cfg.DirExport, true);

                Console.WriteLine();
                Console.WriteLine("Done...");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private static void z_SaveProgress(object sender, SaveProgressEventArgs e)
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
            try
            {
                var sb = new StringBuilder();
                sb.Append(String.Format(@"{0}\", DateTime.Now.Year));
                sb.Append(String.Format(@"Q{0}\", DateTime.Now.GetYearQuater()));
                sb.Append(String.Format(
                    "{0}-{1}-{2}.zip",
                    DateTime.Now.Year,
                    DateTime.Now.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
                    DateTime.Now.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0')
                              ));

                return String.Format(_cfg.ZipFileName, sb);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return String.Empty;
        }

        private static void CheckForConfigFile()
        {
            try
            {
                if (!File.Exists("./RnConfig.xml"))
                {
                    // todo: create it...
                    RnLogger.LogError("Could not find config file 'RnConfig.xml'!");
                    Thread.Sleep(3000);
                    Environment.Exit(-1);
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                Thread.Sleep(3000);
                Environment.Exit(-1);
            }
        }

        private static bool LoadFromConfig()
        {
            try
            {
                const string configElementName = "Dev";
                if (!SvnGlobalConfig.Instance.RnSvnConfigExists(configElementName))
                {
                    RnLogger.LogError("Could not find the config element '{0}'!", configElementName);
                    return false;
                }

                // Generate config file from SvnGlobalConfig
                _cfg = new RnSvnConfig(configElementName);
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return false;
        }

    }

}