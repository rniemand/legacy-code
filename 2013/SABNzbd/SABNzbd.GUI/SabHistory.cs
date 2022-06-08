using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using RichardTestJson.Helpers;

namespace RichardTestJson
{
    class SabHistory
    {
        public string ShowDetails { get; private set; }
        public string Meta { get; private set; }
        public string FailMessage { get; private set; }
        public string Id { get; private set; }
        public string Size { get; private set; }
        public string Category { get; private set; }
        public string Pp { get; private set; }
        public string Retry { get; private set; }
        public string Completeness { get; private set; }
        public string Script { get; private set; }
        public string NzbName { get; private set; }
        public string DownloadTime { get; private set; }
        public string Storage { get; private set; }
        public string Status { get; private set; }
        public string Completed { get; private set; }
        public string NzoId { get; private set; }
        public string Downloaded { get; private set; }
        public string Path { get; private set; }
        public string PostprocTime { get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }
        public string Bytes { get; private set; }


        public SabHistory(XmlNode n)
        {
            try
            {
                ShowDetails = n.GetNodeValString("show_details");
                Meta = n.GetNodeValString("meta");
                FailMessage = n.GetNodeValString("loaded");
                Id = n.GetNodeValString("id");
                Size = n.GetNodeValString("size");
                Category = n.GetNodeValString("category");
                Pp = n.GetNodeValString("pp");
                Retry = n.GetNodeValString("retry");
                Completeness = n.GetNodeValString("completeness");
                Script = n.GetNodeValString("script");
                NzbName = n.GetNodeValString("nzb_name");
                DownloadTime = n.GetNodeValString("download_time");
                Storage = n.GetNodeValString("storage");
                Status = n.GetNodeValString("status");
                Completed = n.GetNodeValString("completed");
                NzoId = n.GetNodeValString("nzo_id");
                Downloaded = n.GetNodeValString("downloaded");
                Path = n.GetNodeValString("path");
                PostprocTime = n.GetNodeValString("postproc_time");
                Name = n.GetNodeValString("name");
                Url = n.GetNodeValString("url");
                Bytes = n.GetNodeValString("bytes");
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }


    }
}

