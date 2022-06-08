using System;
using System.Collections.Generic;
using System.IO;
using RnCore.Db.Sqlite;
using RnCore.Helpers;

namespace MfLib.Database
{
    public static class MusicDb
    {
        public static string BaseFilePath { get; private set; }
        public static string XmlSpPath { get; private set; }
        public static DbSqlite DbObject { get; private set; }

        private static Dictionary<string, XmlSp> _xmlSps;

        static MusicDb()
        {
            BaseFilePath = "./Res\\DB\\".MakeRelative();
            XmlSpPath = "./Res\\XmlSp\\".MakeRelative();
            _xmlSps = new Dictionary<string, XmlSp>();
        }

        public static void RegisterDb(string dbFilePath)
        {
            try
            {
                DbObject = SqliteFactory.RegisterDb(@"c:\bob.db");
                RunDbIntegrityCheck();
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }

        public static void RunDbIntegrityCheck()
        {
            if (DbObject == null)
                return;

            // Ensure that the integrity file exists
            var integFile = String.Format("{0}CreateDb.sql", BaseFilePath);
            if (!File.Exists(integFile))
            {
                RnLocale.LogEvent("mf", "mf.0003", integFile);
                Environment.Exit(-1);
            }

            try
            {
                // todo - LATER - add chack in for specific tables / version so we dont run this each time...
                // Run the CreateDb.sql file against the DB
                
                var commands = RnIO.ReadAllText(integFile);
                // todo - LATER - check the length of commands to ensure we have something to work with

                DbObject.ExecuteNonQuery(commands);
                RnLocale.LogEvent("mf", "mf.0004");
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }


        public static bool XmlSpLoaded(string spFileName, string version = "1.0.0")
        {
            if (_xmlSps.Count == 0)
                return false;

            try
            {
                return _xmlSps.ContainsKey(String.Format("{0}.{1}", spFileName, version));
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return false;
            }
        }

        public static void LoadXmlSp(string spFileName, string version = "1.0.0" )
        {
            if (XmlSpLoaded(spFileName, version))
                return;

            _xmlSps.Add(
                String.Format("{0}.{1}", spFileName, version),
                new XmlSp(XmlSpPath, spFileName, version)
                );
        }

        public static XmlSp GetXmlSp(string spFileName, string version = "1.0.0")
        {
            if (!XmlSpLoaded(spFileName, version))
                return null;

            try
            {
                return _xmlSps[String.Format("{0}.{1}", spFileName, version)];
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return null;
            }
        }

        public static string GetXmlSpCmd(string spFileName, string spName, string version = "1.0.0", params object[] parameters)
        {
            try
            {
                LoadXmlSp(spFileName, version);
                var xmlsp = GetXmlSp(spFileName, version);
                return xmlsp.GetSp(spName, parameters);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return "";
            }
        }




    }
}
