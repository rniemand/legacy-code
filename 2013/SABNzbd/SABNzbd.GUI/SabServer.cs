using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using RichardTestJson.Helpers;

namespace RichardTestJson
{
    class SabServer
    {
        public string Username { get; private set; }
        public string Enable { get; private set; }
        public string Name { get; private set; }
        public string Fillserver { get; private set; }
        public string Connections { get; private set; }
        public string Ssl { get; private set; }
        public string Host { get; private set; }
        public string Timeout { get; private set; }
        public string Optional { get; private set; }
        public string Port { get; private set; }
        public string Retention { get; private set; }



        public SabServer(XmlNode n)
        {
            try
            {
                Username = n.GetNodeValString("username");
                Enable = n.GetNodeValString("enable");
                Name = n.GetNodeValString("name");
                Fillserver = n.GetNodeValString("fillserver");
                Connections = n.GetNodeValString("connections");
                Ssl = n.GetNodeValString("ssl");
                Host = n.GetNodeValString("host");
                Timeout = n.GetNodeValString("timeout");
                Optional = n.GetNodeValString("optional");
                Port = n.GetNodeValString("port");
                Retention = n.GetNodeValString("retention");
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }

        }
    }
}
