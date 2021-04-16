CREATE PROCEDURE [dbo].[spBookUraPrimka_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, DatumKnjizenja, BrojPrimke, Storno, MaloprodajnaVrijednost, NazivDobavljaca, BrojRacuna,
	DatumRacuna, Otpremnica, DospijecePlacanja, FakturnaVrijednost, MaloprodajnaMarza, IznosPdv, VrijednostBezPoreza, NabavnaVrijednost,
	MaloprodajniRabat, NettoNabavnaVrijednost, Pretporez, VeleprodajniRabat, CassaSconto, NettoRuc, PovratnaNaknada, PorezniBroj, BrojUKnjiziUra
	FROM BookUraPrimka
	ORDER BY BrojUKnjiziUra DESC;

END