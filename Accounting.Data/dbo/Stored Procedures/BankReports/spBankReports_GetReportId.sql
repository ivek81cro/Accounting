CREATE PROCEDURE [dbo].[spBankReports_GetReportId]
@RedniBroj int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id
	FROM BankReport
	WHERE RedniBroj = @RedniBroj;
END