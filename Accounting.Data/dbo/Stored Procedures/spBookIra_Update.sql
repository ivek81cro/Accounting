﻿CREATE PROCEDURE [dbo].[spBookIra_Update]
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
	UPDATE BookIra SET BrojRacuna=@BrojRacuna, Storno=@Storno, IzRacuna=@IzRacuna, Datum=@Datum, Dospijece=@Dospijece, 
		DatumZadnjeUplate=@DatumZadnjeUplate, NazivISjedisteKupca=@NazivISjedisteKupca, Oib=@Oib, IznosSPdv=@IznosSPdv, 
		OslobodjenoPdvEU=@OslobodjenoPdvEU, OslobodjenoPdvOstalo=@OslobodjenoPdvOstalo, ProlaznaStavka=@ProlaznaStavka, 
		PoreznaOsnovica0=@PoreznaOsnovica0,	PoreznaOsnovica5=@PoreznaOsnovica5, Pdv5=@Pdv5, PoreznaOsnovica10=@PoreznaOsnovica10, 
		Pdv10=@Pdv10, PoreznaOsnovica13=@PoreznaOsnovica13, Pdv13=@Pdv13, PoreznaOsnovica23=@PoreznaOsnovica23, Pdv23=@Pdv23, 
		PoreznaOsnovica25=@PoreznaOsnovica25, Pdv25=@Pdv25, UkupniPdv=@UkupniPdv, UkupnoUplaceno=@UkupnoUplaceno, 
		PreostaloZaUplatit=@PreostaloZaUplatit, NapomenaORacunu=@NapomenaORacunu, ZaprimljenUHzzo=@ZaprimljenUHzzo, 
		DanaOdZaprimanja=@DanaOdZaprimanja, DanaNeplacanja=@DanaNeplacanja, Knjizen=@Knjizen, TemeljnicaId=@TemeljnicaId
	WHERE Id=@Id;
END