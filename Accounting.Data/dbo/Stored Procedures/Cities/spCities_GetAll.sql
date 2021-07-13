CREATE PROCEDURE [dbo].[spCities_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, Mjesto, Posta, Zupanija, Drzava, Prirez, Sifra
	FROM City;

END
