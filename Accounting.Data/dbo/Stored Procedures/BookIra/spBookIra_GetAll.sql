CREATE PROCEDURE [dbo].[spBookIra_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, RedniBroj, BrojRacuna, Storno, IzRacuna, Datum, Dospijece, DatumZadnjeUplate, 
		NazivISjedisteKupca, Oib, IznosSPdv, OslobodjenoPdvEU, OslobodjenoPdvOstalo, ProlaznaStavka, PoreznaOsnovica0, 
		PoreznaOsnovica5, Pdv5, PoreznaOsnovica10, Pdv10, PoreznaOsnovica13, Pdv13, PoreznaOsnovica23, Pdv23, PoreznaOsnovica25, 
		Pdv25, UkupniPdv, UkupnoUplaceno, PreostaloZaUplatit, NapomenaORacunu, ZaprimljenUHzzo, DanaOdZaprimanja, DanaNeplacanja, Knjizen,
		TemeljnicaId
	FROM BookIra
	ORDER BY RedniBroj ASC;
END