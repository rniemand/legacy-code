using Massive.SQLite;

namespace RnSync.DbModel
{
  static class SyncDBFactory
  {

    public static dynamic JobsTable()
    {
      return new DynamicModel("RnSync", "sync_jobs", "jobId");
    }

    public static dynamic LocationsTable()
    {
      return new DynamicModel("RnSync", "sync_job_locations", "locationId");
    }

    public static dynamic JobDestinationsTable()
    {
      return new DynamicModel("RnSync", "sync_job_destinations", "destinationId");
    }

  }
}
