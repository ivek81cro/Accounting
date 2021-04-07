CREATE PROCEDURE [dbo].[spEmployee_Delete]
@id int
AS
BEGIN
	
	SET NOCOUNT ON;
	DECLARE @Oib nvarchar(11);
	SET @Oib=(SELECT Oib FROM Employee WHERE Id=@id);

	DELETE FROM JoppdEmployee WHERE Oib=@Oib

	DELETE FROM Payroll WHERE Oib=@Oib

	DELETE FROM PayrollSupplementEmployee WHERE Oib=@Oib;

	DELETE FROM Employee WHERE Id=@id;

END
