using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Helpers;
using Rn.Logging;

namespace AlertProcessor.Classes
{
    public class RxPatternStaticMappings
    {
        public bool Enabled { get; private set; }
        public AlertField Field { get; private set; }
        public string Value { get; private set; }
        public MappingType Type { get; private set; }
        public bool AllowOverwrite { get; private set; }

        public RxPatternStaticMappings(XmlNode n)
        {
            try
            {
                Enabled = n.GetAttribute("Enabled", false);
                Field = n.GetAttribute("Field", "").ToAlertField();
                Value = n.GetAttribute("Value", "");
                Type = n.GetAttribute("Type", "").ToMappingType();
                AllowOverwrite = n.GetAttribute("AllowOverwrite", false);
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(
                    "Error creating a new instance of 'RxPatternStaticMappings': {0}",
                    ex.Message));
            }
        }

    }
}
