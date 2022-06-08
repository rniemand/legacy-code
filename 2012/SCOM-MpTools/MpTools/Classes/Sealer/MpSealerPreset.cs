using System;
using System.Xml;
using MpTools.Tools.Sealer;
using Rn.Configuration;
using Rn.Logging;
using Rn.Helpers;

namespace MpTools.Classes.Sealer
{
    public class MpSealerPreset
    {
        public string Name { get; private set; }
        public bool Default { get; private set; }
        public bool Enabled { get; private set; }
        public int SourceDirId { get; private set; }
        public int KeyFileId { get; private set; }
        public int OutDirId { get; private set; }
        public string CompanyName { get; private set; }
        public bool ZipMps { get; private set; }
        public string ZipPath { get; private set; }


        public MpSealerPreset(XmlNode n)
        {
            try
            {
                Name = n.GetAttribute("Name", "");
                Enabled = n.GetAttribute("Enabled", true);
                Default = n.GetAttribute("Default", false);
                SourceDirId = n.GetKetAttributeInt("MPSourceDir");
                KeyFileId = n.GetKetAttributeInt("KeyFile");
                OutDirId = n.GetKetAttributeInt("MPOutputDir");
                CompanyName = n.GetKeyAttribute("Company");
                ZipMps = n.GetKetAttributeBool("ZipMps");
                ZipPath = n.GetKeyAttribute("ZipDir");
                Logger.LogDebug(String.Format("New instance of 'MpSealerPreset': {0}", Name));
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "Error creating a new instance of 'MpSealerPreset': {0}",
                    ex.Message));
            }
        }

        // todo - use a set key value function

        public void SetCompanyName(string name)
        {
            CompanyName = name;
            if(MpSealerStatic.FormLoading) return;
            var xpath = String.Format("/MpToolsConfig/MpSealerPresets/Preset[@Name='{0}']/Key[@Name='Company']", Name);
            ConfigObj.GetConfig().SetNodeAttributeValue(xpath, "Value", name);
        }

        public void SetMpSourceDIr(int id)
        {
            if (MpSealerStatic.FormLoading) return;
            SourceDirId = id;
            var xpath = String.Format("/MpToolsConfig/MpSealerPresets/Preset[@Name='{0}']/Key[@Name='MPSourceDir']", Name);
            ConfigObj.GetConfig().SetNodeAttributeValue(xpath, "Value", id.ToString());
        }

        public void SetKeyFile(int id)
        {
            if (MpSealerStatic.FormLoading) return;
            KeyFileId = id;
            var xpath = String.Format("/MpToolsConfig/MpSealerPresets/Preset[@Name='{0}']/Key[@Name='KeyFile']", Name);
            ConfigObj.GetConfig().SetNodeAttributeValue(xpath, "Value", id.ToString());
        }

        public void SetOutputDir(int id)
        {
            if (MpSealerStatic.FormLoading) return;
            OutDirId = id;
            var xpath = String.Format("/MpToolsConfig/MpSealerPresets/Preset[@Name='{0}']/Key[@Name='MPOutputDir']", Name);
            ConfigObj.GetConfig().SetNodeAttributeValue(xpath, "Value", id.ToString());
        }

    }
}
