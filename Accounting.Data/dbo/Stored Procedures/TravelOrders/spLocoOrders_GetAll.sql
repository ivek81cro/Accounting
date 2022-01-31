CREATE PROCEDURE [dbo].[spLocoOrders_GetAll]
@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Date, StartingKm, FinishKm, Destination, TotalDistance, Description, CalculationId
	FROM LocoOrder
	WHERE CalculationId=@Id;
END