CREATE PROCEDURE [dbo].[spBankReports_GetHeader]
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, RedniBroj, DatumIzvoda, SumaPotrazna, SumaDugovna, StanjePrethodnogIzvoda, NovoStanje, Knjizen, TemeljnicaId
	FROM BankReport
	WHERE Id=@Id;
END