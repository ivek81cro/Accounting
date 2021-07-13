CREATE PROCEDURE [dbo].[spAccountingJournal_Update]
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
@BrojTemeljnice INT,
@DatumKnjizenja DATETIME2
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE AccountingJournal 
	SET BrojTemeljnice = @BrojTemeljnice, DatumKnjizenja=@DatumKnjizenja
	WHERE Id=@Id;
END
