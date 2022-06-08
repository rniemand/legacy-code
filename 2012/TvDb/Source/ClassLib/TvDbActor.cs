using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Core.Helpers;
using Rn.Core.Logging;

namespace Rn.TvDb
{
    public class TvDbActor
    {
        public int Id { get; private set; }
        public string Image { get; private set; }
        public string Name { get; private set; }
        public string[] Roles { get; private set; }
        public int SortOrder { get; private set; }

        public TvDbActor(XmlNode n)
        {
            Id = n.GetNodeValueInt("id");
            Image = n.GetNodeValue("Image");
            Name = n.GetNodeValue("Name");
            Roles = n.GetNodeValue("Role").Split('|');
            SortOrder = n.GetNodeValueInt("SortOrder");
            Locale.LogEvent("rn.core.common", "0003", String.Format("TvDbActor ({0})", Name));
        }
    }
}
