CREATE PROCEDURE [dbo].[spCities_Insert]
@Id int,
@Mjesto nvarchar(50),
@Posta nvarchar(50),
@Zupanija nvarchar(75),
@Drzava nvarchar(75),
@Prirez decimal(8,2),
@Sifra nvarchar(10)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO City (Id, Mjesto, Posta, Zupanija, Drzava, Prirez, Sifra)
	VALUES (@Id, @Mjesto, @Posta, @Zupanija, @Drzava, @Prirez, @Sifra)
END
