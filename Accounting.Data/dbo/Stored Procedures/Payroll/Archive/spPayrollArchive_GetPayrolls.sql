CREATE PROCEDURE [dbo].[spPayrollArchive_GetPayrolls]
@AccountingId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Oib, Ime, Prezime, Bruto, Mio1, Mio2, Dohodak, Odbitak, PoreznaOsnovica, PoreznaStopa1, PoreznaStopa2, UkupnoPorez,
	Prirez, UkupnoPorezPrirez, Neto, DoprinosZdravstvo, SamoPrviStupMio, AccountingId
	FROM PayrollArchivePayroll
	WHERE AccountingId=@AccountingId;
END
