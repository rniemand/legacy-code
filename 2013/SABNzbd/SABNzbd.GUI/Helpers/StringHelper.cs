using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RichardTestJson.Helpers
{
    public static class StringHelper
    {
        public static double AsDouble(this object o, double defaultVal = 0)
        {
            try
            {
                return double.Parse(o.ToString().Trim().ToLower());
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
                SabLogger.Instance.LogError("Could not parse '{0}' as a double!", o);
                return defaultVal;
            }
        }

        public static float AsFloat(this object o, float defaultVal = 0)
        {
            try
            {
                return float.Parse(o.ToString());
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
                SabLogger.Instance.LogError("Could not parse '{0}' as a float!", o);
                return defaultVal;
            }
        }
    }
}
