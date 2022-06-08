using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using Rn.Core.Helpers;
using Rn.Core.Logging;

namespace CamCopy
{
    public class CamDumpFolder
    {
        public string WatcherName { get; private set; }
        public string WatchDir { get; private set; }
        public string TargetFile { get; private set; }
        public string FileFilter { get; private set; }
        public bool DeleteSourceFile { get; private set; }
        public bool Ready { get; private set; }
        public bool OverwriteTarget { get; private set; }

        private DirectoryInfo _watchDir;
        

        public CamDumpFolder(XmlNode n)
        {
            try
            {
                WatcherName = n.GetAttribute("Name");
                WatchDir = n.GetKeyValue("WatchDir");
                TargetFile = n.GetKeyValue("TargetFile");
                FileFilter = n.GetKeyValue("FileFilter");
                DeleteSourceFile = n.GetKeyValueBool("DeleteSource");
                OverwriteTarget = n.GetKeyValueBool("OverwriteTarget");

                CheckFolder();
                CreateWatcher();
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format("Error creating a new instance of 'CamDumpFolder': {0}", ex.Message));
            }
        }


        private void CheckFolder()
        {
            if (!Directory.Exists(WatchDir))
                return;

            try
            {
                _watchDir = new DirectoryInfo(WatchDir);
                CleanWatchDir();
                Ready = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format("Error creating the DirectoryInfo object for '{0}': {1}", WatchDir,
                                              ex.Message));
            }
        }

        private void CleanWatchDir()
        {
            if (!DeleteSourceFile) return;

            try
            {
                _watchDir.Refresh();
                var files = _watchDir.GetFiles(FileFilter);
                if (files.Length == 0) return;

                foreach (var f in files)
                    IOHelper.DeleteFile(f.FullName);

                Logger.LogDebug(String.Format("Removed '{0}' files from '{1}'", files.Length, WatchDir));
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format("Error cleaning up source dir: {0}", ex.Message));
            }
        }

        private void CreateWatcher()
        {
            if(!Ready) return;

            try
            {
                // Create the file system watcher
                var fsw = new FileSystemWatcher(WatchDir)
                              {
                                  Filter = FileFilter,
                                  EnableRaisingEvents = true
                              };
                fsw.Created += FswObjectCreated;

                // Ensure that the source dir is clean
                CleanWatchDir();
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format("Error creating watcher: {0}", ex.Message));
            }
        }

        void FswObjectCreated(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                IOHelper.CopyFile(e.FullPath, TargetFile, OverwriteTarget);

                if (DeleteSourceFile)
                {
                    Thread.Sleep(50);
                    IOHelper.DeleteFile(e.FullPath);
                    CleanWatchDir();
                }
            }
        }

    }
}
