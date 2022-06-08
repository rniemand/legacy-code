using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rn.Core.Logging;

namespace Rn.Core.Helpers
{
    public static class StringHelper
    {

        public static int AsInt(this object o, int defaultValue = 0)
        {
            try
            {
                return int.Parse(o.ToString());
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0004", o.ToString(), "Int", ex.Message);
                return defaultValue;
            }
        }

        public static bool AsBool(this object o, bool defaultValue = false)
        {
            switch (o.ToString().ToLower().Trim())
            {
                case "1":
                case "true":
                case "yes":
                    return true;

                case "0":
                case "false":
                case "no":
                    return false;

                default:
                    Locale.LogEvent("rn.core.common", "0004", o.ToString(), "Bool", "");
                    return false;
            }
        }

        public static long AsLong(this object o, long defaultValue = 0)
        {
            try
            {
                return long.Parse(o.ToString());
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0004", o.ToString(), "Long", ex.Message);
                return defaultValue;
            }
        }

        public static double AsDouble(this object o, double defaultValue = 0)
        {
            try
            {
                return double.Parse(o.ToString());
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0004", o.ToString(), "Double", ex.Message);
                return defaultValue;
            }
        }

    }
}
