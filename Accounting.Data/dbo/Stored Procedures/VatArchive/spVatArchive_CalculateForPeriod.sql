CREATE PROCEDURE [dbo].[spVatArchive_CalculateForPeriod]
@DateFrom DATETIME2,
@DateTo DATETIME2
AS
BEGIN
	SET NOCOUNT ON;

	DROP TABLE IF EXISTS #VatIraTemp, #VatUraTemp;

	SELECT	SUM(PoreznaOsnovica5) AS UraOsnovica5,
			SUM(PoreznaOsnovica10) AS UraOsnovica10,
			SUM(PoreznaOsnovica13) AS UraOsnovica13,
			SUM(PoreznaOsnovica25) AS UraOsnovica25,
			SUM(PoreznaOsnovica0) AS UraOsnovica0,
			SUM(PretporezT5) AS PretporezT5,
			SUM(PretporezT10) AS PretporezT10,
			SUM(PretporezT13) AS PretporezT13,
			SUM(PretporezT25) AS PretporezT25
	INTO #VatUraTemp
	FROM BookUraRest
	WHERE Datum BETWEEN @DateFrom AND @DateTo;

	SELECT	SUM(PoreznaOsnovica5) AS IraOsnovica5,
			SUM(PoreznaOsnovica10) AS IraOsnovica10,
			SUM(PoreznaOsnovica13) AS IraOsnovica13,
			SUM(PoreznaOsnovica25) AS IraOsnovica25,
			SUM(PoreznaOsnovica0) AS IraOsnovica0,
			SUM(Pdv5) AS Pdv5,
			SUM(Pdv10) AS Pdv10,
			SUM(Pdv13) AS Pdv13,
			SUM(Pdv25) AS Pdv25
	INTO #VatIraTemp
	FROM BookIra
	WHERE Datum BETWEEN @DateFrom AND @DateTo;

	SELECT UraOsnovica0, UraOsnovica5, UraOsnovica10, UraOsnovica13, UraOsnovica25,
			PretporezT5, PretporezT10, PretporezT13, PretporezT25,
			IraOsnovica0, IraOsnovica5, IraOsnovica10, IraOsnovica13, IraOsnovica25,
			Pdv5, Pdv10, Pdv13, Pdv25, @DateFrom AS DateFrom, @DateTo AS DateTo
	FROM #VatIraTemp, #VatUraTemp;

END