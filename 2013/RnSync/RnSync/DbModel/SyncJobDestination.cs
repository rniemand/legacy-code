using System;

namespace RnSync.DbModel
{
  class SyncJobDestination
  {
    // Basic properties
    public long ID { get; private set; }
    public bool Enabled { get; private set; }
    public long JobID { get; private set; }
    public long LocationID { get; private set; }
    public DateTime DateAdded { get; private set; }
    public DateTime? LastUpdated { get; private set; }
    public DateTime? LastIntegrityCheck { get; private set; }

    // Job Info
    private SyncJob _job;
    public SyncJob Job
    {
      get
      {
        if (JobID == 0) return null;
        return _job ?? (_job = new SyncJob(JobID));
      }
    }

    // Location
    private SyncJobLocation _jobLocation;
    public SyncJobLocation Location
    {
      get
      {
        if (LocationID == 0) return null;
        return _jobLocation ?? (_jobLocation = new SyncJobLocation(LocationID));
      }
    }

    public SyncJobDestination(dynamic oDestination)
    {
      ID = oDestination.destinationId;
      Enabled = oDestination.destinationEnabled;
      JobID = oDestination.jobId;
      LocationID = oDestination.locationId;
      DateAdded = oDestination.dateAdded;
      LastUpdated = oDestination.dateLastUpdated;
      LastIntegrityCheck = oDestination.dateLastIntegrityCheck;
    }

  }
}
