using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RnCore.Helpers;

namespace RnCore.Db.Sqlite
{
    public static class SqliteFactory
    {
        private static readonly Dictionary<string, DbSqlite> Dbs;

        static SqliteFactory()
        {
            Dbs = new Dictionary<string, DbSqlite>();
        }


        public static bool HasDb(string dbName = "default")
        {
            return Dbs.Count != 0 && Dbs.ContainsKey(dbName);
        }

        public static DbSqlite GetDb(string dbName = "default")
        {
            if (!HasDb(dbName))
                return null;
            return Dbs[dbName];
        }

        public static DbSqlite RegisterDb(string dbFilePath, bool createIfMissing = true, string dbName = "default")
        {
            if (HasDb(dbName))
                return GetDb(dbName);

            try
            {
                Dbs.Add(dbName, new DbSqlite(dbFilePath, createIfMissing));
                return GetDb(dbName);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return null;
            }
        }

    }
}
