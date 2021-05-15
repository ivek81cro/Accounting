CREATE PROCEDURE [dbo].[spPayrollArchive_SetProcessed]
@Id int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE PayrollArchiveHeader SET Knjizen=1
	WHERE Id=@Id;
END