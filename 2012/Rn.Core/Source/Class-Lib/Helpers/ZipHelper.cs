using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Rn.Core.Logging;

namespace Rn.Core.Helpers
{
    public static class ZipHelper
    {

        public static string AsString(this ZipEntry e, string defValue = "")
        {
            try
            {
                using (var ms = new MemoryStream())
                using (var sr = new StreamReader(ms))
                {
                    e.Extract(ms);
                    ms.Position = 0;
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return defValue;
            }
        }

        public static void AddFileFromUrl(string zipPath, string url, string nameInZip, bool replace = false)
        {
            try
            {
                // Check if the zip file contains the file
                CreateEmptyZip(zipPath);
                if (ContainsEntry(zipPath, nameInZip) && !replace) return;
                var tmpFileName = Path.GetTempFileName();

                // Add the file into the zip
                if (!WebHelper.DownloadFile(url, tmpFileName, true))
                    return;

                using (var zip = ZipFile.Read(zipPath))
                {
                    if (zip.ContainsEntry(nameInZip))
                        zip.RemoveEntry(nameInZip);
                    else
                        zip.AddEntry(nameInZip, new FileStream(tmpFileName, FileMode.Open));
                    zip.Save();
                }

                File.Delete(tmpFileName);
                Locale.LogEvent("as.core", "0017", url, nameInZip, zipPath);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }
        }

        public static bool ContainsEntry(string zipPath, string entryName)
        {
            try
            {
                using (var zip = ZipFile.Read(zipPath))
                    return zip.ContainsEntry(entryName);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return false;
            }
        }

        public static bool ContainsEntry(this ZipFile z, string entryName)
        {
            try
            {
                if (z == null || z.Entries.Count == 0)
                    return false;

                return z.Entries.Any(e => e.FileName == entryName);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return false;
            }
        }

        public static bool CreateEmptyZip(string zipPath)
        {
            if (File.Exists(zipPath)) return true;

            try
            {
                using (var z = new ZipFile(zipPath))
                    z.Save();

                return true;
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return false;
            }
        }



        public static string ExtractFile(string zipPath, string fileName, string defValue = "")
        {
            if (!File.Exists(zipPath))
            {
                Locale.LogEvent("rn.core.common", "0001", zipPath);
                return defValue;
            }

            using (var z = new ZipFile(zipPath))
            {
                // todo - make a IsEmpty method to replace this
                if (z.Entries.Count == 0)
                {
                    Locale.LogEvent("rn.core", "0016",fileName, zipPath);
                    return defValue;
                }

                foreach (var e in z.Entries.Where(e => e.FileName == fileName))
                    return e.AsString(defValue);

                Locale.LogEvent("rn.core", "0016", fileName, zipPath);
                return defValue;
            }
        }

        public static Stream ExtractFileStream(string zipPath, string fileName)
        {
            var streamOut = new MemoryStream();

            if (!File.Exists(zipPath))
            {
                Locale.LogEvent("rn.core.common", "0001", zipPath);
                return streamOut;
            }

            using (var z = new ZipFile(zipPath))
            {
                // todo - make a IsEmpty method to replace this
                if (z.Entries.Count == 0)
                {
                    Locale.LogEvent("rn.core", "0016", fileName, zipPath);
                    return streamOut;
                }

                foreach (var e in z.Entries.Where(e => e.FileName == fileName))
                    e.Extract(streamOut);

                Locale.LogEvent("rn.core", "0016", fileName, zipPath);
                return streamOut;
            }
        }

    }
}
