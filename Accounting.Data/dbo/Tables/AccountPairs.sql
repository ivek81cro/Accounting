﻿CREATE TABLE [dbo].[AccountPairs]
(
	[Id] INT NOT NULL IDENTITY,
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(255) NULL,
	[Account] NVARCHAR(25) NOT NULL,
	[BookName] NVARCHAR(75) NOT NULL
)
