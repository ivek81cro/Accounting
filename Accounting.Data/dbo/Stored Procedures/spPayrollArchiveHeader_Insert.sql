CREATE PROCEDURE [dbo].[spPayrollArchiveHeader_Insert]
@Id INT,
@UniqueId nvarchar(255),
@Opis nvarchar(255),
@DatumOd datetime2,
@DatumDo datetime2,
@SatiRada int,
@SatiPraznika int,
@DatumObracuna datetime2
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO PayrollArchiveHeader (Opis, UniqueId, DatumOd, DatumDo, SatiRada, SatiPraznika, DatumObracuna) 
	VALUES(@Opis, @UniqueId, @DatumOd, @DatumDo, @SatiRada, @SatiPraznika, @DatumObracuna);
END
