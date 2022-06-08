using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using RichardTestJson.Helpers;

namespace RichardTestJson
{
    class SabJob
    {
        public string TimeLeft { get; private set; }
        public double PercentComplete { get; private set; }
        public string FileName { get; private set; }
        public string Id { get; private set; }
        public double Mb { get; private set; }
        public double MbLeft { get; private set; }

        public SabJob(XmlNode n)
        {
            try
            {
                TimeLeft = n.GetNodeValString("timeleft");
                Mb = n.GetNodeValDouble("mb");
                FileName = n.GetNodeValString("filename");
                MbLeft = n.GetNodeValDouble("mbleft");
                Id = n.GetNodeValString("id");
                PercentComplete = Math.Round((100 - ((MbLeft/Mb)*100)), 2);
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }

    }
}
