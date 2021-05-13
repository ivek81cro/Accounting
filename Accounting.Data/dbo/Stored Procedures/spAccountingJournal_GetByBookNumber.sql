CREATE PROCEDURE [dbo].[spAccountingJournal_GetByBookNumber]
@BrojTemeljnice int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Opis, Dokument, Broj, Konto, Datum, Valuta, Dugovna, Potrazna, VrstaTemeljnice, BrojTemeljnice
	FROM AccountingJournal
	WHERE BrojTemeljnice = @BrojTemeljnice;
END