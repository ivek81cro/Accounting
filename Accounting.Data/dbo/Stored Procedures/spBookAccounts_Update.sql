CREATE PROCEDURE [dbo].[spBookAccounts_Update]
@Konto nvarchar(25),
@Opis nvarchar(255)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE BookAccount SET Opis = @Opis
	WHERE Konto=@Konto;
END