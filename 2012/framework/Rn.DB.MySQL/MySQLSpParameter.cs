using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Db;
using Rn.Helpers;
using Rn.Logging;

namespace Rn.DB.MySQL
{
    public class MySQLSpParameter
    {
        public string Name { get; internal set; }
        public DBParamType Type { get; internal set; }
        public int MaxLength { get; internal set; }
        public string DefaultValue { get; internal set; }
        public string Value { get; internal set; }
        public string CommandTextName { get; internal set; }

        public MySQLSpParameter(XmlNode n)
        {
            try
            {
                // todo - complete this properly
                Name = n.GetAttribute("Name", "");
                Type = DBParamType.Varchar;
                MaxLength = n.GetAttribute("MaxLength", -1);
                DefaultValue = n.GetAttribute("DefaultValue", "");
                Value = n.GetAttribute("DefaultValue", "");
                CommandTextName = String.Format("@{{{0}}}", Name);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "Error creating a new instance of 'MySQLSpParameter': {0}",
                    ex.Message));
            }
        }

        public void SetValue(string value)
        {
            Value = value;
        }

    }
}
