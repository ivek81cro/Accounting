CREATE TABLE [dbo].[Employee]
(
    [Id] INT NOT NULL IDENTITY,
	[Oib] NVARCHAR(11) NOT NULL UNIQUE,
    [Ime] NVARCHAR(75) NOT NULL,
    [Prezime] NVARCHAR(75) NOT NULL,
    [Ulica] NVARCHAR(75),
    [Broj] NVARCHAR(75),
    [Mjesto] NVARCHAR(75),
    [Drzava] NVARCHAR(75),
    [Telefon] NVARCHAR(75),
    [Email] NVARCHAR(75),
    [StrucnaSprema] NVARCHAR(75),
    [Zvanje] NVARCHAR(75),
    [Olaksica] DECIMAL(8,2) NOT NULL,
    [Iban] NVARCHAR(75),
    [DatumDolaska] DATETIME2 NOT NULL,
    [DatumOdlaska] DATETIME2
)
