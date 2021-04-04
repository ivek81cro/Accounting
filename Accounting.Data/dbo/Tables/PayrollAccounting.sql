CREATE TABLE [dbo].[PayrollAccounting]
(
	[Id] INT NOT NULL IDENTITY,
	[Opis] NVARCHAR(MAX) NOT NULL,
	[DatumOd] DATETIME2 NOT NULL,
	[DatumDo] DATETIME2 NOT NULL,
	[SatiRada] INT NOT NULL,
	[SatiPraznika] INT NOT NULL,
	[DatumObracuna]  DATETIME2 NOT NULL
)
