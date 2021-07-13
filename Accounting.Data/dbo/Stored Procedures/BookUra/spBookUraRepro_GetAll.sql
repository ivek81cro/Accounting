CREATE PROCEDURE [dbo].[spBookUraRepro_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, DatumKnjizenja, BrojPrimke, Storno,  NazivDobavljaca, BrojRacuna,
	DatumRacuna, Otpremnica, DospijecePlacanja, FakturnaVrijednost, NabavnaVrijednost, VeleprodajniRabat,
	Rabat, NettoNabavnaVrijednost, Pretporez, CassaSconto, PorezniBroj, BrojUKnjiziUra, Knjizen, TemeljnicaId
	FROM BookUraRepro;
END
