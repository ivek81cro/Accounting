CREATE TABLE [dbo].[Assets]
(
	[Id] INT NOT NULL IDENTITY,
	[Naziv] nvarchar(255) NOT NULL,
	[DatumNabave] datetime2,
	[Kolicina] decimal(9,2) NOT NULL,
	[Lokacija] nvarchar(255),
	[InvBroj] int,
	[Dobavljac] nvarchar(255) NOT NULL,
	[Dokument] nvarchar(255) NOT NULL,
	[DatumUporabe] datetime2,
	[NabavnaVrijednost] decimal(9,2) NOT NULL,
	[Skupina] nvarchar(255),
	[VijekTrajanja] decimal(9,2),
	[StopaOtpisa] decimal(9,2),
	[SintetickiKonto] nvarchar(255) NOT NULL,
	[KontoOtpisa] nvarchar(255) NOT NULL,
	[IznosOtpisa] decimal(9,2),
	[SadasnjaVrijednost] decimal(9,2),
	[DatumRashodovanja] datetime2,
	[VrstaPoTrajanju] nvarchar(255)
)
