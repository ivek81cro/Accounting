CREATE TABLE [dbo].[CashRegisterJournal]
(
	[Id] INT NOT NULL IDENTITY,
	[Datum] datetime2 NOT NULL,
	[RedniBroj] INT NOT NULL,
	[Gotovina] decimal(9,2) NOT NULL,
	[KreditneKartice] decimal(9,2) NOT NULL,
	[Sveukupno] decimal(9,2) NOT NULL,
	[IznosSudjelovanja] decimal(9,2) NOT NULL
)
