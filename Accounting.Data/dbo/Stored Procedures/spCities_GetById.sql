CREATE PROCEDURE [dbo].[spCities_GetById]
@Id int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Mjesto, Posta, Zupanija, Drzava, Prirez, Sifra
	FROM City
	WHERE Id=@Id;

END
