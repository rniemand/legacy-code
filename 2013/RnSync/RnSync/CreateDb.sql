-- ================================================================================================
-- Reset DB
-- ================================================================================================
DROP TABLE IF EXISTS [sync_job_destinations];
DROP TABLE IF EXISTS [sync_jobs];
DROP TABLE IF EXISTS [sync_job_locations];
DROP TABLE IF EXISTS [sync_files];


-- ================================================================================================
-- Generate DB Tables
-- ================================================================================================
CREATE TABLE [sync_job_locations] (
       [locationId] INTEGER PRIMARY KEY AUTOINCREMENT, 
       [locationProvider] VARCHAR(32) NOT NULL DEFAULT FileSystem, 
       [locationType] VARCHAR(32) NOT NULL DEFAULT Source, 
       [baseDirectory] VARCHAR(256) NOT NULL, 
       [dateAdded] DATETIME NOT NULL DEFAULT (datetime('now','localtime')), 
       [lastUpdated] DATETIME, 
       [lastIntegrityCheck] DATETIME
);

CREATE TABLE [sync_jobs] (
       [jobId] INTEGER NOT NULL PRIMARY KEY ON CONFLICT FAIL AUTOINCREMENT, 
       [jobEnabled] BOOLEAN NOT NULL ON CONFLICT FAIL DEFAULT 1, 
       [jobName] VARCHAR(64) NOT NULL ON CONFLICT FAIL, 
       [jobDescription] VARCHAR(1024),  
       [sourceLocationId] INTEGER,
       [dateAdded] DATETIME NOT NULL DEFAULT (datetime('now','localtime')),       
       [sourceLastUpdated] DATETIME NULL,
       FOREIGN KEY(sourceLocationId) REFERENCES sync_job_locations(locationId)
);

CREATE TABLE [sync_job_destinations] (
       [destinationId] INTEGER PRIMARY KEY AUTOINCREMENT,
       [destinationEnabled] BOOLEAN NOT NULL ON CONFLICT FAIL DEFAULT 1, 
       [jobId] INTEGER,
       [locationId] INTEGER,       
       [dateAdded] DATETIME NOT NULL DEFAULT (datetime('now','localtime')),       
       [dateLastUpdated] DATETIME NULL,       
       [dateLastIntegrityCheck] DATETIME NULL,
       FOREIGN KEY (jobId) REFERENCES sync_jobs(jobId),       
       FOREIGN KEY (locationId) REFERENCES sync_job_locations(locationId)
);

CREATE TABLE [sync_files] (
       [fileId] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,       
       [jobId] INTEGER NOT NULL ON CONFLICT FAIL,
       [sourceLocationId] INTEGER NOT NULL ON CONFLICT FAIL,
       [destinationLocationId] INTEGER NOT NULL ON CONFLICT FAIL,       
       [srcFileExists] INT(1) NOT NULL DEFAULT 1,       
       [srcFileSize] BIGINT NOT NULL DEFAULT 0,
       [srcFileLastModded] DATETIME NOT NULL DEFAULT (datetime('0', 'unixepoch')),       
       [srcFileLastChecked] DATETIME NOT NULL DEFAULT (datetime('0', 'unixepoch')),       
       [srcFilePath] VARCHAR(256) NOT NULL,       
       [dstFileExists] INT(1) NOT NULL DEFAULT 1,       
       [dstFileSize] BIGINT NOT NULL DEFAULT 0,
       [dstFileLastModded] DATETIME NOT NULL DEFAULT (datetime('0', 'unixepoch')),       
       [dstFileLastChecked] DATETIME NOT NULL DEFAULT (datetime('0', 'unixepoch')),       
       [dstFilePath] VARCHAR(256) NOT NULL,
       FOREIGN KEY (sourceLocationId) REFERENCES sync_job_locations(locationId)       
       FOREIGN KEY (destinationLocationId) REFERENCES sync_job_locations(locationId)       
       FOREIGN KEY (jobId) REFERENCES sync_jobs(jobId)
);

-- ================================================================================================
-- Populate DB tables
-- ================================================================================================
INSERT INTO [sync_jobs]
       (jobName, jobDescription)
VALUES
      ('WIS', 'Testing...');      


INSERT INTO [sync_job_locations]
       (baseDirectory, locationType, locationProvider)
VALUES
      ('Z:\Windows-Installs\', 'Source', 'FileSystem'),
      ('E:\Windows-Installs\', 'Destination', 'FileSystem'),
      ('N:\Windows-Installs\', 'Destination', 'FileSystem');      

UPDATE [sync_jobs] SET sourceLocationId = 1 WHERE jobID = 1;

INSERT INTO [sync_job_destinations]
       (jobId, locationId)       
VALUES
      (1, 2),      
      (1, 3);


SELECT * FROM sync_job_destinations WHERE jobId = 1 AND destinationEnabled = 1