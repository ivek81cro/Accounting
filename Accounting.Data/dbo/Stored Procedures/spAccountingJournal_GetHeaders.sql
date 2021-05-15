CREATE PROCEDURE [dbo].[spAccountingJournal_GetHeaders]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT BrojTemeljnice, VrstaTemeljnice, Datum, sum(Dugovna) as Dugovna, sum(Potrazna) as Potrazna
	FROM AccountingJournal
	WHERE BrojTemeljnice = 0
	GROUP BY VrstaTemeljnice, BrojTemeljnice, Datum
END