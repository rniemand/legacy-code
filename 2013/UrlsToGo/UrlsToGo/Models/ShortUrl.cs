using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using UrlsToGo.RnUtils;

namespace UrlsToGo.Models
{
    public class ShortUrl
    {
        public int UrlId { get; set; }
        public int HitCount { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateLastUsed { get; set; }
        public string ShortCode { get; set; }
        public string UrlFull { get; set; }
        public string ShortUrlFull { get; set; }

        private bool _isEncoded ;
        private bool _loadedFromDb;


        public ShortUrl(string url)
        {
            try
            {
                _isEncoded = !Regex.IsMatch(url, @".{2,5}:\/\/.{2,}(:\d+|)(\/|).*$", RegexOptions.IgnoreCase);
                if (_isEncoded) { ShortCode = url; } else { UrlFull = url; }
                LoadUrlInfo();
            }
            catch (Exception ex)
            {
                // todo: log
            }
        }

        public ShortUrl()
        {
            // create empty guy, normally for errors
        }


        private void LoadUrlInfo()
        {
            try
            {
                if (LoadFromDb())
                {
                    ShortUrlFull = RnHelpers.GetBaseUrl(String.Format("{0}/{1}", "Go", ShortCode));
                    return;
                }

                if (CreateNewUrl())
                {
                    ShortUrlFull = RnHelpers.GetBaseUrl(String.Format("{0}/{1}", "Go", ShortCode));
                    return;
                }

                // todo: log that there was an error
                Console.WriteLine("dddddd");
            }
            catch (Exception ex)
            {
                // todo: log this
            }
        }

        private bool LoadFromDb()
        {
            try
            {
                string sqlText;

                if (!_isEncoded)
                {
                    const string sqlFull = @"SELECT * FROM [tb_urls] WHERE urlFull = '{0}'";
                    sqlText = String.Format(sqlFull, UrlFull);
                }
                else
                {
                    const string sqlFull = @"SELECT * FROM [tb_urls] WHERE urlShortCode = '{0}'";
                    sqlText = String.Format(sqlFull, ShortCode);
                }

                using (var sqlCon = RnSql.GetConnection())
                {
                    using (var sqlCmd = new SqlCommand(sqlText, sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.Text;
                        using (var reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                UrlId = (int) reader["urlID"];
                                HitCount = (int) reader["urlHitCount"];
                                DateAdded = (DateTime) reader["addedDate"];
                                DateLastUsed = (DateTime) reader["urlLastHitDate"];
                                ShortCode = (string) reader["urlShortCode"];
                                UrlFull = (string) reader["urlFull"];
                                _loadedFromDb = true;
                            }
                        }
                    }
                    sqlCon.Close();
                }

                return UrlId > 0;
            }
            catch (Exception ex)
            {
                // todo: log this
            }

            return false;
        }

        private bool CreateNewUrl()
        {
            try
            {
                UrlId = GetNewUrlId();
                HitCount = 0;
                DateAdded = DateTime.UtcNow;
                DateLastUsed = DateTime.UtcNow;
                ShortCode = UrlId.ToString(CultureInfo.InvariantCulture).Base64Encode();

                const string sqlTpl = @"INSERT INTO [tb_urls] 
	                ([urlHitCount], [addedDate], [urlLastHitDate], [urlShortCode], [urlFull])
                VALUES
	                (0, GETUTCDATE(), GETUTCDATE(), @shortCode, @fullUrl)";

                using (var sqlCon = RnSql.GetConnection())
                {
                    using (var sqlCmd = new SqlCommand(sqlTpl, sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.Parameters.AddWithValue("shortCode", ShortCode);
                        sqlCmd.Parameters.AddWithValue("fullUrl", UrlFull);
                        sqlCmd.ExecuteNonQuery();
                    }
                    sqlCon.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                // todo: log this
            }

            return false;
        }

        private int GetNewUrlId()
        {
            try
            {
                const string cmdText = @"SELECT ISNULL(MAX(urlID), 0) + 1 as 'newId' from [tb_urls]";

                using (var sqlCon = RnSql.GetConnection())
                {
                    using (var sqlCmd = new SqlCommand(cmdText, sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.Text;
                        using (var reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                return (int) reader[0];
                            }
                        }
                    }

                    sqlCon.Close();
                }

            }
            catch (Exception ex)
            {
                // todo: log this
            }

            return 0;
        }

        public void UpdateHitCounter()
        {
            try
            {
                const string sqlText = @"UPDATE [tb_urls]
	                SET urlHitCount = urlHitCount + 1,
	                urlLastHitDate = GETUTCDATE()
                WHERE urlShortCode = @shortCode";

                using (var sqlCon = RnSql.GetConnection())
                {
                    using (var sqlCmd = new SqlCommand(sqlText, sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.Parameters.AddWithValue("shortCode", ShortCode);
                        sqlCmd.ExecuteNonQuery();
                    }
                    sqlCon.Close();
                }
            }
            catch (Exception ex)
            {
                // todo: log this
            }
        }

    }
}