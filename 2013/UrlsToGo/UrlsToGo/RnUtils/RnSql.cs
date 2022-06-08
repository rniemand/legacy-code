using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace UrlsToGo.RnUtils
{
    public static class RnSql
    {

        public static SqlConnection GetConnection()
        {
            try
            {
                var con = new SqlConnection(WebConfigurationManager.ConnectionStrings["DbCon"].ConnectionString);
                con.Open();
                return con;
            }
            catch (Exception ex)
            {
                // todo: log this
            }

            return null;
        }

    }
}