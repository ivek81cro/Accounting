CREATE PROCEDURE [dbo].[spPayrollArchive_IfExists]
@UniqueIdentifier nvarchar(255)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id
	FROM PayrollArchiveHeader
	WHERE UniqueIdentifier=@UniqueIdentifier;
END
