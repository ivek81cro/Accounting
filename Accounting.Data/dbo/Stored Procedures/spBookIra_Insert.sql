﻿CREATE PROCEDURE [dbo].[spBookIra_Insert]
@Id INT,
@RedniBroj INT,
@BrojRacuna NVARCHAR(250),
@Storno BIT,
@IzRacuna INT,
@Datum DATETIME2,
@Dospijece DATETIME2,
@DatumZadnjeUplate DATETIME2,
@NazivISjedisteKupca NVARCHAR(255),
@Oib NVARCHAR(11),
@IznosSPdv DECIMAL(9,2),
@OslobodjenoPdvEU DECIMAL(9,2),
@OslobodjenoPdvOstalo DECIMAL(9,2),
@ProlaznaStavka DECIMAL(9,2),
@PoreznaOsnovica0 DECIMAL(9,2),
@PoreznaOsnovica5 DECIMAL(9,2),
@Pdv5 DECIMAL(9,2),
@PoreznaOsnovica10 DECIMAL(9,2),
@Pdv10 DECIMAL(9,2),
@PoreznaOsnovica13 DECIMAL(9,2),
@Pdv13 DECIMAL(9,2),
@PoreznaOsnovica23 DECIMAL(9,2),
@Pdv23 DECIMAL(9,2),
@PoreznaOsnovica25 DECIMAL(9,2),
@Pdv25 DECIMAL(9,2),
@UkupniPdv DECIMAL(9,2),
@UkupnoUplaceno DECIMAL(9,2),
@PreostaloZaUplatit DECIMAL(9,2),
@NapomenaORacunu NVARCHAR(255),
@ZaprimljenUHzzo DATETIME2,
@DanaOdZaprimanja INT,
@DanaNeplacanja INT,
@Knjizen BIT,
@TemeljnicaId INT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO BookIra (RedniBroj, BrojRacuna, Storno, IzRacuna, Datum, Dospijece, DatumZadnjeUplate, 
	NazivISjedisteKupca, Oib, IznosSPdv, OslobodjenoPdvEU, OslobodjenoPdvOstalo, ProlaznaStavka, PoreznaOsnovica0, 
	PoreznaOsnovica5, Pdv5, PoreznaOsnovica10, Pdv10, PoreznaOsnovica13, Pdv13, PoreznaOsnovica23, Pdv23, PoreznaOsnovica25, 
	Pdv25, UkupniPdv, UkupnoUplaceno, PreostaloZaUplatit, NapomenaORacunu, ZaprimljenUHzzo, DanaOdZaprimanja, DanaNeplacanja, Knjizen,
	TemeljnicaId)
	VALUES (@RedniBroj, @BrojRacuna, @Storno, @IzRacuna, @Datum, @Dospijece, @DatumZadnjeUplate, 
	@NazivISjedisteKupca, @Oib, @IznosSPdv, @OslobodjenoPdvEU, @OslobodjenoPdvOstalo, @ProlaznaStavka, @PoreznaOsnovica0, 
	@PoreznaOsnovica5, @Pdv5, @PoreznaOsnovica10, @Pdv10, @PoreznaOsnovica13, @Pdv13, @PoreznaOsnovica23, @Pdv23, @PoreznaOsnovica25, 
	@Pdv25, @UkupniPdv, @UkupnoUplaceno, @PreostaloZaUplatit, @NapomenaORacunu, @ZaprimljenUHzzo, @DanaOdZaprimanja, @DanaNeplacanja, @Knjizen,
	@TemeljnicaId);
END