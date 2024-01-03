CREATE PROCEDURE [dbo].[spCities_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, Mjesto, Posta, Zupanija, Drzava, Prirez, Porez1, Porez2, Sifra
	FROM City;

END
