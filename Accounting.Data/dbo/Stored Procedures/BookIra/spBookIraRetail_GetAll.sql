CREATE PROCEDURE [dbo].[spBookIraRetail_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, RedniBroj, Datum, Stopa, NaplacenaVrijednost, PoreznaOsnovica, NettoRuc, Pdv, NabavnaVrijednost,
	StornoMarze, StornoPdv, MaloprodajnaVrijednost, Knjizen, TemeljnicaId
	FROM BookIraRetail;
END