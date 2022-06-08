using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Rn.Core.Logging;

namespace Rn.Core.Helpers
{
    public static class IOHelper
    {
        public static string BaseDir { get; private set; }

        // Constructor for the class
        static IOHelper()
        {
            // Set the applications root directory..
            BaseDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if (BaseDir != null && BaseDir.Substring(BaseDir.Length - 1, 1) != @"\")
                BaseDir = BaseDir + @"\";
        }


        public static bool CreateDir(string dirPath)
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
                Locale.LogEvent("rn.core", "0006", dirPath, ex.Message);
                return false;
            }
        }

        public static bool RenameFile(string oldName, string newName, bool overwrite = false)
        {
            if (!File.Exists(oldName)) return false;
            
            if (!overwrite && File.Exists(newName))
            {
                Locale.LogEvent("rn.core", "0009", oldName, newName, "Target file already exists");
                return false;
            }

            try
            {
                if (File.Exists(newName))
                {
                    if (!DeleteFile(newName))
                    {
                        Locale.LogEvent("rn.core", "0009", oldName, newName, "Cannot remove target file");
                    }
                }
                File.Move(oldName, newName);
                return true;
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core", "0009", oldName, newName, ex.Message);
                return false;
            }
        }

        public static bool DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
                Locale.LogEvent("rn.core", "0010", filePath);
                return true;
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return false;
            }
        }

        public static bool CopyFile(string source, string destination, bool overwrite = false)
        {
            // Check to see if we need to delete this file
            if (File.Exists(destination) && !overwrite)
                return true;

            if (File.Exists(destination) && overwrite)
                if (!DeleteFile(destination))
                {
                    Locale.LogEvent("rn.core", "0011", source, "Target file already exists");
                    return false;
                }

            try
            {
                File.Copy(source, destination);
                Locale.LogEvent("rn.core", "0008", source, destination);
                return true;
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return false;
            }
        }

        public static string FormatRelative(string path)
        {
            path = path.Replace("./", BaseDir);

            if (path.Substring(path.Length - 1, 1) != @"\")
                path = path + @"\";

            return path;
        }

        public static string GetExtension(string filePath)
        {
            var s = Path.GetExtension(filePath);
            return s != null ? s.Replace(".", "") : filePath;
        }

        public static string MakeRelativePath(this string s)
        {
            return s.Replace("./", BaseDir);
        }

        public static string AppendSlashes(string path)
        {
            return path.Substring(path.Length - 1, 1) != @"\" ? String.Format(@"{0}\", path) : path;
        }

    }
}
