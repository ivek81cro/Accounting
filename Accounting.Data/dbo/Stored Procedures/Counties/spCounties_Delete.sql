CREATE PROCEDURE [dbo].[spCounties_Delete]
@Id int
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM Counties WHERE Id=@Id;
END

