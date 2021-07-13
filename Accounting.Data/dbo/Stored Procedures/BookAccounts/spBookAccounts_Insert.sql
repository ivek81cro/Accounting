CREATE PROCEDURE [dbo].[spBookAccounts_Insert]
@Konto nvarchar(25),
@Opis nvarchar(255)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO BookAccount (Konto, Opis)
	VALUES (@Konto, @Opis);
END