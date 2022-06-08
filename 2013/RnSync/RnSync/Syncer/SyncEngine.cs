using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RnSync.DbModel;
using RnUtils.Logging;

namespace RnSync.Syncer
{
  class SyncEngine
  {
    public SyncEngineState State { get; private set; }
    public SyncJob Job { get; private set; }

    public SyncEngine(SyncJob syncJob)
    {
      State = SyncEngineState.Uninitilized;
      Job = syncJob;
      ValidateJob();
    }

    private void ValidateJob()
    {
      if (Job.SourceLocation == null)
      {
        State = SyncEngineState.NoSourceDefined;
        return;
      }

      if (Job.Destinations == null || Job.Destinations.DestinationCount == 0)
      {
        State = SyncEngineState.NoDestinationsDefined;
        return;
      }

      State = SyncEngineState.Ready;
    }


    public void RunSync()
    {
      if (State != SyncEngineState.Ready)
      {
        RnLogger.Error("Cannot sync, SyncEngine not ready");
        return;
      }
      
      
    }

  }
}
