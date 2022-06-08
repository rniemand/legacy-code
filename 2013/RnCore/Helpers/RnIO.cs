using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RnCore.Logging;

namespace RnCore.Helpers
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
                    // todo: log
                    return false;
                }

                if (File.Exists(dst) && !replaceIfExists)
                {
                    // todo: log
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

    }
}
