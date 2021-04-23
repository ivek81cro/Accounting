CREATE PROCEDURE [dbo].[spBankReports_InsertHeader]
@Id int,
@RedniBroj INT, 
@DatumIzvoda DATETIME2, 
@SumaPotrazna DECIMAL(9, 2), 
@SumaDugovna DECIMAL(9, 2), 
@StanjePrethodnogIzvoda DECIMAL(9, 2), 
@NovoStanje DECIMAL(9, 2), 
@Knjizen BIT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO BankReport (RedniBroj, DatumIzvoda, SumaPotrazna, SumaDugovna, StanjePrethodnogIzvoda, NovoStanje, Knjizen)
	VALUES (@RedniBroj, @DatumIzvoda, @SumaPotrazna, @SumaDugovna, @StanjePrethodnogIzvoda, @NovoStanje, @Knjizen);
END