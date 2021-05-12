CREATE TABLE [dbo].[AccountingJournal]
(
	[Id] INT IDENTITY,
    [Opis] NVARCHAR(255) NOT NULL,
    [Dokument] NVARCHAR(255) NOT NULL,
    [Broj] INT NOT NULL,
    [Konto] NVARCHAR(25) NOT NULL,
    [Datum] DATETIME2 NOT NULL,
    [Valuta] NVARCHAR(5) NOT NULL,
    [Dugovna] DECIMAL(9,2) NOT NULL,
    [Potražna] DECIMAL(9,2) NOT NULL,
    [VrstaTemeljnice] NVARCHAR(125) NOT NULL,
    [BrojTemeljnice] INT NOT NULL
)
