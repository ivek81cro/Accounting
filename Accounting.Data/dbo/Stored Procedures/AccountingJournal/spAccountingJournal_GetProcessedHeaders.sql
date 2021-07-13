CREATE PROCEDURE [dbo].[spAccountingJournal_GetProcessedHeaders]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT BrojTemeljnice, VrstaTemeljnice, sum(Dugovna) as Dugovna, sum(Potrazna) as Potrazna, DatumKnjizenja
	FROM AccountingJournal
	WHERE BrojTemeljnice <> 0
	GROUP BY VrstaTemeljnice, BrojTemeljnice, DatumKnjizenja
	ORDER BY BrojTemeljnice
END