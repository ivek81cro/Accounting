CREATE PROCEDURE [dbo].[spAccountingJournal_GetLatestNumber]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT BrojTemeljnice
	FROM AccountingJournal
	ORDER BY BrojTemeljnice DESC;
END
