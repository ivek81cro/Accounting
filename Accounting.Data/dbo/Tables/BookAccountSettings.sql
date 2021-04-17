CREATE TABLE [dbo].[BookAccountSettings]
(
	[Id] INT NOT NULL IDENTITY,
	[BookName] nvarchar(50) NOT NULL,
	[Name] nvarchar(100) NOT NULL,
	[Account] nvarchar(20) NOT NULL,
	[Side] nvarchar(20) NOT NULL,
	[Prefix] NVARCHAR NOT NULL
)
