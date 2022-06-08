using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnReaderLib
{
    public static class RnIO
    {

        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return true;
                File.Delete(filePath);
                return !File.Exists(filePath);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return false;
        }

        public static bool WriteFile(string filePath, string fileContents, bool replaceFile = false, bool createDirectory = false)
        {
            try
            {
                if (File.Exists(filePath) & !replaceFile)
                    return false;

                if (createDirectory)
                    if (!CreateDirectory(Path.GetDirectoryName(filePath)))
                        return false;

                if (DeleteFile(filePath))
                {
                    File.WriteAllText(filePath, fileContents);
                    return File.Exists(filePath);    
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return false;
        }

        public static bool CreateDirectory(string dirPath)
        {
            try
            {
                if (Directory.Exists(dirPath))
                    return true;
                Directory.CreateDirectory(dirPath);
                
                return Directory.Exists(dirPath);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return false;
        }

        public static string ReadFileAsString(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return String.Empty;
        }

    }
}
