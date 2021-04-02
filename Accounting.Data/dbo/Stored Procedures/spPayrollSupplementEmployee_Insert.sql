CREATE PROCEDURE [dbo].[spPayrollSupplementEmployee_Insert]
@Id int,
@Oib nvarchar(11),
@Naziv nvarchar(max),
@Iznos decimal(8,2),
@Sifra nvarchar(10)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO PayrollSupplementEmployee (Oib, Naziv, Iznos, Sifra)
	VALUES (@Oib, @Naziv, @Iznos, @Sifra);
END