CREATE PROCEDURE [dbo].[spCities_Update]
@Id int,
@Mjesto nvarchar(50),
@Posta nvarchar(50),
@Zupanija nvarchar(75),
@Drzava nvarchar(75),
@Prirez decimal(8,2),
@Porez1 decimal(8,2),
@Porez2 decimal(8,2),
@Sifra nvarchar(10)
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE City SET Mjesto=@Mjesto, Posta=@Posta, Zupanija=@Zupanija, Drzava=@Drzava, Sifra=@Sifra, 
	Prirez=@Prirez, Porez1=@Porez1, Porez2=@Porez2
	WHERE Id=@Id;
END