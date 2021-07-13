CREATE PROCEDURE [dbo].[spCounties_Insert]
@Naziv nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Counties (Naziv) VALUES(@Naziv);
END
