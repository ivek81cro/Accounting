﻿CREATE PROCEDURE [dbo].[spBackupDatabase_Full]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @fileDate NVARCHAR(40);
	DECLARE @savePath NVARCHAR(512);
	DECLARE @name NVARCHAR(256);
	DECLARE @fileName NVARCHAR(512);
	
	SELECT @fileDate = CONVERT(NVARCHAR(20),GETDATE(),112);
	
	SET @savePath = '/var/opt/mssql/data/';
	SET @name = (SELECT DB_NAME());
	
	SET @fileName = @savePath + @name + '_' + @fileDate + '.BAK';
	BACKUP DATABASE @name TO DISK = @fileName;
END