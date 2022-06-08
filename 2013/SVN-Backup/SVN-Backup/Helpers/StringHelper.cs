using System;
using System.Globalization;

namespace SVN_Backup.Helpers
{
    public static class StringHelper
    {

        public static int AsInt(this double n, int defValue = 0)
        {
            try
            {
                return int.Parse(n.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return defValue;
        }

        public static bool AsBool(this object o, bool defValue = false)
        {
            try
            {
                switch (o.ToString().Trim().ToLower())
                {
                    case "0":
                    case "flase":
                    case "no":
                        return false;
                    case "1":
                    case "true":
                    case "yes":
                        return true;
                    default:
                        return defValue;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return defValue;
        }

    }
}
