CREATE PROCEDURE [dbo].[spBookAccountsSettings_Insert]
@Id int, 
@BookName nvarchar(50), 
@Name nvarchar(100), 
@Account nvarchar(20), 
@Side nvarchar(20),
@Prefix nvarchar(1)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO BookAccountSettings (BookName, Name, Account, Side, Prefix)
	VALUES (@BookName, @Name, @Account, @Side, @Prefix);
END