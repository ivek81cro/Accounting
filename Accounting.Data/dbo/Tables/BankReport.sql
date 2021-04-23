CREATE TABLE [dbo].[BankReport]
(
	[Id] INT NOT NULL IDENTITY, 
    [RedniBroj] INT NOT NULL UNIQUE, 
    [DatumIzvoda] DATETIME2 NOT NULL, 
    [SumaPotrazna] DECIMAL(9, 2) NOT NULL, 
    [SumaDugovna] DECIMAL(9, 2) NOT NULL, 
    [StanjePrethodnogIzvoda] DECIMAL(9, 2) NOT NULL, 
    [NovoStanje] DECIMAL(9, 2) NOT NULL, 
    [Knjizen] BIT NOT NULL
)
