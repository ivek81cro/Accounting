CREATE PROCEDURE [dbo].[spCompany_Update]
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
@VrstaDjelatnosti nvarchar(15),
@SifraDjelatnosti nvarchar(15),
@NazivDjelatnosti nvarchar(55)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE Company 
	SET Oib=@Oib, Naziv=@Naziv, Ulica=@Ulica, Broj=@Broj, Posta=@Posta, Mjesto=@Mjesto, 
	Telefon=@Telefon, Fax=@Fax, Email=@Email, Iban=@Iban, Mbo=@Mbo, NazivDjelatnosti=@NazivDjelatnosti, VrstaDjelatnosti=@VrstaDjelatnosti,
	SifraDjelatnosti=@SifraDjelatnosti
	WHERE Id=@Id;
END