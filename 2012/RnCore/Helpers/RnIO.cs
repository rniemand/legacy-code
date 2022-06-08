using System;
using System.IO;

namespace RnCore.Helpers
{
    public static class RnIO
    {
        public static string BaseDir { get; private set; }

        static RnIO()
        {
            // Set the base directory
            var curFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            BaseDir = Path.GetDirectoryName(curFilePath) + "\\";
        }

        public static string MakeRelative(this string p)
        {
            return p.Replace("./", BaseDir);
        }

        // =====> Folder Methods
        public static bool CreateDirectory(string dirPath)
        {
            if (Directory.Exists(dirPath))
                return true;

            try
            {
                Directory.CreateDirectory(dirPath);
                return true;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return false;
            }
        }


        // =====> File Methods
        public static bool Move(string src, string dst, bool overwrite = false)
        {
            if (!File.Exists(src))
            {
                RnLocale.LogEvent("rn.common", "common.003", src);
                return false;
            }

            if (File.Exists(dst) && !overwrite)
            {
                RnLocale.LogEvent("rn.common", "common.005", dst);
                return false;
            }

            try
            {
                // Try delete TGT file if found
                if (File.Exists(dst) && !Delete(dst))
                    return false;

                // Do the move
                File.Move(src, dst);
                return true;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return false;
            }
        }

        public static bool Delete(string filePath)
        {
            if (!File.Exists(filePath))
            {
                RnLocale.LogEvent("rn.common", "common.003", filePath);
                return false;
            }

            try
            {
                File.Delete(filePath);
                RnLocale.LogEvent("rn.common", "common.004", filePath);
                return true;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return false;
            }
        }

        public static string ReadAllText(string filePath)
        {
            if (!File.Exists(filePath))
            {
                RnLocale.LogEvent("rn.common", "common.003", filePath);
                return String.Empty;
            }

            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return String.Empty;
            }
        }

        public static bool WriteAllText(string filePath, string fileContents, bool replaceFile = false)
        {
            if (File.Exists(filePath) && !replaceFile)
            {
                RnLocale.LogEvent("rn.common", "common.002", filePath);
                return false;
            }

            if (File.Exists(filePath) && !Delete(filePath))
                return false;

            try
            {
                File.WriteAllText(filePath, fileContents);
                return true;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return false;
            }
        }

    }
}
