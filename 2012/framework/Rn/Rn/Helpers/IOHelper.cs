using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Rn.Logging;

namespace Rn.Helpers
{
    public static class IOHelper
    {
        public static Random Rand { get; private set; }

        static IOHelper()
        {
            Rand = new Random(5423);
        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public static void CreateDirectory(string path)
        {
            if (DirectoryExists(path)) return;

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "Error while trying to create the directory '{0}': {1}",
                    path, ex.Message));
            }
        }

        public static string ReadAllText(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "Error getting lines from '{0}': {1}",
                    filePath, ex.Message));

                return "";
            }
        }
    }
}
