CREATE PROCEDURE [dbo].[spBackupDatabase_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
	CONVERT(CHAR(100), SERVERPROPERTY('Servername')) AS ServerName, 
	msdb.dbo.backupset.database_name AS DbName, 
	msdb.dbo.backupset.backup_start_date AS StartDate, 
	msdb.dbo.backupset.backup_finish_date AS FinishDate, 
	CASE msdb..backupset.type 
		WHEN 'D' THEN 'Database' 
		WHEN 'L' THEN 'Log' 
	END 
	AS BackupType,
	msdb.dbo.backupset.backup_size AS BackupSize,
	msdb.dbo.backupmediafamily.physical_device_name AS SavePath, 
	msdb.dbo.backupset.name AS BackupSetName
	FROM msdb.dbo.backupmediafamily 
	INNER JOIN msdb.dbo.backupset ON msdb.dbo.backupmediafamily.media_set_id = msdb.dbo.backupset.media_set_id 
	WHERE (CONVERT(datetime, msdb.dbo.backupset.backup_start_date, 102) >= GETDATE() - 7) 
	ORDER BY 
	msdb.dbo.backupset.database_name, 
	msdb.dbo.backupset.backup_finish_date 
END