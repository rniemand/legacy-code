using System;
using System.Text.RegularExpressions;
using Rn.Logging;

namespace AlertProcessor.Classes
{
    public static class Helper
    {
        public static int ToInt(this object s, int defaultValue = 0)
        {
            try
            {
                return int.Parse(s.ToString().Trim());
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static DateTime ToDateTime(this object o)
        {
            try
            {
                return DateTime.Parse(o.ToString().Trim());
            }
            catch (Exception)
            {
                return DateTime.Now.AddYears(-3);
            }
        }

        public static bool MatchesRx(this string s, string rx)
        {
            return Regex.IsMatch(s, rx, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public static Match GetMatch(this string s, string rx)
        {
            return Regex.Match(s, rx, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public static string GetGroupString(this Match m, int groupNo = 1)
        {
            return m.Groups[groupNo].Value.Trim();
        }

        public static int GetGroupInt(this Match m, int groupNo = 1)
        {
            return m.Groups[groupNo].Value.Trim().ToInt();
        }

        public static AlertField ToAlertField(this string s)
        {
            switch (s.ToLower().Trim())
            {
                case "originaldescription":
                    return AlertField.OriginalDescription;
                case "alertname":
                    return AlertField.AlertName;
                case "severity":
                    return AlertField.Severity;
                case "priority":
                    return AlertField.Priority;
                case "category":
                    return AlertField.Category;
                case "raiseddatetime":
                    return AlertField.RaisedDateTime;
                case "repeatcount":
                    return AlertField.RepeatCount;
                case "lineno":
                    return AlertField.LineNo;
                case "linepos":
                    return AlertField.LinePos;
                case "scomscript":
                    return AlertField.ScomScript;
                case "scriptargs":
                    return AlertField.ScriptArgs;
                case "workflowname":
                    return AlertField.WorkflowName;
                case "instancename":
                    return AlertField.InstanceName;
                case "InstanceId":
                    return AlertField.InstanceId;
                case "managementgroupname":
                    return AlertField.ManagementGroupName;
                case "query":
                    return AlertField.Query;
                case "result":
                    return AlertField.Result;
                case "details":
                    return AlertField.Details;
                case "output":
                    return AlertField.Output;
                case "servername":
                    return AlertField.ServerName;
                case "source":
                    return AlertField.Source;
                default:
                    return AlertField.Result;
            }
        }

        public static MappingType ToMappingType(this string s)
        {
            switch (s.ToLower().Trim())
            {
                case"string":
                    return MappingType.String;
                case"int":
                    return MappingType.Int;
                case "datetime":
                    return MappingType.DateTime;
                default:
                    return MappingType.String;
            }
        }

        public static void MapAlertProperty(ScomAlert alert, AlertField field, string value, string defaultValue)
        {
            var cValue = (String.IsNullOrEmpty(value) ? defaultValue : value);

            switch (field)
            {
                case AlertField.AlertName:
                    alert.AlertName = value;
                    break;
                case AlertField.Category:
                    alert.Category = value;
                    break;
                case AlertField.Details:
                    alert.Details = value;
                    break;
                case AlertField.InstanceId:
                    alert.InstanceId = value;
                    break;
                case AlertField.InstanceName:
                    alert.InstanceName = value;
                    break;
                case AlertField.LineNo:
                    alert.LineNo = value.ToInt();
                    break;
                case AlertField.LinePos:
                    alert.LinePos = value.ToInt();
                    break;
                case AlertField.ManagementGroupName:
                    alert.ManagementGroupName = value;
                    break;
                case AlertField.Output:
                    alert.Output = value;
                    break;
                case AlertField.Priority:
                    alert.Priority = value.ToInt();
                    break;
                case AlertField.Query:
                    alert.Query = value;
                    break;
                case AlertField.RaisedDateTime:
                    alert.RaisedDateTime = value.ToDateTime();
                    break;
                case AlertField.RepeatCount:
                    alert.RepeatCount = value.ToInt();
                    break;
                case AlertField.Result:
                    alert.Result = value;
                    break;
                case AlertField.ScomScript:
                    alert.ScomScript = value;
                    break;
                case AlertField.ScriptArgs:
                    alert.ScriptArgs = value;
                    break;
                case AlertField.ServerName:
                    alert.ServerName = value;
                    break;
                case AlertField.Severity:
                    alert.Severity = value.ToInt();
                    break;
                case AlertField.WorkflowName:
                    alert.WorkflowName = value;
                    break;
                case AlertField.Source:
                    alert.Source = value;
                    return;
                default:
                    Logger.LogWarning(String.Format("Unknown mapping: {0}", field));
                    break;
            }
        }




        public static string CsvSafeLine(this object o, bool addComma = true)
        {
            var oo = (o == null ? "" : o.ToString());
            oo = oo.Replace("\"", "'");
            oo = String.Format("\"{0}\"", oo);

            if (addComma) return oo + ",";
            return oo;
        }

    }
}
