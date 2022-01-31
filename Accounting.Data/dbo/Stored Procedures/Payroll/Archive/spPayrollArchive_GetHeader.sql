CREATE PROCEDURE [dbo].[spPayrollArchive_GetHeader]
@id int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, UniqueId, Opis, SatiRada, SatiPraznika, DatumOd, DatumDo, DatumObracuna, Knjizen, TemeljnicaId
	FROM PayrollArchiveHeader
	WHERE Id=@id;
END