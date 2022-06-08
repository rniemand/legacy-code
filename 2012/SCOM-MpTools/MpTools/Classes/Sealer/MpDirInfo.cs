using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Helpers;
using Rn.Logging;

namespace MpTools.Classes.Sealer
{
    public class MpDirInfo
    {
        public int Id { get; private set; }
        public bool Enabled { get; private set; }
        public string Path { get; private set; }
        public bool PathValid { get; private set; }

        public MpDirInfo(XmlNode n)
        {
            try
            {
                Id = n.GetAttribute("Id", 0);
                Enabled = n.GetAttribute("Enabled", false);
                Path = n.GetAttribute("Path", "");
                CheckPathIsValid();
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "An exception was thrown while creating a new instance of 'MpSourceDir': {0}",
                    ex.Message));
            }
        }

        private void CheckPathIsValid()
        {
            try
            {
                PathValid = Directory.Exists(Path);
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(
                    "Error while checking if '{0}' is a valid path: {1}",
                    Path, ex.Message));
            }
            
        }
    }
}
