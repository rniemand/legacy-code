using System;
using System.Xml;
using Rn.Helpers;
using Rn.Logging;

namespace AlertProcessor.Classes
{
    public class RxPatternGroup
    {
        public bool Enabled { get; private set; }
        public int GroupNo { get; private set; }
        public AlertField MapsTo { get; private set; }
        public MappingType Type { get; private set; }
        public string DefaultValue { get; private set; }
        public bool MapDefaultValue { get; private set; }

        public RxPatternGroup(XmlNode n)
        {
            try
            {
                Enabled = n.GetAttribute("Enabled", false);
                GroupNo = n.GetAttribute("GroupNo", 0);
                MapsTo = n.GetAttribute("MapsTo", "").ToAlertField();
                Type = n.GetAttribute("Type", "string").ToMappingType();
                DefaultValue = n.GetAttribute("DefaultValue", "");
                MapDefaultValue = !String.IsNullOrEmpty(DefaultValue);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "Error creating a new instance of 'RxPatternGroup': {0}",
                    ex.Message));
            }
        }

    }
}
