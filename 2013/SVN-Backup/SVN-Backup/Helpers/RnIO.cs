using System;
using System.IO;

namespace SVN_Backup.Helpers
{
    public static class RnIO
    {

        public static bool DirExists(string dirPath, bool createIfMissing = false)
        {
            try
            {
                if (Directory.Exists(dirPath))
                    return true;

                if (!Directory.Exists(dirPath) && !createIfMissing)
                    return false;

                Directory.CreateDirectory(dirPath);
                return Directory.Exists(dirPath);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return false;
        }

        public static bool DeleteDirectory(string dirPath, bool recurse = false)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                    return true;
                Directory.Delete(dirPath, recurse);
                return !Directory.Exists(dirPath);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return false;
        }

        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return true;
                File.Delete(filePath);
                return !File.Exists(filePath);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return false;
        }

        public static bool MoveFile(string src, string dst, bool replaceIfExists = false)
        {
            try
            {
                if (!File.Exists(src))
                {
                    RnLogger.LogWarning("Unable to move file, the source file '{0}' can not be found!", src);
                    return false;
                }

                if (File.Exists(dst) && !replaceIfExists)
                {
                    RnLogger.LogWarning("Cannot move '{0}' => '{1}' (target file exists)", src, dst);
                    return false;
                }

                if (File.Exists(dst) && !DeleteFile(dst))
                    return false;

                File.Move(src, dst);
                return File.Exists(dst);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return false;
        }

        public static string GetFileName(string filePath)
        {
            try
            {
                return Path.GetFileName(filePath);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return filePath;
        }

    }
}