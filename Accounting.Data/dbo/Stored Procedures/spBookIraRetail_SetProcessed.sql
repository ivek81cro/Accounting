CREATE PROCEDURE [dbo].[spBookIraRetail_SetProcessed]
@RedniBroj int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE BookIraRetail set Knjizen = 1
	WHERE RedniBroj = @RedniBroj;
END
