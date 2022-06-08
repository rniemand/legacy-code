using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Rn.Helpers;
using Rn.Logging;

namespace AlertProcessor.Classes
{
    public class RxPattern
    {
        public XmlNode RxPatternNode { get; private set; }
        public string PatternName { get; private set; }
        public string Pattern { get; private set; }
        public bool Enabled { get; private set; }
        public List<RxPatternGroup> GroupMappings { get; private set; }
        public List<RxPatternStaticMappings> StaticMappings { get; private set; }


        // =====> Class constructor
        public RxPattern(XmlNode n)
        {
            GroupMappings = new List<RxPatternGroup>();
            StaticMappings = new List<RxPatternStaticMappings>();

            RxPatternNode = n;
            PatternName = n.GetAttribute("Name", "");
            Enabled = n.GetAttribute("Enabled", false);
            
            LoadRxPattern();
            LoadGroupMappings();
            LoadStaticMappings();
        }

        private void LoadGroupMappings()
        {
            var gNode = RxPatternNode.SelectNodes("RxMappings/Group[@Enabled='true']");
            if (gNode == null || gNode.Count == 0) return;
            foreach (XmlNode n in gNode)
                GroupMappings.Add(new RxPatternGroup(n));
            Logger.LogInfo(String.Format(
                "Successfully loaded '{0}' Group mappings for the RxPattern '{1}'",
                gNode.Count, PatternName));
        }

        private void LoadRxPattern()
        {
            try
            {
                var n = RxPatternNode.SelectSingleNode("RxPattern");
                if (n == null)
                {
                    Logger.LogWarning(String.Format(
                        "There is no RxPattern defined for the RxPatternEntry with the name '{0}'",
                        PatternName));
                    return;
                }
                Pattern = n.InnerText;
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(
                    "Error loading RxPattern '{0}': {1}",
                    PatternName, ex.Message));
            }
            
        }

        private void LoadStaticMappings()
        {
            try
            {
                var nodes = RxPatternNode.SelectNodes("StaticMappings/Mapping[@Enabled='true']");
                if (nodes == null || nodes.Count == 0) return;
                foreach (XmlNode n in nodes)
                    StaticMappings.Add(new RxPatternStaticMappings(n));
                Logger.LogInfo(String.Format(
                    "Successfully loaded '{0}' static mappings for the RxPattern '{1}'",
                    nodes.Count, PatternName));
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(
                    "Error loading Static Mapping for the RxPattern '{0}': {1}",
                    PatternName, ex.Message));
            }
        }


        // =====> Public Methods
        public bool IsMatch(string s)
        {
            return Regex.IsMatch(s, Pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public void ProcessAlert(ScomAlert alert)
        {
            var counter = 0;
            var match = alert.OriginalDescription.GetMatch(Pattern);

            // todo - static mappings - the ones that can change

            // Map known alert fields
            if (match.Groups.Count > 0)
                foreach (Group g in match.Groups)
                {
                    counter++;
                    if (counter == 1) continue;
                    TryMapValue(alert, g.Value.Trim(), counter - 1);
                }

            // todo - static mappings - the ones that cannot change

            // Mark the alert as processed
            alert.AlertProcessed = true;
        }

        private void TryMapValue(ScomAlert alert, string value, int groupNo)
        {
            if (GroupMappings.Count == 0)
                return;

            // do the mapping
            foreach (var gm in GroupMappings.Where(gm => gm.GroupNo == groupNo))
            {
                Helper.MapAlertProperty(alert, gm.MapsTo, value, gm.DefaultValue);
                return;
            }

            //Logger.LogWarning(String.Format("Unknown Group Mapping for RxPattern '{0}'. (Group: {1})",PatternName, groupNo));
        }
        
    }
}
