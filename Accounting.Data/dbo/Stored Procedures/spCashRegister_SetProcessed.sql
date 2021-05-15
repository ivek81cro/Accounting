CREATE PROCEDURE [dbo].[spCashRegister_SetProcessed]
@RedniBroj int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE CashRegisterJournal set Knjizen = 1
	WHERE RedniBroj = @RedniBroj;
END
