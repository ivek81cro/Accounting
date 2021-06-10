CREATE PROCEDURE [dbo].[spBalance_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT aj.Konto, ba.Opis, sum(Dugovna) as Dugovna, sum(Potrazna) as Potrazna, (sum(Dugovna) - sum(Potrazna)) as Stanje
	FROM AccountingJournal aj, BookAccount ba
	WHERE aj.Konto=ba.Konto
	GROUP BY aj.Konto, ba.Opis
	ORDER BY aj.Konto;
END
