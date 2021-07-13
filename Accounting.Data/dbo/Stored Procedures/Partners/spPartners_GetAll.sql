CREATE PROCEDURE [dbo].[spPartners_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Oib, Naziv, Ulica, Broj, Posta, Mjesto, Telefon, Fax, Email, Iban, Mbo, KontoK, KontoD
	FROM Partners;
END
