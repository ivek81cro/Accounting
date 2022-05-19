CREATE PROCEDURE [dbo].[spBookIraHzzo_Update]
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
	
	DECLARE @Racun as NVARCHAR(5);
	DECLARE @Godina as NVARCHAR(4);
	SET @Racun = (SELECT TOP 1 * FROM STRING_SPLIT(@OriginalniBroj, '-'));
	SET @Godina = SUBSTRING(@OriginalniBroj, PATINDEX('%/%', @OriginalniBroj)+1, 4);

	IF(@Racun <> @Godina)
	BEGIN
		UPDATE BookIra SET UkupnoUplaceno+=@PlaceniIznos, PreostaloZaUplatit-=@PlaceniIznos 
		WHERE (BrojRacuna=@Racun OR (IzRacuna=@Racun AND IznosSPdv > 0)) AND year(Datum)=@Godina;
		UPDATE BookIraHzzo SET Povezan=1 WHERE Id=@Id;
	END

END
