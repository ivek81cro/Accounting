CREATE PROCEDURE [dbo].[spCities_GetByName]
@Mjesto nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Mjesto, Posta, Zupanija, Drzava, Prirez, Sifra
	FROM City
	WHERE Mjesto=@Mjesto;

END
