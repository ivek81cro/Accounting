CREATE PROCEDURE [dbo].[spPayroll_Delete]
@Oib nvarchar(11)
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM Payroll WHERE Oib=@Oib;
END