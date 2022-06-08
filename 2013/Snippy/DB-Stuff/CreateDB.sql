USE master
GO


-- ===============================================================
-- Create the DB
-- ===============================================================
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Snippy')
	BEGIN
		ALTER DATABASE [Snippy]
		SET SINGLE_USER WITH ROLLBACK IMMEDIATE

		DROP DATABASE [Snippy]
	END
GO

CREATE DATABASE [Snippy]
GO

USE [Snippy]
GO


-- ===============================================================
-- Create Tables
-- ===============================================================
CREATE TABLE [tb_snippy_users] (
	userId int NOT NULL IDENTITY (1, 1),
	dateCreated datetime NOT NULL DEFAULT GETUTCDATE(),
	lastSeen datetime NULL,
	userName varchar(32) NOT NULL,
	PRIMARY KEY(userId)
)
GO


-- ===============================================================
-- Create Users
-- ===============================================================
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE [name] = 'SnippyUser')
	BEGIN
		CREATE LOGIN SnippyUser WITH PASSWORD = 'Password1234$';
	END
GO

CREATE USER SnippyUser FOR LOGIN SnippyUser
GO

exec sp_addrolemember db_datareader, 'SnippyUser' 
go
exec sp_addrolemember db_datawriter, 'SnippyUser' 
go