CREATE PROCEDURE [dbo].[spAccountingJournal_LoadLedger]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, Opis, Dokument, Broj, Konto, Datum, Valuta, Dugovna, Potrazna, VrstaTemeljnice, BrojTemeljnice,
		DatumKnjizenja
	FROM AccountingJournal
	ORDER BY Datum;
END
