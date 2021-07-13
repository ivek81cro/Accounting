CREATE PROCEDURE [dbo].[spCounties_GetById]
@Id int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, Naziv
	FROM Counties
	WHERE Id=@Id;
END