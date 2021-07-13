CREATE PROCEDURE [dbo].[spBankReports_Delete]
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM BankReport WHERE Id=@Id;

	DELETE FROM BankReportItems WHERE IdIzvod=@Id;
END