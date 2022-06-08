using System.Collections.Generic;

namespace RnSync.DbModel
{
  class SyncJobDestinations
  {
    public List<SyncJobDestination> SyncDestination { get; private set; }
    public int DestinationCount { get; private set; }
    public long JobID { get; private set; }


    public SyncJobDestinations(long jobId)
    {
      SyncDestination = new List<SyncJobDestination>();
      DestinationCount = 0;
      JobID = jobId;
      
      RefreshDestinations();
    }

    public void RefreshDestinations()
    {
      SyncDestination.Clear();
      DestinationCount = 0;

      var table = SyncDBFactory.JobDestinationsTable();
      var jobs = table.Find(jobId: JobID, destinationEnabled: 1);
      foreach (var oDestination in jobs)
      {
        SyncDestination.Add(new SyncJobDestination(oDestination));
      }

      DestinationCount = SyncDestination.Count;
    }


  }
}
