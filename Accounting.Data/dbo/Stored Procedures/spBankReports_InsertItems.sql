CREATE PROCEDURE [dbo].[spBankReports_InsertItems]
@Id INT, 
@IdIzvod INT, 
@Naziv NVARCHAR(255),
@Opis NVARCHAR(255),
@Konto NVARCHAR(25),
@Dugovna DECIMAL(9, 2),
@Potrazna DECIMAL(9, 2),
@Strana NCHAR(1)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO BankReportItems (IdIzvod, Naziv, Opis, Konto, Dugovna, Potrazna, Strana)
	VALUES (@IdIzvod, @Naziv, @Opis, @Konto, @Dugovna, @Potrazna, @Strana);
END