﻿CREATE TABLE [dbo].[BookUraRepro]
(
	[Id] INT NOT NULL IDENTITY,
	[DatumKnjizenja] datetime2,
	[BrojPrimke] int,
	[Storno] bit,
	[NazivDobavljaca] nvarchar(255),
	[BrojRacuna] nvarchar(255),
	[DatumRacuna] datetime2,
	[Otpremnica] bit,
	[DospijecePlacanja] datetime2,
	[FakturnaVrijednost] decimal(9,2),
	[NabavnaVrijednost] decimal(9,2),
	[NettoNabavnaVrijednost] decimal(9,2),
	[Pretporez] decimal(9,2),
	[Rabat] decimal(9,2),
	[VeleprodajniRabat] decimal(9,2),
	[CassaSconto] decimal(9,2),
	[PorezniBroj] nvarchar(50),
	[BrojUKnjiziUra] int, 
    [Knjizen] BIT NULL,
	[TemeljnicaId] INT
)
