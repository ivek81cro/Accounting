CREATE PROCEDURE [dbo].[spPayroll_GetByOib]
@Oib nvarchar(11)
AS
	SELECT Oib, Ime, Prezime, Bruto, Mio1, Mio2, Dohodak, Odbitak, PoreznaOsnovica, PoreznaStopa1, PoreznaStopa2, UkupnoPorez,
	Prirez, UkupnoPorezPrirez, Neto, DoprinosZdravstvo, SamoPrviStupMio
	FROM Payroll
	WHERE Oib=@Oib;
RETURN 0
