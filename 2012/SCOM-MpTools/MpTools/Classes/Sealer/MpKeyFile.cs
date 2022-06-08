using System;
using System.IO;
using System.Xml;
using Rn.Helpers;
using Rn.Logging;

namespace MpTools.Classes.Sealer
{
    public class MpKeyFile
    {
        public string Name { get; private set; }
        public bool Enabled { get; private set; }
        public bool Default { get; private set; }
        public int Id { get; private set; }
        public string Path { get; private set; }
        public bool PathValid { get; private set; }

        public MpKeyFile(XmlElement n)
        {
            try
            {
                Name = n.GetAttribute("Name", "");
                Enabled = n.GetAttribute("Enabled", true);
                Default = n.GetAttribute("Default", false);
                Id = n.GetAttribute("Id", 0);
                Path = n.GetAttribute("Path", "");
                PathValid = File.Exists(Path);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "There was an error creating a new instance of MpKeyFile: {0}",
                    ex.Message));
            }
        }
        
    }
}
