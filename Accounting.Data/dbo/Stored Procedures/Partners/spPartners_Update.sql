CREATE PROCEDURE [dbo].[spPartners_Update]
@Id int,
@Oib nvarchar(11),
@Naziv nvarchar(150),
@Ulica nvarchar(100),
@Broj nvarchar(10),
@Posta nvarchar(10),
@Mjesto nvarchar(100),
@Telefon nvarchar(25),
@Fax nvarchar(25),
@Email nvarchar(100),
@Iban nvarchar(25),
@Mbo nvarchar(15),
@KontoK nvarchar(15),
@KontoD nvarchar(15)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE Partners 
	SET Oib=@Oib, Naziv=@Naziv, Ulica=@Ulica, Broj=@Broj, Posta=@Posta, Mjesto=@Mjesto, 
	Telefon=@Telefon, Fax=@Fax, Email=@Email, Iban=@Iban, Mbo=@Mbo, KontoK=@KontoK, KontoD=@KontoD
	WHERE Id=@Id;
END
