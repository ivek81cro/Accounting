CREATE PROCEDURE [dbo].[spPayrollArchive_Delete]
@Id int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM PayrollArchiveSupplement WHERE AccountingId=@Id;

	DELETE FROM PayrollArchivePayroll WHERE AccountingId=@Id;

	DELETE FROM PayrollArchiveHeader WHERE Id=@Id;
END
