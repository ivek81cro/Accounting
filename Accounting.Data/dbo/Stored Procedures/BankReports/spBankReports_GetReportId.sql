CREATE PROCEDURE [dbo].[spBankReports_GetReportId]
@RedniBroj int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 1 Id
	FROM BankReport
	WHERE RedniBroj = @RedniBroj
	ORDER BY Id DESC;
END