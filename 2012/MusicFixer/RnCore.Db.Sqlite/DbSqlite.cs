using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using RnCore.Helpers;

namespace RnCore.Db.Sqlite
{
    public class DbSqlite
    {
        // http://blog.tigrangasparian.com/2012/02/09/getting-started-with-sqlite-in-c-part-one/

        public bool Ready { get; private set; }
        public SQLiteConnection SqlConnection { get; private set; }


        public DbSqlite(string dbFilePath, bool createIfMissing = false)
        {
            if (!File.Exists(dbFilePath) && !createIfMissing)
            {
                RnLocale.LogEvent("rn.db.sqlite", "sqlite.0001", dbFilePath);
                return;
            }

            if (!File.Exists(dbFilePath) && !CreateDb(dbFilePath))
                return;

            try
            {
                RnLocale.LogEvent("rn.db.sqlite", "sqlite.002", dbFilePath);
                var conStr = String.Format(@"Data Source={0};Version=3;", dbFilePath);
                SqlConnection = new SQLiteConnection(conStr);
                SqlConnection.Open();
                Ready = true;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        public bool CreateDb(string filePath)
        {
            if (File.Exists(filePath))
            {
                RnLocale.LogEvent("rn.common", "common.002", filePath);
                return true;
            }

            try
            {
                SQLiteConnection.CreateFile(filePath);
                return true;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return false;
            }
        }


        public long ExecuteNonQuery(string sql)
        {
            try
            {
                var cmd = new SQLiteCommand(sql, SqlConnection);
                cmd.ExecuteNonQuery();
                return SqlConnection.LastInsertRowId;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return 0;
            }
        }

        public SQLiteDataReader ExecuteReader(string sql)
        {
            try
            {
                var cmd = new SQLiteCommand(sql, SqlConnection);
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return null;
            }
        }

        

        public string GetSingleResult(string sql)
        {
            if(String.IsNullOrEmpty(sql))
            {
                RnLocale.LogEvent("rn.db.sqlite", "sqlite.003");
                return String.Empty;
            }

            try
            {
                using (var reader = ExecuteReader(sql))
                {
                    if (reader.HasRows && reader.Read())
                        return reader[0].ToString();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }

            return String.Empty;
        }

        public int GetSingleResultInt(string sql, int dValue = 0)
        {
            var res = GetSingleResult(sql);
            return String.IsNullOrEmpty(res) ? dValue : res.AsInt();
        }



    }
}
