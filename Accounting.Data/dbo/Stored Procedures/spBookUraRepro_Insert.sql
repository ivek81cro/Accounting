CREATE PROCEDURE [dbo].[spBookUraRepro_Insert]
@Id INT ,
@DatumKnjizenja datetime2,
@BrojPrimke int,
@Storno bit,
@NazivDobavljaca nvarchar(255),
@BrojRacuna nvarchar(255),
@DatumRacuna datetime2,
@Otpremnica bit,
@DospijecePlacanja datetime2,
@FakturnaVrijednost decimal(9,2),
@NabavnaVrijednost decimal(9,2),
@Rabat decimal(9,2),
@NettoNabavnaVrijednost decimal(9,2),
@Pretporez decimal(9,2),
@VeleprodajniRabat decimal(9,2),
@CassaSconto decimal(9,2),
@PorezniBroj nvarchar(50),
@BrojUKnjiziUra int,
@Knjizen bit,
@TemeljnicaId INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO BookUraRepro (DatumKnjizenja, BrojPrimke, Storno,  NazivDobavljaca, BrojRacuna,
	DatumRacuna, Otpremnica, DospijecePlacanja, FakturnaVrijednost, NabavnaVrijednost, VeleprodajniRabat,
	Rabat, NettoNabavnaVrijednost, Pretporez, CassaSconto, PorezniBroj, BrojUKnjiziUra, Knjizen, TemeljnicaId)
	VALUES (@DatumKnjizenja, @BrojPrimke, @Storno,  @NazivDobavljaca, @BrojRacuna,
	@DatumRacuna, @Otpremnica, @DospijecePlacanja, @FakturnaVrijednost, @NabavnaVrijednost, @VeleprodajniRabat,
	@Rabat, @NettoNabavnaVrijednost, @Pretporez, @CassaSconto, @PorezniBroj, @BrojUKnjiziUra, @Knjizen, @TemeljnicaId);

END
