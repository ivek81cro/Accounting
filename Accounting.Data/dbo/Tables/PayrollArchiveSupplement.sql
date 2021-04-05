CREATE TABLE [dbo].[PayrollArchiveSupplement]
(
	[Id] INT NOT NULL IDENTITY,
	[Oib] NVARCHAR(11) NOT NULL,
	[Sifra] NVARCHAR(10) NOT NULL,
	[Naziv] NVARCHAR(MAX) NOT NULL,
	[Iznos] DECIMAL(8,2) NOT NULL,
	[AccountingId] INT NOT NULL
)
