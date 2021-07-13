CREATE PROCEDURE [dbo].[spBookAccounts_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Konto, Opis
	FROM BookAccount;
END