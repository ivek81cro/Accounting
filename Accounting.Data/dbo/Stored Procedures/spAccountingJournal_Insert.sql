CREATE PROCEDURE [dbo].[spAccountingJournal_Insert]
@Id INT,
@Opis NVARCHAR(255),
@Dokument NVARCHAR(255),
@Broj INT,
@Konto NVARCHAR(25),
@Datum DATETIME2,
@Valuta NVARCHAR(5),
@Dugovna DECIMAL(9,2),
@Potrazna DECIMAL(9,2),
@VrstaTemeljnice NVARCHAR(125),
@BrojTemeljnice INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO AccountingJournal (Opis, Dokument, Broj, Konto, Datum, Valuta, Dugovna, Potrazna, VrstaTemeljnice, BrojTemeljnice)
	VALUES (@Opis, @Dokument, @Broj, @Konto, @Datum, @Valuta, @Dugovna, @Potrazna, @VrstaTemeljnice, @BrojTemeljnice);
END