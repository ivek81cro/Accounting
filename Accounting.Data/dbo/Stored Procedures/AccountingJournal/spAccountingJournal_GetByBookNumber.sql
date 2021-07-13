CREATE PROCEDURE [dbo].[spAccountingJournal_GetByBookNumber]
@BrojTemeljnice int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Opis, Dokument, Broj, Konto, Datum, Valuta, Dugovna, Potrazna, VrstaTemeljnice, BrojTemeljnice,
	DatumKnjizenja
	FROM AccountingJournal
	WHERE BrojTemeljnice = @BrojTemeljnice
	ORDER BY Broj ASC;
END