using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Core.Helpers;
using Rn.Core.Logging;

namespace Rn.TvDb
{
    public class TvDBMirror
    {
        public int Id { get; internal set; }
        public int TypeMask { get; internal set; }
        public string Url { get; internal set; }

        public TvDBMirror(XmlNode n)
        {
            Id = n.GetNodeValueInt("id");
            TypeMask = n.GetNodeValueInt("typemask");
            Url = n.GetNodeValue("mirrorpath");

            if (Url.Substring(Url.Length - 1, 1) != "/")
                Url = String.Format("{0}/", Url);

            Locale.LogEvent("tvdb", "0003", Id, TypeMask, Url);
        }

    }
}
