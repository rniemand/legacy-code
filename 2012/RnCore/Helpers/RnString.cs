using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RnCore.Helpers
{
    public static class RnString
    {
        
        public static bool AsBool(this object o, bool defValue = false)
        {
            try
            {
                switch (o.ToString().ToLower().Trim())
                {
                    case "0":
                    case "false":
                    case "no":
                    case "off":
                        return false;

                    case "1":
                    case "true":
                    case "yes":
                    case "on":
                        return true;

                    default:
                        return defValue;
                }
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return defValue;
            }
        }

        public static int AsInt(this object o, int defValue = 0)
        {
            try
            {
                return int.Parse(o.ToString());
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return defValue;
            }
        }

        public static double AsDbl(this object o, double defValue = 0)
        {
            try
            {
                return double.Parse(o.ToString());
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return defValue;
            }
        }



        // Regular Expressions
        public static bool RxIsMatch(this string s, string rxPattern)
        {
            return Regex.IsMatch(s, rxPattern, RegexOptions.IgnoreCase);
        }

    }
}
