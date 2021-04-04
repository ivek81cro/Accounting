CREATE PROCEDURE [dbo].[spPayrollSupplementEmployee_GetDistinct]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT Sifra, Naziv
	FROM PayrollSupplementEmployee;
END
