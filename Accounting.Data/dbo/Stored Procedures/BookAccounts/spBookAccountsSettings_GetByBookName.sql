CREATE PROCEDURE [dbo].[spBookAccountsSettings_GetByBookName]
@BookName nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, BookName, Name, Account, Side, Prefix
	FROM BookAccountSettings
	WHERE BookName=@BookName;
END