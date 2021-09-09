CREATE PROCEDURE [dbo].[spBackupDatabase_Full]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @fileDate NVARCHAR(40);
	DECLARE @path NVARCHAR(512);
	DECLARE @name NVARCHAR(256);
	DECLARE @fileName NVARCHAR(512);
	
	SELECT @fileDate = CONVERT(NVARCHAR(20),GETDATE(),112);
	
	SET @path = '/var/opt/mssql/data/';
	SET @name = 'Accounting';
	
	SET @fileName = @path + @name + '_' + @fileDate + '.BAK';
	BACKUP DATABASE @name TO DISK = @fileName;
END