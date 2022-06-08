using System;

namespace RnSync.DbModel
{
  class SyncJobLocation
  {
    public long LocationId { get; private set; }
    public string LocationProvider { get; private set; }
    public string LocationType { get; private set; }
    public string BaseDirectory { get; private set; }
    public DateTime DateAdded { get; private set; }
    public DateTime? LastUpdated { get; private set; }
    public DateTime? LastIntegrityCheck { get; private set; }

    public SyncJobLocation(long locationId)
    {
      var table = SyncDBFactory.LocationsTable();
      var location = table.Find(locationId: locationId);
      foreach (var oLocation in location)
      {
        MapDbRow(oLocation);
        break;
      }
    }

    public SyncJobLocation(dynamic oLocation)
    {
      MapDbRow(oLocation);
    }

    private void MapDbRow(dynamic dbRow)
    {
      LocationId = dbRow.locationId;
      LocationProvider = dbRow.locationProvider;
      LocationType = dbRow.locationType;
      BaseDirectory = dbRow.baseDirectory;
      DateAdded = dbRow.dateAdded;
      LastUpdated = dbRow.lastUpdated;
      LastIntegrityCheck = dbRow.lastIntegrityCheck;
    }

  }
}
