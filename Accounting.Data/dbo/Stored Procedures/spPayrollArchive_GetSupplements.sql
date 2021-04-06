CREATE PROCEDURE [dbo].[spPayrollArchive_GetSupplements]
@AccountingId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Oib, Naziv, Iznos, Sifra
	FROM PayrollArchiveSupplement
	WHERE AccountingId=@AccountingId
END
