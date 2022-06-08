using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RnCore.Helpers;
using RnCore.Logging;

namespace FtpSync.Db
{
    public static class FtpSyncDb
    {
        public static string DbFilePath { get; private set; }
        public static bool Connected { get; private set; }
        private static SQLiteConnection _con;


        static FtpSyncDb()
        {
            try
            {
                DbFilePath = "./FtpSyncDb.db";
                OpenDbConnection();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private static void OpenDbConnection()
        {
            try
            {
                // todo: expand this later
                if (!File.Exists(DbFilePath))
                {
                    SQLiteConnection.CreateFile(DbFilePath);
                }

                var conStr = String.Format(@"Data Source={0};Version=3;", DbFilePath);
                _con = new SQLiteConnection(conStr);
                _con.Open();

                if (!CheckTableExists("ftp_fileInfo"))
                    CreateDbTables();

                Connected = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                Connected = false;
            }
        }

        public static bool CheckTableExists(string tableName)
        {
            try
            {
                var q = String.Format(
                    @"SELECT COUNT(1) as 'found' FROM sqlite_master WHERE type='table' AND name='{0}'",
                    tableName);

                using (var cmd = new SQLiteCommand(q, _con))
                {
                    var res = cmd.ExecuteReader();
                    if (res.HasRows && res.Read())
                    {
                        return res["found"].CastInt() == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return false;
        }

        private static void CreateDbTables()
        {
            try
            {
                var sql = File.ReadAllText("./CreateDb.sql");
                using (var cmd = new SQLiteCommand(sql,_con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public static void ExecuteNonQuery(SQLiteCommand cmd)
        {
            try
            {
                if (cmd.Connection == null)
                    cmd.Connection = _con;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public static SQLiteDataReader ExecuteReader(SQLiteCommand cmd)
        {
            try
            {
                if (cmd.Connection == null)
                    cmd.Connection = _con;
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return null;
        }

        public static SQLiteDataReader GetFirstResultRow(SQLiteCommand cmd)
        {
            SQLiteDataReader row = null;

            try
            {
                if (cmd.Connection == null)
                    cmd.Connection = _con;

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        row = reader;
                    }
                }
                
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return row;
        }

        public static SQLiteCommand NewSQLiteCommand(string command)
        {
            return new SQLiteCommand(command, _con);
        }

        public static long InsertAndGetRowId(SQLiteCommand cmd)
        {
            try
            {
                if (cmd.Connection == null)
                    cmd.Connection = _con;
                cmd.ExecuteNonQuery();
                return cmd.Connection.LastInsertRowId;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return 0;
        }



        public static string GetStringColumn(this SQLiteDataReader reader, string columnName, string defaultValue = "")
        {
            try
            {
                return reader[columnName].ToString();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return defaultValue;
        }

        public static int GetIntColumn(this SQLiteDataReader reader, string columnName, int defaultValue = 0)
        {
            try
            {
                return (int) reader[columnName];
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return defaultValue;
        }

        public static long GetLongColumn(this SQLiteDataReader reader, string columnName, long defaultValue = 0)
        {
            try
            {
                return (long) reader[columnName];
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return defaultValue;
        }

        public static bool GetBoolColumn(this SQLiteDataReader reader, string columnName, bool defaultValue = false)
        {
            try
            {
                var colValue = reader.GetStringColumn(columnName, defaultValue.ToString()).LowerTrim();

                switch (colValue)
                {
                    case "1":
                    case "true":
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
            }

            return defaultValue;
        }

        public static DateTime GetDateTimeColumn(this SQLiteDataReader reader, string columnName)
        {
            var defaultDate = new DateTime(1970, 1, 1);

            try
            {
                var strDate = reader.GetStringColumn(columnName);
                if (!String.IsNullOrEmpty(strDate))
                    if (DateTime.TryParse(strDate, out defaultDate))
                        return defaultDate;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return defaultDate;
        }



        public static long GetSingleResultAsLong(SQLiteCommand cmd, long defaultValue = 0, int columnId = 0)
        {
            try
            {
                if (cmd.Connection == null)
                    cmd.Connection = _con;

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        defaultValue = reader[columnId].ToString().CastLong();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return defaultValue;
        }

        public static long GetSingleResultAsLong(string cmd, long defaultValue = 0, int columnId = 0)
        {
            return GetSingleResultAsLong(new SQLiteCommand(cmd), defaultValue, columnId);
        }


    }
}
