CREATE PROCEDURE [dbo].[spPartners_AddNew]
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

	INSERT INTO Partners (Oib, Naziv, Ulica, Broj, Posta, Mjesto, Telefon, Fax, Email, Iban, Mbo, KontoK, KontoD)
	VALUES (@Oib, @Naziv, @Ulica, @Broj, @Posta, @Mjesto, @Telefon, @Fax, @Email, @Iban, @Mbo, @KontoK, @KontoD);
END