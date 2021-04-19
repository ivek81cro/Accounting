﻿CREATE TABLE [dbo].[BookUraRest]
(
	[Id] INT NOT NULL IDENTITY,
	[RedniBroj] INT NOT NULL,
	[Datum] DATETIME2 NOT NULL,
	[BrojRacuna] NVARCHAR(150) NOT NULL,
	[Storno] BIT  NULL,
	[StornoBroja] INT NULL,
	[DatumRacuna] DATETIME2 NOT NULL,
	[StarostRacuna] INT NOT NULL,
	[Dospijece] DATETIME2 NOT NULL,
	[PlaniranaUplata] DECIMAL(9,2) NULL,
	[DatumUplate] DATETIME2 NULL,
	[ZaUplatu] DECIMAL(9,2) NOT NULL,
	[NazivDobavljaca] NVARCHAR(255) NOT NULL,
	[BrojPrimke] INT NULL,
	[NapomenaORacunu] NVARCHAR(255) NULL,
	[NettoNabavnaVrijednost] DECIMAL(9,2) NOT NULL,
	[SjedisteDobavljaca] NVARCHAR(255) NULL,
	[OIB] NVARCHAR(11) NOT NULL,
	[IznosSPorezom] DECIMAL(9,2) NOT NULL,
	[PoreznaOsnovica0] DECIMAL(9,2) NOT NULL,
	[PoreznaOsnovica5] DECIMAL(9,2) NOT NULL,
	[PretporezT5] DECIMAL(9,2) NOT NULL,
	[PoreznaOsnovica10] DECIMAL(9,2) NOT NULL,
	[PretporezT10] DECIMAL(9,2) NOT NULL,
	[PoreznaOsnovica13] DECIMAL(9,2) NOT NULL,
	[PretporezT13] DECIMAL(9,2) NOT NULL,
	[PoreznaOsnovica23] DECIMAL(9,2) NOT NULL,
	[PretporezT23] DECIMAL(9,2) NOT NULL,
	[PoreznaOsnovica25] DECIMAL(9,2) NOT NULL,
	[PretporezT25] DECIMAL(9,2) NOT NULL,
	[UkupniPretporez] DECIMAL(9,2) NOT NULL,
	[MozeSeOdbiti] DECIMAL(9,2) NOT NULL,
	[NeMozeSeOdbiti] DECIMAL(9,2) NOT NULL,
	[IznosBezPoreza] DECIMAL(9,2) NOT NULL,
	[ProlaznaStavka] DECIMAL(9,2) NOT NULL,
	[Neoporezivo] DECIMAL(9,2) NOT NULL,
	[CassaScontoPercent] DECIMAL(9,2) NOT NULL,
	[CassaSconto] DECIMAL(9,2) NOT NULL,
	[BrojOdobrenja] NVARCHAR(50) NULL,
	[OdobrenjaBezPDV] NVARCHAR(50) NULL,
	[OdobreniPDV] DECIMAL(9,2) NULL,
	[DatumPodnosenja] DATETIME2 NULL,
	[DatumIzvrsenja] DATETIME2 NULL,
	[UkupnoUplaceno] DECIMAL(9,2) NULL,
	[PreostaloZaUplatit] DECIMAL(9,2) NULL,
	[DospijeceDana] INT NULL,
	[Knjizen] BIT NULL
)
