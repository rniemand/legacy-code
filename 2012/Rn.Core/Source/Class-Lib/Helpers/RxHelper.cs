using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Rn.Core.Logging;

namespace Rn.Core.Helpers
{
    public static class RxHelper
    {

        public static bool RxIsMatch(this string s, string rxPattern)
        {
            return Regex.IsMatch(s, rxPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public static string RxGetMatch(this string s, string rxPattern, int groupNo = 1, string defaultValue = "")
        {
            if (!s.RxIsMatch(rxPattern)) return defaultValue;

            try
            {
                var match = Regex.Match(s, rxPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                return match.Groups[groupNo].Value;
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return defaultValue;
            }
        }


    }
}
