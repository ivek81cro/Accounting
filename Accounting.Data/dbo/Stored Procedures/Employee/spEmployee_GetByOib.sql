CREATE PROCEDURE [dbo].[spEmployee_GetByOib]
@Oib nvarchar(11)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Oib, Ime, Prezime, Ulica, Broj, Mjesto, Drzava, Telefon, Email, StrucnaSprema, Zvanje, Olaksica, Iban, DatumDolaska, DatumOdlaska
	FROM Employee
	WHERE Oib = @Oib;
END