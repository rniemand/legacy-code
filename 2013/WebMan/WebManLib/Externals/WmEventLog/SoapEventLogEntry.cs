using System;
using System.Collections.Generic;
using System.Diagnostics;
using RnCore.Logging;

namespace Rn.WebManLib.Externals.WmEventLog
{
    public class SoapEventLogEntry
    {
        public string Category { get; set; }
        public short CategoryNumber { get; set; }
        public string EntryType { get; set; }
        public int Index { get; set; }
        public long EventId { get; set; }
        public long InstanceId { get; set; }
        public string MachineName { get; set; }
        public string Message { get; set; }
        public List<string> ReplacementStrings { get; set; }
        public string Site { get; set; }
        public string Source { get; set; }
        public DateTime TimeGenerated { get; set; }
        public DateTime TimeWritten { get; set; }
        public string UserName { get; set; }

        public SoapEventLogEntry()
        {
            // holder
        }

        public SoapEventLogEntry(EventLogEntry entry, bool includeMessageInfo = false)
        {
            try
            {
                ReplacementStrings = new List<string>();

                // http://social.msdn.microsoft.com/Forums/en-US/csharplanguage/thread/a76f64c3-a3df-4302-820c-1ad28fdf8103/
                EventId = entry.InstanceId & 0x3FFFFFFF;
                Category = entry.Category;
                CategoryNumber = entry.CategoryNumber;
                EntryType = entry.EntryType.ToString();
                Index = entry.Index;
                InstanceId = entry.InstanceId;
                MachineName = entry.MachineName;
                
                if (includeMessageInfo)
                {
                    Message = entry.Message;
                    if (entry.ReplacementStrings.Length > 0)
                        foreach (var s in entry.ReplacementStrings)
                            ReplacementStrings.Add(s);
                }
                else
                {
                    Message = "";
                }

                Site = entry.Site == null ? "" : entry.Site.ToString();
                Source = entry.Source;
                TimeGenerated = entry.TimeGenerated;
                TimeWritten = entry.TimeWritten;
                UserName = entry.UserName;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }


    }
}
