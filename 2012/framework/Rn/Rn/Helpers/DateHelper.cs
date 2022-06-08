using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.Helpers
{
    public static class DateHelper
    {
        public static string GetFullDate()
        {
            var sb = new StringBuilder();
            var dt = DateTime.Now;

            sb.Append(dt.Year.ToString());
            sb.Append("-");
            sb.Append(dt.Month.ToString().PadLeft(2, '0'));
            sb.Append("-");
            sb.Append(dt.Day.ToString().PadLeft(2, '0'));
            sb.Append(" ");
            sb.Append(dt.Hour.ToString().PadLeft(2, '0'));
            sb.Append(":");
            sb.Append(dt.Minute.ToString().PadLeft(2, '0'));
            sb.Append(":");
            sb.Append(dt.Second.ToString().PadLeft(2, '0'));

            return sb.ToString();
        }

        public static string GetDate()
        {
            var sb = new StringBuilder();
            var dt = DateTime.Now;

            sb.Append(dt.Year.ToString());
            sb.Append("-");
            sb.Append(dt.Month.ToString().PadLeft(2, '0'));
            sb.Append("-");
            sb.Append(dt.Day.ToString().PadLeft(2, '0'));

            return sb.ToString();
        }

        public static DateTime ToDate(this object o)
        {
            try
            {
                return DateTime.Parse(o.ToString().ToLower());
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        public static int MonthAsInt(string month)
        {
            switch (month.ToLower().Trim())
            {
                case "january":
                    return 1;
                case "february":
                    return 2;
                case "march":
                    return 3;
                case "april":
                    return 4;
                case "may":
                    return 5;
                case "june":
                    return 6;
                case "july":
                    return 7;
                case "august":
                    return 8;
                case "september":
                    return 9;
                case "october":
                    return 10;
                case "november":
                    return 11;
                case "december":
                    return 12;
                default:
                    // todo - add this to the logger
                    return 0;
            }
        }

    }
}
