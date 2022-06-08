using System;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using RnSync.DbModel;
using RnSync.Old;
using RnSync.Syncer;

namespace RnSync
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      CreateAppDb();

      var tbl = SyncDBFactory.JobsTable();
      var rows = tbl.All();

      foreach (var oJob in rows)
      {
        var job = new SyncJob(oJob);
        var engine = new SyncEngine(job);
        engine.RunSync();
      }



      Console.WriteLine();
      Console.WriteLine("All done...");
      Console.ReadLine();
    }


    private static FtpSyncClientConfig ConfigSingleDir()
    {
      return new FtpSyncClientConfig
        {
          FtpHost = "richard-niemand.name",
          FtpUserName = "backup@richard-niemand.name",
          FtpPassword = "e4h67gh$$",
          LocalDir = @"Z:\richard-niemand.name\wp-admin\css\",
          RemoteDir = "/wp-admin/css/",
          AcceptAllCerts = true,
          UseClearText = true,
          FetchRemoteFileInfoOnInitialScan = false,
          ConfigName = "richard-niemand.name (dev)",
          DbFilePath = @"z:\richard-niemand.name.dev.db"
        };
    }

    private static FtpSyncClientConfig ConfigFull()
    {
      return new FtpSyncClientConfig
        {
          FtpHost = "richard-niemand.name",
          FtpUserName = "backup@richard-niemand.name",
          FtpPassword = "e4h67gh$$",
          LocalDir = @"Z:\richard-niemand.name\",
          RemoteDir = "/",
          AcceptAllCerts = true,
          UseClearText = true,
          FetchRemoteFileInfoOnInitialScan = false,
          ConfigName = "richard-niemand.name",
          DbFilePath = @"z:\richard-niemand.name.db"
        };
    }

    private static void OriginalVibe()
    {
      var config = ConfigFull();
      var client = new FtpSyncClient(config);

      client.AddIgnoredDir("/richard");
      client.AddIgnoredDir("/cgi-bin");

      //client.UpdateRemoteFilesInfo();
      //client.DownloadFiles();
      //client.BatchVerifyLocalFiles();
      client.RunLocalFileIntegrityCheck2();
    }

    private static string WorkSqliteFilePath(string connectionString)
    {
      if (String.IsNullOrEmpty(connectionString)) return "";
      var conArguments = connectionString.Split(';');

      foreach (var parts in from part in conArguments where part.ToUpper().Contains("DATA SOURCE") select part.Split('='))
      {
        return parts[1].Trim();
      }

      return "";
    }

    private static void CheckSqliteDB(string dbFilePath, string connectionString)
    {
      if (String.IsNullOrEmpty(dbFilePath)) return;
      if (File.Exists(dbFilePath)) return;

      SQLiteConnection.CreateFile(dbFilePath);
      const string generateDbSqlFile = @"./CreateDb.sql";
      if (File.Exists(generateDbSqlFile))
      {
        var generateSql = File.ReadAllText(generateDbSqlFile);
        var con = new SQLiteConnection(connectionString);
        con.Open();
        var cmd = new SQLiteCommand(con) {CommandText = generateSql};
        cmd.ExecuteNonQuery();
        cmd.Dispose();
        con.Close();
        con.Dispose();
      }
    }

    private static void CreateAppDb()
    {
      var con = ConfigurationManager.ConnectionStrings["RnSync"];
      var dbFilePath = WorkSqliteFilePath(con.ConnectionString);
      if (File.Exists(dbFilePath)) File.Delete(dbFilePath);
      CheckSqliteDB(dbFilePath, con.ConnectionString);
    }

  }
}
