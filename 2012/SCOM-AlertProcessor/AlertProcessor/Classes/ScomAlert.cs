using System;

namespace AlertProcessor.Classes
{
    public class ScomAlert
    {
        // SCOM DB Fields
        public string OriginalDescription { get; set; }
        public string AlertName { get; set; }
        public int Severity { get; set; }
        public int Priority { get; set; }
        public string Category { get; set; }
        public DateTime RaisedDateTime { get; set; }
        public int RepeatCount { get; set; }

        // Useful
        public bool AlertProcessed { get; set; }

        public int LineNo { get; set; }
        public int LinePos { get; set; }
        public string ScomScript { get; set; }
        public string ScriptArgs { get; set; }
        public string WorkflowName { get; set; }
        public string InstanceName { get; set; }
        public string InstanceId { get; set; }
        public string ManagementGroupName { get; set; }
        public string Query { get; set; }
        public string Result { get; set; }
        public string Details { get; set; }
        public string Output { get; set; }
        public string ServerName { get; set; }
        public string Source { get; set; }
    }
}
