CREATE PROCEDURE [dbo].[spCities_Update]
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
	
	UPDATE City SET Mjesto=@Mjesto, Posta=@Posta, Zupanija=@Zupanija, Drzava=@Drzava, Sifra=@Sifra, Prirez=@Prirez
	WHERE Id=@Id;
END