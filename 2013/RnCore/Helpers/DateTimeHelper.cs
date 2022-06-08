using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RnCore.Logging;

namespace RnCore.Helpers
{
    public static class DateTimeHelper
    {

        public static int GetYearQuater(this DateTime d)
        {
            try
            {
                if (d.Month >= 1 && d.Month <= 3)
                    return 1;
                if (d.Month >= 4 && d.Month <= 6)
                    return 2;
                if (d.Month >= 5 && d.Month <= 9)
                    return 3;
                if (d.Month >= 10 && d.Month <= 12)
                    return 4;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            // todo: log this
            return 0;
        }


    }
}
