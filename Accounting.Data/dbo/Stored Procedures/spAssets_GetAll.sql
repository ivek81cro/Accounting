CREATE PROCEDURE [dbo].[spAssets_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Naziv, DatumNabave, Kolicina, Lokacija, InvBroj, Dobavljac, Dokument, DatumUporabe, NabavnaVrijednost, Skupina, VijekTrajanja, 
		StopaOtpisa, SintetickiKonto, KontoOtpisa, IznosOtpisa, SadasnjaVrijednost, DatumRashodovanja, VrstaPoTrajanju
	FROM Assets;
END