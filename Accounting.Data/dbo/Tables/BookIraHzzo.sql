CREATE TABLE [dbo].[BookIraHzzo]
(
	[Id] INT NOT NULL IDENTITY,
	[DatumPlacanja] DATETIME2 NOT NULL,
	[Dokument] NVARCHAR(255) NOT NULL,
	[OriginalniBroj] NVARCHAR(255) NOT NULL,
	[DatumDokumenta] DATETIME2 NOT NULL,
	[Program] NVARCHAR(255) NOT NULL,
	[Opis] NVARCHAR(255) NOT NULL,
	[IznosRacuna] DECIMAL(9, 2) NOT NULL,
	[PlaceniIznos] DECIMAL(9, 2) NOT NULL,
	[Povezan] BIT NOT NULL
)
