CREATE PROCEDURE [dbo].[spBookIraHzzo_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, DatumPlacanja, Dokument, OriginalniBroj, DatumDokumenta, Program, Opis, IznosRacuna, PlaceniIznos, Povezan 
	FROM BookIraHzzo;
END
