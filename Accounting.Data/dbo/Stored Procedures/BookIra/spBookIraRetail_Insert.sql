CREATE PROCEDURE [dbo].[spBookIraRetail_Insert]
@Id INT,
@RedniBroj INT,
@Datum DATETIME2,
@Stopa DECIMAL(9,2),
@NaplacenaVrijednost DECIMAL(9,2),
@PoreznaOsnovica DECIMAL(9,2),
@NettoRuc DECIMAL(9,2),
@Pdv DECIMAL(9,2),
@NabavnaVrijednost DECIMAL(9,2),
@StornoMarze DECIMAL(9,2),
@StornoPdv DECIMAL(9,2),
@MaloprodajnaVrijednost DECIMAL(9,2),
@Knjizen BIT,
@TemeljnicaId INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO BookIraRetail (RedniBroj, Datum, Stopa, NaplacenaVrijednost, PoreznaOsnovica, NettoRuc, Pdv, NabavnaVrijednost,
	StornoMarze, StornoPdv, MaloprodajnaVrijednost, Knjizen, TemeljnicaId)
	VALUES (@RedniBroj, @Datum, @Stopa, @NaplacenaVrijednost, @PoreznaOsnovica, @NettoRuc, @Pdv, @NabavnaVrijednost,
	@StornoMarze, @StornoPdv, @MaloprodajnaVrijednost, @Knjizen, @TemeljnicaId);
END