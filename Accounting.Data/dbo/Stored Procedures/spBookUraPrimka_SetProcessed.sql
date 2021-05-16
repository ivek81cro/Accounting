CREATE PROCEDURE [dbo].[spBookUraPrimka_SetProcessed]
@BrojUKnjiziUra int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE BookUraPrimka set Knjizen = 1
	WHERE BrojUKnjiziUra = @BrojUKnjiziUra;

	UPDATE BookUraRest set Knjizen = 1
	WHERE RedniBroj = @BrojUKnjiziUra;
END