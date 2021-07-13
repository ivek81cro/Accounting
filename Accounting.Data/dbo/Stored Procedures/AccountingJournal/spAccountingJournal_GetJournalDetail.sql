CREATE PROCEDURE [dbo].[spAccountingJournal_GetJournalDetail]
@BrojTemeljnice int,
@VrstaTemeljnice nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Opis, Dokument, Broj, Konto, Datum, Valuta, Dugovna, Potrazna, VrstaTemeljnice, BrojTemeljnice,
	DatumKnjizenja
	FROM AccountingJournal
	WHERE BrojTemeljnice = @BrojTemeljnice AND VrstaTemeljnice = @VrstaTemeljnice
	ORDER BY Broj ASC;
END