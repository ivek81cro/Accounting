CREATE PROCEDURE [dbo].[spLocoTravelOrders_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, EmployeeId, VehicleMake, VehicleRegistration, DateOfCalculation, DateOfPayment, TotalCost, Processed
	FROM LocoCalculation;
END