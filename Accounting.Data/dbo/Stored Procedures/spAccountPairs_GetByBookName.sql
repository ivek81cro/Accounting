CREATE PROCEDURE [dbo].[spAccountPairs_GetByBookName]
@BookName NVARCHAR(75)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, Name, Description, Account
	FROM AccountPairs
	WHERE BookName=@BookName;
END