CREATE PROCEDURE [dbo].[spAssets_Insert]
@Id int,
@Naziv nvarchar(255),
@DatumNabave datetime2,
@Kolicina decimal(9,2),
@Lokacija nvarchar(255),
@InvBroj int,
@Dobavljac nvarchar(255),
@Dokument nvarchar(255),
@DatumUporabe datetime2,
@NabavnaVrijednost decimal(9,2),
@Skupina nvarchar(255),
@VijekTrajanja decimal(9,2),
@StopaOtpisa decimal(9,2),
@SintetickiKonto nvarchar(255),
@KontoOtpisa nvarchar(255),
@IznosOtpisa decimal(9,2),
@SadasnjaVrijednost decimal(9,2),
@DatumRashodovanja datetime2,
@VrstaPoTrajanju nvarchar(255)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Assets (Naziv, DatumNabave, Kolicina, Lokacija, InvBroj, Dobavljac, Dokument, DatumUporabe, NabavnaVrijednost, Skupina, VijekTrajanja, StopaOtpisa, 
		SintetickiKonto, KontoOtpisa, IznosOtpisa, SadasnjaVrijednost, DatumRashodovanja, VrstaPoTrajanju)
	VALUES (@Naziv, @DatumNabave, @Kolicina, @Lokacija, @InvBroj, @Dobavljac, @Dokument, @DatumUporabe, @NabavnaVrijednost, @Skupina, @VijekTrajanja, @StopaOtpisa, 
		@SintetickiKonto, @KontoOtpisa, @IznosOtpisa, @SadasnjaVrijednost, @DatumRashodovanja, @VrstaPoTrajanju);
END