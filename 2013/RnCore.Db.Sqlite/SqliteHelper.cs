using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using RnCore.Helpers;

namespace RnCore.Db.Sqlite
{
    public static class SqliteHelper
    {

        public static string GetString(this SQLiteDataReader r, string columnName, string defValue = "")
        {
            try
            {
                return r[columnName].ToString();
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return defValue;
            }
        }

        public static int GetInt(this SQLiteDataReader r, string columnName, int defValue = 0)
        {
            try
            {
                return r.GetString(columnName, defValue.ToString(CultureInfo.InvariantCulture)).AsInt();
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return defValue;
            }
        }

        public static double GetDbl(this SQLiteDataReader r, string columnName, double defValue = 0)
        {
            try
            {
                return r.GetString(columnName, defValue.ToString(CultureInfo.InvariantCulture)).AsDbl();
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return defValue;
            }
        }

    }
}
