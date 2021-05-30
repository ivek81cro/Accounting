CREATE PROCEDURE [dbo].[spAccountingJournal_GetAccountBalance]
@Konto NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Opis, Dokument, Datum, Dugovna, Potrazna 
	FROM AccountingJournal 
	WHERE Konto=@Konto 
	ORDER BY Datum;
END
