CREATE PROCEDURE [dbo].[spEmployee_Delete]
@id int
AS
BEGIN
	
	SET NOCOUNT ON;

	DELETE FROM Payroll
	WHERE Oib=(SELECT Oib FROM Employee WHERE Id=@id);

	DELETE FROM PayrollSupplementEmployee
	WHERE Oib=(SELECT Oib FROM Employee WHERE Id=@id);

	DELETE FROM Employee
	WHERE Id=@id;
END
