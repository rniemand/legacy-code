using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rn.WebManLib.Externals.FileBrowser;
using Rn.WebManLib.Interfaces;
using RnCore.Logging;

namespace Rn.WebManLib
{
    public class FileBrowser : IFileBrowser
    {


        public string HelloWorld()
        {
            return "Hello World";
        }

        public List<SoapDriveInfo> ListDrives()
        {
            var foundDrives = new List<SoapDriveInfo>();

            try
            {
                var drives = DriveInfo.GetDrives();

                foreach (DriveInfo d in drives)
                {
                    foundDrives.Add(new SoapDriveInfo
                        {
                            AvailableFreeSpace = d.AvailableFreeSpace,
                            DriveFormat = d.DriveFormat,
                            DriveType = d.DriveType.ToString(),
                            IsReady = d.IsReady,
                            Name = d.Name,
                            TotalFreeSpace = d.TotalFreeSpace,
                            TotalSize = d.TotalSize,
                            VolumeLabel = d.VolumeLabel
                        });
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return foundDrives;
        }

        public List<SoapDirectoryInfo> ListFolders(string path)
        {
            var foundDirs = new List<SoapDirectoryInfo>();

            try
            {
                var di = new DirectoryInfo(path);
                var dirs = di.GetDirectories();

                foreach (DirectoryInfo dir in dirs)
                {
                    foundDirs.Add(new SoapDirectoryInfo
                        {
                            Attributes = dir.Attributes.ToString(),
                            CreationTime = dir.CreationTime,
                            CreationTimeUtc = dir.CreationTimeUtc,
                            Exists = dir.Exists,
                            Extension = di.Extension,
                            FullName = dir.FullName,
                            LastAccessTime = dir.LastAccessTime,
                            LastAccessTimeUtc = dir.LastAccessTimeUtc,
                            LastWriteTime = dir.LastWriteTime,
                            LastWriteTimeUtc = dir.LastWriteTimeUtc,
                            Name = dir.Name
                        });
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return foundDirs;
        }

    }
}
