CREATE PROCEDURE [dbo].[spBookUraPrimka_Insert]
@Id INT ,
@DatumKnjizenja datetime2,
@BrojPrimke int,
@Storno bit, 
@MaloprodajnaVrijednost decimal(9,2),
@NazivDobavljaca nvarchar(255),
@BrojRacuna nvarchar(255),
@DatumRacuna datetime2,
@Otpremnica bit,
@DospijecePlacanja datetime2,
@FakturnaVrijednost decimal(9,2),
@MaloprodajnaMarza decimal(9,2),
@IznosPdv decimal(9,2),
@VrijednostBezPoreza decimal(9,2),
@NabavnaVrijednost decimal(9,2),
@MaloprodajniRabat decimal(9,2),
@NettoNabavnaVrijednost decimal(9,2),
@Pretporez decimal(9,2),
@VeleprodajniRabat decimal(9,2),
@CassaSconto decimal(9,2),
@NettoRuc decimal(9,2),
@PovratnaNaknada decimal(9,2),
@PorezniBroj nvarchar(50),
@BrojUKnjiziUra int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO BookUraPrimka (DatumKnjizenja, BrojPrimke, Storno, MaloprodajnaVrijednost, NazivDobavljaca, BrojRacuna,
	DatumRacuna, Otpremnica, DospijecePlacanja, FakturnaVrijednost, MaloprodajnaMarza, IznosPdv, VrijednostBezPoreza, NabavnaVrijednost,
	MaloprodajniRabat, NettoNabavnaVrijednost, Pretporez, VeleprodajniRabat, CassaSconto, NettoRuc, PovratnaNaknada, PorezniBroj, BrojUKnjiziUra)
	VALUES (@DatumKnjizenja, @BrojPrimke, @Storno, @MaloprodajnaVrijednost, @NazivDobavljaca, @BrojRacuna,
	@DatumRacuna, @Otpremnica, @DospijecePlacanja, @FakturnaVrijednost, @MaloprodajnaMarza, @IznosPdv, @VrijednostBezPoreza, @NabavnaVrijednost,
	@MaloprodajniRabat, @NettoNabavnaVrijednost, @Pretporez, @VeleprodajniRabat, @CassaSconto, @NettoRuc, @PovratnaNaknada, @PorezniBroj, @BrojUKnjiziUra);

END