CREATE TABLE [ftp_fileInfo] (
  [configId] BIGINT DEFAULT 0,
  [RemoteFileExists] BOOLEAN DEFAULT 0,
  [RemoteFilePath] VARCHAR(256), 
  [RemoteModifiedTime] DATETIME,
  [RemoteCheckedTime] DATETIME,
  [RemoteSize] BIGINT DEFAULT 0, 
  [LocalFileExists] BOOL DEFAULT 0, 
  [LocalFileName] VARCHAR(256), 
  [LocalModifiedTime] DATETIME, 
  [LocalSize] BIGINT DEFAULT 0, 
  [LocalCheckedTime] DATETIME,
  [LoaclIntegrityCheckedTime] DATETIME
);

CREATE TABLE [ftp_config] (
  [configName] VARCHAR(256)
);

