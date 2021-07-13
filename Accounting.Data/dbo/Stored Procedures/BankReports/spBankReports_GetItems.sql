CREATE PROCEDURE [dbo].[spBankReports_GetItems]
@IdIzvod INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, IdIzvod, Naziv, Opis, Konto, Dugovna, Potrazna, Strana
	FROM BankReportItems
	WHERE IdIzvod=@IdIzvod;
END