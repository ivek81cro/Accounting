CREATE PROCEDURE [dbo].[spPayrollSupplement_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Sifra, Naziv
	FROM PayrollSupplement;
END
