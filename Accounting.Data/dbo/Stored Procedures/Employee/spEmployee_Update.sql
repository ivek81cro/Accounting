CREATE PROCEDURE [dbo].[spEmployee_Update]
@Id int,
@Oib nvarchar(75),
@Ime nvarchar(75),
@Prezime nvarchar(75),
@Ulica nvarchar(75),
@Broj nvarchar(75),
@Mjesto nvarchar(75),
@Drzava nvarchar(75),
@Telefon nvarchar(75),
@Email nvarchar(75),
@StrucnaSprema nvarchar(75),
@Zvanje  nvarchar(75),
@Olaksica decimal(8,2),
@Iban nvarchar(75),
@DatumDolaska datetime2,
@DatumOdlaska datetime2
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE Employee 
	SET Oib=@Oib, Ime=@Ime, Prezime=@Prezime, Ulica=@Ulica, Broj=@Broj, Mjesto=@Mjesto, Drzava=@Drzava,
	Telefon=@Telefon, Email=@Email, StrucnaSprema=@StrucnaSprema, Zvanje=@Zvanje, Olaksica=@Olaksica,
	Iban=@Iban, DatumDolaska=@DatumDolaska,	DatumOdlaska=@DatumOdlaska
	WHERE Id=@Id;

	UPDATE Payroll
	SET Ime = @Ime, Prezime=@Prezime
	WHERE Oib=@Oib;

	DECLARE @SifraMjesto nvarchar(10);
	SET @SifraMjesto = (SELECT Sifra FROM City WHERE Mjesto=@Mjesto);

	DECLARE @MjestoPoslodavac nvarchar(75);
	SET @MjestoPoslodavac = (SELECT TOP 1 Mjesto FROM Company);

	DECLARE @SifraOpcineRada nvarchar(10);
	SET @SifraOpcineRada = (SELECT Sifra FROM City WHERE Mjesto=@MjestoPoslodavac);

	UPDATE JoppdEmployee 
	SET SifraPrebivalista = @SifraMjesto, SifraOpcineRada=@SifraOpcineRada
	WHERE Oib=@Oib;
END
