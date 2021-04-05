CREATE PROCEDURE [dbo].[spPayrollArchiveSupplement_Insert]
@Id int,
@Oib nvarchar(11),
@Naziv nvarchar(max),
@Iznos decimal(8,2),
@Sifra nvarchar(10),
@AccountingId int
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO PayrollArchiveSupplement (Oib, Naziv, Iznos, Sifra, AccountingId)
	VALUES (@Oib, @Naziv, @Iznos, @Sifra, @AccountingId);
END
