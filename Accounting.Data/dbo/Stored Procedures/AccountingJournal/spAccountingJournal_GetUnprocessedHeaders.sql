CREATE PROCEDURE [dbo].[spAccountingJournal_GetUnprocessedHeaders]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT BrojTemeljnice, VrstaTemeljnice, sum(Dugovna) as Dugovna, sum(Potrazna) as Potrazna
	FROM AccountingJournal
	WHERE BrojTemeljnice = 0
	GROUP BY VrstaTemeljnice, BrojTemeljnice
END