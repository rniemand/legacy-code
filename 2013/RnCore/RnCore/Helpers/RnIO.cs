using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RnCore.Logging;

namespace RnCore.Helpers
{
    public static class RnIO
    {
        public static string BasePath { get; private set; }


        static RnIO()
        {
            WorkBasePath();
        }

        private static void WorkBasePath()
        {
            try
            {
                //Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().Location);
                BasePath = AppDomain.CurrentDomain.BaseDirectory;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }


        public static bool WriteToFile(string filePath, string fileContents, bool overwrite = false)
        {
            try
            {
                RnLogger.Loggers.LogDebug("Attempting to write to the file '{0}'", 107, filePath);

                if (File.Exists(filePath) && !overwrite)
                {
                    RnLogger.Loggers.LogWarning(
                        "The file '{0}' already exists, overwrite set to false, skipping write", 108, filePath);
                    return false;
                }

                if (File.Exists(filePath) && !DeleteFile(filePath))
                {
                    RnLogger.Loggers.LogWarning("Write to '{0}' failed, the file exists but could not be replaced", 110,
                                                filePath);
                    return false;
                }

                File.WriteAllText(filePath, fileContents);
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return true;

                File.Delete(filePath);
                RnLogger.Loggers.LogDebug("Successfully deleted the file '{0}'", 109, filePath);
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }


    }
}
