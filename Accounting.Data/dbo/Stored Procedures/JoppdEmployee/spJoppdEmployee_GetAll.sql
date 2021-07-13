CREATE PROCEDURE [dbo].[spJoppdEmployee_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Oib, SifraPrebivalista, SifraOpcineRada, OznakaStjecatelja, OznakaPrimitka,
	DodatniMio, ObvezaInvaliditet, PrviZadnjiMjesec, PunoNepunoRadnoVrijeme, NacinIsplate
	FROM JoppdEmployee;
END