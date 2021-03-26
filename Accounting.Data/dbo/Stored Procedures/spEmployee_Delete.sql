CREATE PROCEDURE [dbo].[spEmployee_Delete]
@id int
AS
BEGIN
	
	SET NOCOUNT ON;

	DELETE FROM Employee
	WHERE Id=@id;

END
