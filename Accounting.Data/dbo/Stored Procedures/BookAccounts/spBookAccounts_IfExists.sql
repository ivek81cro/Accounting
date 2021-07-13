CREATE PROCEDURE [dbo].[spBookAccounts_IfExists]
@Konto nvarchar(25)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Konto
	FROM BookAccount
	WHERE Konto=@Konto;
END