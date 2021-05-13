CREATE PROCEDURE [dbo].[spBankReports_UpdateHeader]
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
	UPDATE BankReport SET Knjizen=@Knjizen
	WHERE RedniBroj=@RedniBroj;
END