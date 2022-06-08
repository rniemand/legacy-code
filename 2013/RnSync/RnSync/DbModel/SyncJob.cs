using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnSync.DbModel
{

  class SyncJob
  {
    // Basic job information
    public long JobID { get; private set; }
    public bool JobEnabled { get; private set; }
    public string JobName { get; private set; }
    public string JobDescription { get; private set; }
    public long SourceLocationID { get; private set; } // todo: get source location
    public DateTime DateAdded { get; private set; }
    public DateTime? SourceLastUpdated { get; private set; }

    // Job Source Location
    private SyncJobLocation _sourceLocation;
    public SyncJobLocation SourceLocation
    {
      get { return _sourceLocation ?? (_sourceLocation = new SyncJobLocation(SourceLocationID)); }
    }

    // Job Destination
    private SyncJobDestinations _destinations;
    public SyncJobDestinations Destinations
    {
      get
      {
        if (_destinations != null) _destinations.RefreshDestinations();
        return _destinations ?? (_destinations = new SyncJobDestinations(JobID));
      }
    }


    public SyncJob(dynamic oJob)
    {
      JobID = oJob.jobId;
      JobEnabled = oJob.jobEnabled;
      JobName = oJob.jobName;
      JobDescription = oJob.jobDescription;
      SourceLocationID = oJob.sourceLocationId;
      DateAdded = oJob.dateAdded;
      SourceLastUpdated = oJob.sourceLastUpdated;
    }



  }
}
