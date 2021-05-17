CREATE TABLE [dbo].[BookIraRetail]
(
	[Id] INT NOT NULL IDENTITY,
	[RedniBroj] INT NOT NULL,
	[Datum] DATETIME2 NOT NULL,
	[Stopa] DECIMAL(9,2) NOT NULL,
    [NaplacenaVrijednost] DECIMAL(9,2) NOT NULL,
    [PoreznaOsnovica] DECIMAL(9,2) NOT NULL,
    [NettoRuc] DECIMAL(9,2) NOT NULL,
    [Pdv] DECIMAL(9,2) NOT NULL,
    [NabavnaVrijednost] DECIMAL(9,2) NOT NULL,
    [StornoMarze] DECIMAL(9,2) NOT NULL,
    [StornoPdv] DECIMAL(9,2) NOT NULL,
    [MaloprodajnaVrijednost] DECIMAL(9,2) NOT NULL,
    [Knjizen] BIT NOT NULL
)
