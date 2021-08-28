CREATE PROCEDURE [dbo].[spLocoOrders_Delete]
@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM LocoCalculation WHERE Id=@Id;

	DELETE FROM LocoOrder WHERE CalculationId=@Id;
END