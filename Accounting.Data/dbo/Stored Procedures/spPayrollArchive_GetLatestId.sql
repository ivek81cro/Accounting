CREATE PROCEDURE [dbo].[spPayrollArchive_GetLatestId]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 Id FROM PayrollArchiveHeader ORDER BY Id DESC
END
