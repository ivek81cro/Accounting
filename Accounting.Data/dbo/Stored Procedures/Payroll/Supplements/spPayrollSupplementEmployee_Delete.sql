CREATE PROCEDURE [dbo].[spPayrollSupplementEmployee_Delete]
@Id int
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM PayrollSupplementEmployee WHERE Id=@Id;
END