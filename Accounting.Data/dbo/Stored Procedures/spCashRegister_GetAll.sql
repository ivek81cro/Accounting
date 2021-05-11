CREATE PROCEDURE [dbo].[spCashRegister_GetAll]

AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, RedniBroj, Datum, Gotovina, KreditneKartice, Sveukupno, IznosSudjelovanja, Knjizen
	FROM CashRegisterJournal
END