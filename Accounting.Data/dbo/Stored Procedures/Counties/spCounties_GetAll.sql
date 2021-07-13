CREATE PROCEDURE [dbo].[spCounties_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, Naziv
	FROM Counties;
END
