CREATE PROCEDURE [dbo].[spPartners_GetByOib]
@Oib nvarchar(11)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Oib, Naziv, Ulica, Broj, Posta, Mjesto, Telefon, Fax, Email, Iban, Mbo, KontoK, KontoD
	FROM Partners
	WHERE Oib=@Oib;
END