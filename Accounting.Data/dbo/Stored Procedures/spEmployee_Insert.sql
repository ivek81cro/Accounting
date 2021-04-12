CREATE PROCEDURE [dbo].[spEmployee_Insert]
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
@Iban  nvarchar(75),
@DatumDolaska datetime2,
@DatumOdlaska datetime2
AS
BEGIN
	DECLARE @SifraOpcineRada nvarchar(10);
	DECLARE @MjestoPoslodavac nvarchar(75);

	SET NOCOUNT ON;

	INSERT INTO Employee (Oib, Ime, Prezime, Ulica, Broj, Mjesto, Drzava, Telefon, Email, StrucnaSprema, Zvanje, Olaksica, Iban, DatumDolaska, DatumOdlaska)
	VALUES (@Oib, @Ime, @Prezime, @Ulica, @Broj, @Mjesto, @Drzava, @Telefon, @Email, @StrucnaSprema, @Zvanje, @Olaksica, @Iban, @DatumDolaska, @DatumOdlaska);

	INSERT INTO Payroll (Oib, Ime, Prezime) VALUES(@Oib, @Ime, @Prezime);

	SET @MjestoPoslodavac = (SELECT TOP 1 Mjesto FROM Company)
	SET @SifraOpcineRada = (SELECT Sifra FROM City WHERE Mjesto=@MjestoPoslodavac);

	INSERT INTO JoppdEmployee (Oib, SifraOpcineRada)
	VALUES (@Oib, @SifraOpcineRada);
END
