CREATE PROCEDURE [dbo].[spPayrollSupplementEmployee_Insert]
@Id int,
@Oib nvarchar(11),
@Naziv nvarchar(max),
@Iznos decimal(8,2),
@Sifra nvarchar(10)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Count int;

	SET @Count = (SELECT COUNT(1)
				  FROM PayrollSupplementEmployee
				  WHERE Oib =@Oib AND Sifra=@Sifra);

	IF @Count > 0
		UPDATE PayrollSupplementEmployee 
		SET Iznos=@Iznos 
		WHERE Sifra=@Sifra AND Oib=@Oib;
	ELSE
		INSERT INTO PayrollSupplementEmployee (Oib, Naziv, Iznos, Sifra)
		VALUES (@Oib, @Naziv, @Iznos, @Sifra);
END