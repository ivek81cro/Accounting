CREATE PROCEDURE [dbo].[spCounties_Update]
@Id int,
@Naziv nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Counties SET Naziv=@Naziv
	WHERE Id=@Id;
END