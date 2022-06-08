using System.Data;
using System.Data.SqlClient;

namespace AlertProcessor.Classes
{
    public static class DbHelper
    {
        private static SqlConnection _con;

        public static SqlConnection GetConnection()
        {
            if(_con==null)
            {
                const string conStr = "Server=scomdw;Database=OperationsManagerDW;Trusted_Connection=True;";
                _con = new SqlConnection(conStr);
            }

            if (_con.State != ConnectionState.Open)
                _con.Open();

            return _con;
        }
    }
}
