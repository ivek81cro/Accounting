CREATE PROCEDURE [dbo].[spProcessedBankReports_Get]
AS
BEGIN
	SELECT Broj FROM AccountingJournal WHERE VrstaTemeljnice='Izvodi' and Konto='1000' ORDER BY Broj
END
