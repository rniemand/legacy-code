create table if not exists [tb_artists] (
  [artistType] VARCHAR(32), 
  [artistId] VARCHAR(64), 
  [artistName] VARCHAR(128), 
  [artistSortName] VARCHAR(128), 
  [artistCountry] VARCHAR(64));

create table if not exists [tb_releases] (
  [releaseDate] DATE, 
  [tbDateAdded] DATETIME DEFAULT (datetime()), 
  [tbDateUpdated] DATETIME, 
  [releaseId] VARCHAR(64), 
  [artistId] VARCHAR(64), 
  [releaseStatus] VARCHAR(64), 
  [releaseQuality] VARCHAR(64), 
  [releaseLang] VARCHAR(64), 
  [releaseScript] VARCHAR(64), 
  [releaseCountry] VARCHAR(64), 
  [releaseBarcode] VARCHAR(64), 
  [releaseTitle] VARCHAR(256));

  create table if not exists  [tb_recordings] (
  [recTrackNo] INT(4), 
  [recLength] INT(8), 
  [recId] VARCHAR(64), 
  [releaseId] VARCHAR(64), 
  [artistId] VARCHAR(64), 
  [recTitle] VARCHAR(128));