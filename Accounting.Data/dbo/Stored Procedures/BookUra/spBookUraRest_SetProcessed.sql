CREATE PROCEDURE [dbo].[spBookUraRest_SetProcessed]
@BrojUKnjiziUra int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE BookUraRest set Knjizen = 1
	WHERE RedniBroj = @BrojUKnjiziUra;
END