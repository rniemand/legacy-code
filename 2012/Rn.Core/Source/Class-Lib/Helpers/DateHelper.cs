using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Rn.Core.Logging;

namespace Rn.Core.Helpers
{
    public static class DateHelper
    {
        
        public static string GetLongString(DateTime date)
        {
            var sb = new StringBuilder();
            
            sb.Append(date.Year.ToString());
            sb.Append("-");
            sb.Append(date.Month.ToString().PadLeft(2, '0'));
            sb.Append("-");
            sb.Append(date.Day.ToString().PadLeft(2, '0'));
            sb.Append(" ");
            sb.Append(date.Hour.ToString().PadLeft(2, '0'));
            sb.Append(":");
            sb.Append(date.Minute.ToString().PadLeft(2, '0'));
            sb.Append(":");
            sb.Append(date.Second.ToString().PadLeft(2, '0'));

            return sb.ToString();
        }

        public static string GetLongString()
        {
            return GetLongString(DateTime.Now);
        }

        public static string GetShortString(DateTime date)
        {
            var sb = new StringBuilder();

            sb.Append(date.Year.ToString());
            sb.Append("-");
            sb.Append(date.Month.ToString().PadLeft(2, '0'));
            sb.Append("-");
            sb.Append(date.Day.ToString().PadLeft(2, '0'));

            return sb.ToString();
        }

        public static string GetShortString()
        {
            return GetShortString(DateTime.Now);
        }

        public static string GetTimeString(DateTime date)
        {
            var sb = new StringBuilder();

            sb.Append(date.Hour.ToString().PadLeft(2, '0'));
            sb.Append(":");
            sb.Append(date.Minute.ToString().PadLeft(2, '0'));
            sb.Append(":");
            sb.Append(date.Second.ToString().PadLeft(2, '0'));

            return sb.ToString();
        }

        public static string GetTimeString()
        {
            return GetTimeString(DateTime.Now);
        }

        public static string AsFullDateString(this DateTime t)
        {
            var sb = new StringBuilder();
            sb.Append(t.Year);
            sb.Append("-");
            sb.Append((t.Month < 10 ? "0" + t.Month : t.Month.ToString()));
            sb.Append("-");
            sb.Append((t.Day < 10 ? "0" + t.Day : t.Day.ToString()));
            sb.Append(" ");
            sb.Append((t.Hour < 10 ? "0" + t.Hour : t.Hour.ToString()));
            sb.Append(":");
            sb.Append((t.Minute < 10 ? "0" + t.Minute : t.Minute.ToString()));
            sb.Append(":");
            sb.Append((t.Second < 10 ? "0" + t.Second : t.Second.ToString()));
            return sb.ToString();
        }

        public static DateTime AsDateTime(this string s)
        {
            try
            {
                return DateTime.Parse("s");
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return DateTime.Parse("Jan 1 1970");
            }
        }

        public static DateTime AsDateTime(this string s, string format)
        {
            try
            {
                return DateTime.ParseExact(s, format, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return DateTime.Parse("Jan 1 1970");
            }
            
        }

        public static double AsUnixTimeStamp(this DateTime dateTime)
        {
            try
            {
                return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return 0;
            }
        }

    }
}
