CREATE PROCEDURE [dbo].[spBookIra_SetProcessed]
@RedniBroj int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE BookIra set Knjizen = 1
	WHERE RedniBroj = @RedniBroj;
END