CREATE TABLE [dbo].[PayrollArchiveHeader]
(
	[Id] INT NOT NULL IDENTITY,
	[UniqueId] NVARCHAR(255) NOT NULL UNIQUE,
	[Opis] NVARCHAR(255) NOT NULL,
	[DatumOd] DATETIME2 NOT NULL,
	[DatumDo] DATETIME2 NOT NULL,
	[SatiRada] INT,
	[SatiPraznika] INT,
	[DatumObracuna] DATETIME2 NOT NULL, 
    [Knjizen] BIT NULL,
	[TemeljnicaId] INT
)
