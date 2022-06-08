using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Rn.Core.Logging
{
    public static class SvcLogger
    {
        public static string LogPath { get; private set; }
        private static FileStream _fs;
        private static StreamWriter _sw;

        static SvcLogger()
        {
            LogPath = @"C:\CamCopy.log";
            OpenFile();
        }

        private static void OpenFile()
        {
            if (File.Exists(LogPath))
                File.Delete(LogPath);

            _fs = new FileStream(LogPath, FileMode.OpenOrCreate);
            _sw = new StreamWriter(_fs);

            LogDebug("Service Logger created...");
        }

        private static void LogMsg(string msg)
        {
            _sw.WriteLine(msg);
            _sw.Flush();
            _fs.Flush();
        }


        public static void LogDebug(string msg)
        {
            LogMsg(String.Format("INFO: {0}", msg));
        }
    }
}
