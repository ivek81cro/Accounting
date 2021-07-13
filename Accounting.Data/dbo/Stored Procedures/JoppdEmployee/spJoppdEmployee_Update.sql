CREATE PROCEDURE [dbo].[spJoppdEmployee_Update]
@Oib NVARCHAR(11),
@SifraPrebivalista NVARCHAR(10),
@SifraOpcineRada NVARCHAR(10),
@OznakaStjecatelja NVARCHAR(10),
@OznakaPrimitka NVARCHAR(10),
@DodatniMio NVARCHAR(10),
@ObvezaInvaliditet NVARCHAR(10),
@PrviZadnjiMjesec NVARCHAR(10),
@PunoNepunoRadnoVrijeme NVARCHAR(10),
@NacinIsplate NVARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE JoppdEmployee
	SET SifraPrebivalista=@SifraPrebivalista, SifraOpcineRada=@SifraOpcineRada, OznakaStjecatelja=@OznakaStjecatelja, OznakaPrimitka=@OznakaPrimitka,
	DodatniMio=@DodatniMio, ObvezaInvaliditet=@ObvezaInvaliditet, PrviZadnjiMjesec=@PrviZadnjiMjesec, PunoNepunoRadnoVrijeme=@PunoNepunoRadnoVrijeme,
	NacinIsplate=@NacinIsplate
	WHERE Oib=@Oib;
END
