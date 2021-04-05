CREATE PROCEDURE [dbo].[spPayrollSupplementEmployee_GetByOib]
@Oib nvarchar(11)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, Oib, Iznos, Naziv, Sifra
	FROM PayrollSupplementEmployee
	WHERE Oib=@Oib
END