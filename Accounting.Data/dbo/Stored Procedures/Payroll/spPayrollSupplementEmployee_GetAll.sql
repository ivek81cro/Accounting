CREATE PROCEDURE [dbo].[spPayrollSupplementEmployee_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, Oib, Iznos, Sifra, Naziv
	from PayrollSupplementEmployee;
END