CREATE TABLE [dbo].[Payroll]
(
	[Oib] NVARCHAR(11) NOT NULL UNIQUE,
	[Ime] NVARCHAR(75) NOT NULL,
	[Prezime] NVARCHAR(75) NOT NULL,
	[Bruto] DECIMAL(8,2),
	[Mio1] DECIMAL(8,2),
	[Mio2] DECIMAL(8,2),
	[Dohodak] DECIMAL(8,2),
	[Odbitak] DECIMAL(8,2),
	[PoreznaOsnovica] DECIMAL(8,2),
	[PoreznaStopa1] DECIMAL(8,2),
	[PoreznaStopa2] DECIMAL(8,2),
	[UkupnoPorez] DECIMAL(8,2),
	[Prirez] DECIMAL(8,2),
	[UkupnoPorezPrirez] DECIMAL(8,2),
	[Neto] DECIMAL(8,2),
	[DoprinosZdravstvo] DECIMAL(8,2),
	[SamoPrviStupMio] BIT
)
