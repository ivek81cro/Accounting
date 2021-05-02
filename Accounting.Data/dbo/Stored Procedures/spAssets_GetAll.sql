CREATE PROCEDURE [dbo].[spAssets_GetAll]
@VrstaPoTrajanju nvarchar(255)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Naziv, DatumNabave, Kolicina, Lokacija, InvBroj, Dobavljac, Dokument, DatumUporabe, NabavnaVrijednost, Skupina, VijekTrajanja, 
		StopaOtpisa, SintetickiKonto, KontoOtpisa, IznosOtpisa, SadasnjaVrijednost, DatumRashodovanja, VrstaPoTrajanju
	FROM Assets
	WHERE VrstaPoTrajanju=@VrstaPoTrajanju;
END