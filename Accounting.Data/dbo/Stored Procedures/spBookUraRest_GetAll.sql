CREATE PROCEDURE [dbo].[spBookUraRest_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, RedniBroj, Datum, BrojRacuna, Storno, StornoBroja, DatumRacuna, StarostRacuna, Dospijece, 
		PlaniranaUplata, DatumUplate, ZaUplatu, NazivDobavljaca, BrojPrimke, NapomenaORacunu, NettoNabavnaVrijednost, 
		SjedisteDobavljaca, OIB, IznosSPorezom, PoreznaOsnovica0, PoreznaOsnovica5, PretporezT5, PoreznaOsnovica10, PretporezT10, 
		PoreznaOsnovica13, PretporezT13, PoreznaOsnovica23, PretporezT23, PoreznaOsnovica25, PretporezT25, UkupniPretporez, 
		MozeSeOdbiti, NeMozeSeOdbiti, IznosBezPoreza, ProlaznaStavka, Neoporezivo, CassaScontoPercent, CassaSconto, BrojOdobrenja, 
		OdobrenjaBezPDV, OdobreniPDV, DatumPodnosenja, DatumIzvrsenja, UkupnoUplaceno, PreostaloZaUplatit, DospijeceDana, Knjizen
	FROM BookUraRest;
END