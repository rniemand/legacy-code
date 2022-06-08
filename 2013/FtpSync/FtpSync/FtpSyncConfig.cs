using System;
using System.Collections.Generic;

namespace FtpSync
{
    public static class FtpSyncConfig
    {
        public static List<string> IgnoreDirs { get; private set; }
        public static string CurrentDir { get; private set; }
        public static string LocalDir { get; private set; }

        


        static FtpSyncConfig()
        {
            IgnoreDirs = new List<string>();
        }

        
        public static void AddIgnoredDir(string dirPath)
        {
            dirPath = AppendSlash(dirPath.ToLower().Trim());
            if (!IgnoreDirs.Contains(dirPath))
                IgnoreDirs.Add(dirPath);
        }

        public static void SetCurrentDir(string dir)
        {
            if (dir.Substring(dir.Length - 1, 1) != "/")
                dir = String.Format("{0}/", dir);

            CurrentDir = AppendSlash(dir);
        }

        public static void SetLocalDir(string dir)
        {
            LocalDir = dir;
        }

        public static bool IsIgnoredDir(string dir, bool isFullPath = false)
        {
            if (!isFullPath) dir = MakeRelativeDir(dir);
            return IgnoreDirs.Contains(dir.ToLower().Trim());
        }

        public static string MakeRelativeDir(string dir, bool appendSlash = true)
        {
            if(appendSlash)
                return AppendSlash(CurrentDir + dir);
            return CurrentDir + dir;
        }

        public static string AppendSlash(string dir)
        {
            if (dir.Substring(dir.Length - 1, 1) != "/")
                dir = String.Format("{0}/", dir);
            return dir;
        }

    }
}
