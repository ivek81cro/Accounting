CREATE PROCEDURE [dbo].[spBookUraRepro_SetProcessed]
@BrojUKnjiziUra int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE BookUraRepro set Knjizen = 1
	WHERE BrojUKnjiziUra = @BrojUKnjiziUra;

	UPDATE BookUraRest set Knjizen = 1
	WHERE RedniBroj = @BrojUKnjiziUra;
END