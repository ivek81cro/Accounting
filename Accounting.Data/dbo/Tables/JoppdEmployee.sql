CREATE TABLE [dbo].[JoppdEmployee]
(
	[Oib] NVARCHAR(11) NOT NULL UNIQUE,
	[SifraPrebivalista] NVARCHAR(10) NULL,
    [SifraOpcineRada] NVARCHAR(10) NULL,
    [OznakaStjecatelja] NVARCHAR(10) NULL,
    [OznakaPrimitka] NVARCHAR(10) NULL,
    [DodatniMio] NVARCHAR(10) NULL,
    [ObvezaInvaliditet] NVARCHAR(10) NULL,
    [PrviZadnjiMjesec] NVARCHAR(10) NULL,
    [PunoNepunoRadnoVrijeme] NVARCHAR(10) NULL,
    [NacinIsplate] NVARCHAR(10) NULL
)
