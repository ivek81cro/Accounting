CREATE PROCEDURE [dbo].[spPayrollArchive_IfExists]
@UniqueId nvarchar(255)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id
	FROM PayrollArchiveHeader
	WHERE UniqueId=@UniqueId;
END
