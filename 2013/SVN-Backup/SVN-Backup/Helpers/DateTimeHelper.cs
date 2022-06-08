using System;
using System.Globalization;

namespace SVN_Backup.Helpers
{
    public static class DateTimeHelper
    {

        public static int GetYearQuater(this DateTime d)
        {
            try
            {
                return Math.Ceiling((double) d.Month/3).AsInt();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            RnLogger.LogWarning("Unable to generate year quater from given date '{0}'",
                                d.ToString(CultureInfo.InvariantCulture));
            return 0;
        }

    }

}
