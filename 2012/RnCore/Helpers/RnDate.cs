using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RnCore.Helpers
{
    public static class RnDate
    {

        public static string FormatDate(string mask, DateTime date)
        {
            var completed = new StringBuilder();

            foreach (var c in mask)
                completed.Append(ReplaceChar(c, date));

            return completed.ToString();
        }

        public static string FormatDate(string mask)
        {
            return FormatDate(mask, DateTime.Now);
        }

        private static string ReplaceChar(char c, DateTime date)
        {

            switch (c)
            {
                // ------------------ DAY ------------------
                case 'd':
                    return date.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
                case 'D':
                    return date.DayOfWeek.ToString().Substring(0, 3);
                case 'j':
                    return date.Day.ToString(CultureInfo.InvariantCulture);
                case 'l':
                    return date.DayOfWeek.ToString();
                case 'N':
                    return ((int)date.DayOfWeek).ToString(CultureInfo.InvariantCulture);
                case 'S':
                    return GetOrdinalSuffix(date);
                case 'z':
                    return date.DayOfYear.ToString(CultureInfo.InvariantCulture);

                // ------------------ WEEK ------------------
                case 'W':
                    return date.WeekNumber().ToString(CultureInfo.InvariantCulture);

                // ------------------ MONTH ------------------
                case 'F':
                    return date.GetMonthName();
                case 'm':
                    return date.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
                case 'M':
                    return date.GetMonthName().Substring(0, 3);
                case 'n':
                    return date.Month.ToString(CultureInfo.InvariantCulture);
                case 't':
                    return DateTime.DaysInMonth(date.Year, date.Month).ToString(CultureInfo.InvariantCulture);

                // ------------------ YEAR ------------------
                case 'L':
                    return (DateTime.IsLeapYear(date.Year) ? "1" : "0");
                case 'Y':
                    return date.Year.ToString(CultureInfo.InvariantCulture);
                case 'y':
                    return date.Year.ToString(CultureInfo.InvariantCulture).Substring(2, 2);

                // ------------------ TIME ------------------
                case 'a':
                    return date.GetAmPm();
                case 'A':
                    return date.GetAmPm().ToUpper();
                case 'g':
                    return date.Format12Hours().ToString(CultureInfo.InvariantCulture);
                case 'G':
                    return date.Hour.ToString(CultureInfo.InvariantCulture);
                case 'h':
                    return date.Format12Hours().ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
                case 'H':
                    return date.Hour.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
                case 'i':
                    return date.Minute.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
                case 's':
                    return date.Second.ToString(CultureInfo.InstalledUICulture).PadLeft(2, '0');
                case 'u':
                    return date.Millisecond.ToString(CultureInfo.InvariantCulture);

                default:
                    return c.ToString(CultureInfo.InvariantCulture);
            }
        }

        public static string GetOrdinalSuffix(this DateTime date)
        {
            // Thanks to - http://www.wduffy.co.uk/blog/ordinal-suffix-datetime-extension-method/

            if (date.Day % 100 >= 11 && date.Day % 100 <= 13)
                return "th";

            switch (date.Day % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }

        public static int WeekNumber(this DateTime value)
        {
            // Thanks to - http://www2.suddenelfilio.net/2010/10/07/how-to-get-the-week-number-for-a-date-in-c/

            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                value, CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);

        }

        public static string GetMonthName(this DateTime date)
        {
            // Thanks to - http://forums.asp.net/t/192751.aspx/1
            var formatInfoinfo = new DateTimeFormatInfo();
            var monthName = formatInfoinfo.MonthNames;
            return monthName[date.Month - 1];
        }

        public static string GetAmPm(this DateTime date)
        {
            return date.Hour < 12 ? "am" : "pm";
        }

        public static int Format12Hours(this DateTime date)
        {
            if (date.Hour > 12)
                return date.Hour - 12;

            return date.Hour;
        }


    }
}
