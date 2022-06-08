using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rn.Logging;

namespace Rn.Helpers
{
    public static class StringHelper
    {

        public static bool ToBool(this object s, bool defaultValue = false)
        {
            switch (s.ToString().ToLower().Trim())
            {
                case "1":
                case "true":
                case "on":
                case "yes":
                    return true;

                case "0":
                case "false":
                case "off":
                case "no":
                    return false;

                default:
                    return defaultValue;
            }
        }

        public static int ToInt(this object s, int defaultValue = 0)
        {
            try
            {
                switch (s.ToString().ToLower())
                {
                    case "true":
                        return 1;
                    case "false":
                        return 0;
                    default:
                        return int.Parse(s.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "Error while trying to convert '{0}' to an int: {1}",
                    s,
                    ex.Message));

                return defaultValue;
            }
        }

        public static double ToDouble(this object o, double defaultValue = 0)
        {
            try
            {
                return double.Parse(o.ToString().Trim());
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "Could not convert '{0}' to a DOUBLE: {1}",
                    o, ex.Message));

                return defaultValue;
            }
        }

    }
}
