CREATE PROCEDURE [dbo].[spEmployee_GetById]
@Id int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Oib, Ime, Prezime, Ulica, Broj, Mjesto, Drzava, Telefon, Email, StrucnaSprema, Zvanje, Olaksica, Iban, DatumDolaska, DatumOdlaska
	FROM Employee
	WHERE Id=@Id;

END
