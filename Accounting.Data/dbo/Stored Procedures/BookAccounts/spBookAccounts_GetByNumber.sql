CREATE PROCEDURE [dbo].[spBookAccounts_GetByNumber]
@Konto NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Konto, Opis
	FROM BookAccount
	WHERE Konto=@Konto;
END
