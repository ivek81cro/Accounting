CREATE PROCEDURE [dbo].[spPartners_GetById]
@Id int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Oib, Naziv, Ulica, Broj, Posta, Mjesto, Telefon, Fax, Email, Iban, Mbo, KontoK, KontoD
	FROM Partners
	WHERE Id=@Id;
END
