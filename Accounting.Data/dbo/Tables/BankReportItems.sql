CREATE TABLE [dbo].[BankReportItems]
(
	[Id] INT NOT NULL IDENTITY, 
    [IdIzvod] INT NOT NULL, 
    [Naziv] NVARCHAR(255) NOT NULL, 
    [Opis] NVARCHAR(255) NOT NULL, 
    [Konto] NVARCHAR(25) NOT NULL, 
    [Dugovna] DECIMAL(9, 2) NULL, 
    [Potrazna] DECIMAL(9, 2) NULL, 
    [Strana] NCHAR(1) NOT NULL
)
