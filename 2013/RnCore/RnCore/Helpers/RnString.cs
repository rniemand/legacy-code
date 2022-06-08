using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RnCore.Logging;

namespace RnCore.Helpers
{
    public static class RnString
    {

        public static bool AsBoolean(this object o, bool defaultValue = false)
        {
            try
            {
                switch (o.ToString().ToLower().Trim())
                {
                    case "true":
                    case "1":
                        return true;

                    case "0":
                    case "false":
                        return false;

                    default:
                        return defaultValue;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                return defaultValue;
            }
        }

        public static int AsInt(this object o, int defaultValue = 0)
        {
            try
            {
                return int.Parse(o.ToString().ToLower().Trim());
            }
            catch (Exception ex)
            {
                ex.LogException();
                return defaultValue;
            }
        }

        public static double AsDouble(this object o, double defaultValue = 0)
        {
            try
            {
                return double.Parse(o.ToString().ToLower().Trim());
            }
            catch (Exception ex)
            {
                ex.LogException();
                return defaultValue;
            }
        }


    }
}
