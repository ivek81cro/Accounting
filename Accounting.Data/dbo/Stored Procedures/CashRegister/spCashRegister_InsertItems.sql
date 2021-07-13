CREATE PROCEDURE [dbo].[spCashRegister_InsertItems]
@Id INT,
@Datum datetime2,
@RedniBroj INT,
@Gotovina decimal(9,2),
@KreditneKartice decimal(9,2),
@Sveukupno decimal(9,2),
@IznosSudjelovanja decimal(9,2),
@Knjizen bit,
@TemeljnicaId INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO CashRegisterJournal (Datum, RedniBroj, Gotovina, KreditneKartice, Sveukupno, IznosSudjelovanja, Knjizen,
	TemeljnicaId)
	VALUES (@Datum, @RedniBroj, @Gotovina, @KreditneKartice, @Sveukupno, @IznosSudjelovanja, @Knjizen, @TemeljnicaId);
END