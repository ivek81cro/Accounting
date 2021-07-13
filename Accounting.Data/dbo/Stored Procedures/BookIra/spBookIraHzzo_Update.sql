CREATE PROCEDURE [dbo].[spBookIraHzzo_Update]
@Id INT,
@Racun NVARCHAR(5),
@Godina NVARCHAR(4),
@Iznos DECIMAL(9,2)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE BookIra SET UkupnoUplaceno+=@Iznos, PreostaloZaUplatit-=@Iznos WHERE BrojRacuna=@Racun AND year(Datum)=@Godina;
	UPDATE BookIraHzzo SET Povezan=1 WHERE Id=@Id;

END
