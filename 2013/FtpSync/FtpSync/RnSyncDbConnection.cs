using System;
using System.Configuration;
using RnUtils.Configuration;
using RnUtils.DB.RnSQLite;

namespace FtpSync
{
    internal class RnSyncDbConnection
    {
        public string DbFilePath { get; private set; }
        public bool DbConnected { get; private set; }

        private readonly RnSQLiteDb _dbObj;

        
        public RnSyncDbConnection(string dbFilePath)
        {
            DbFilePath = dbFilePath;
            _dbObj = new RnSQLiteDb(dbFilePath, true, true);
            DbConnected = _dbObj.Connected;
        }


    }
}
