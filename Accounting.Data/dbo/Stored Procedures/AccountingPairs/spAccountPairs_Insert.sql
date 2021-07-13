CREATE PROCEDURE [dbo].[spAccountPairs_Insert]
@Id INT,
@Name NVARCHAR(255),
@Description NVARCHAR(255),
@Account NVARCHAR(25),
@BookName NVARCHAR(75)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO AccountPairs (Name, Description, Account, BookName)
	VALUES (@Name, @Description, @Account, @BookName);
END