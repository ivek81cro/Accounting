CREATE PROCEDURE [dbo].[spPayroll_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Oib, Ime, Prezime, Bruto, Mio1, Mio2, Dohodak, Odbitak, PoreznaOsnovica, PoreznaStopa1, PoreznaStopa2, UkupnoPorez,
	Prirez, UkupnoPorezPrirez, Neto, DoprinosZdravstvo, SamoPrviStupMio
	FROM Payroll;
END