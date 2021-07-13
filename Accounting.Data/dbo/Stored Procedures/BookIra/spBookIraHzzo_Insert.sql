CREATE PROCEDURE [dbo].[spBookIraHzzo_Insert]
@Id INT,
@DatumPlacanja DATETIME2,
@Dokument NVARCHAR(255),
@OriginalniBroj NVARCHAR(255),
@DatumDokumenta DATETIME2,
@Program NVARCHAR(255),
@Opis NVARCHAR(255),
@IznosRacuna DECIMAL(9,2),
@PlaceniIznos DECIMAL(9,2),
@Povezan BIT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO BookIraHzzo (DatumPlacanja, Dokument, OriginalniBroj, DatumDokumenta, Program, Opis, IznosRacuna, PlaceniIznos, Povezan)
	VALUES(@DatumPlacanja, @Dokument, @OriginalniBroj, @DatumDokumenta, @Program, @Opis, @IznosRacuna, @PlaceniIznos, @Povezan);

END
