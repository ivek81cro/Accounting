CREATE PROCEDURE [dbo].[spPayroll_Update]
@Oib NVARCHAR(11),
@Ime NVARCHAR(75),
@Prezime NVARCHAR(75),
@Bruto DECIMAL(8,2),
@Mio1 DECIMAL(8,2),
@Mio2 DECIMAL(8,2),
@Dohodak DECIMAL(8,2),
@Odbitak DECIMAL(8,2),
@PoreznaOsnovica DECIMAL(8,2),
@PoreznaStopa1 DECIMAL(8,2),
@PoreznaStopa2 DECIMAL(8,2),
@UkupnoPorez DECIMAL(8,2),
@Prirez DECIMAL(8,2),
@UkupnoPorezPrirez DECIMAL(8,2),
@Neto DECIMAL(8,2),
@DoprinosZdravstvo DECIMAL(8,2),
@SamoPrviStupMio BIT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Payroll SET Ime=@Ime, Prezime=@Prezime, Bruto=@Bruto, Mio1=@Mio1, Mio2=@Mio2, Dohodak=@Dohodak, Odbitak=@Odbitak, PoreznaOsnovica=@PoreznaOsnovica, 
	PoreznaStopa1=@PoreznaStopa1, PoreznaStopa2=@PoreznaStopa2, UkupnoPorez=@UkupnoPorez, Prirez=@Prirez, UkupnoPorezPrirez=@UkupnoPorezPrirez, Neto=@Neto, 
	DoprinosZdravstvo=@DoprinosZdravstvo, SamoPrviStupMio=@SamoPrviStupMio
	WHERE Oib=@Oib;
END