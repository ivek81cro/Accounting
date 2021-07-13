CREATE PROCEDURE [dbo].[spBankReports_GetAllHeaders]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, RedniBroj, DatumIzvoda, SumaPotrazna, SumaDugovna, StanjePrethodnogIzvoda, NovoStanje, Knjizen, TemeljnicaId
	FROM BankReport
	ORDER BY RedniBroj DESC;
END