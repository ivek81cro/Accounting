CREATE PROCEDURE [dbo].[spPayrollSupplementEmployee_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, Iznos, Sifra, Naziv
	from PayrollSupplementEmployee;
END