CREATE PROCEDURE [dbo].[spPartners_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Naziv, Ulica, Broj, Mjesto, Telefon, Fax, Email, Iban, Mbo, KontoK, KontoD
	FROM Partners;
END
