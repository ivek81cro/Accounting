CREATE PROCEDURE [dbo].[spCompany_Get]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Oib, Naziv, Ulica, Broj, Posta, Mjesto, Telefon, Fax, Email, Iban, VrstaDjelatnosti, SifraDjelatnosti, NazivDjelatnosti, Mbo
	FROM Company;
END
