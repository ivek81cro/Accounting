CREATE PROCEDURE [dbo].[spBalance_GetForPeriod]
@DateFrom datetime2,
@DateTo datetime2
AS
BEGIN
	SET NOCOUNT ON;
	SELECT aj.Konto, ba.Opis, sum(Dugovna) as Dugovna, sum(Potrazna) as Potrazna, (sum(Dugovna) - sum(Potrazna)) as Stanje
	FROM AccountingJournal aj, BookAccount ba
	WHERE aj.Konto=ba.Konto AND aj.DatumKnjizenja BETWEEN @DateFrom AND @DateTo
	GROUP BY aj.Konto, ba.Opis
	ORDER BY aj.Konto;
END