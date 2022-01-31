CREATE PROCEDURE [dbo].[spLocoTravelOrders_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, EmployeeName, EmployeeOib, VehicleMake, VehicleRegistration, DateOfCalculation, DateOfPayment, TotalCost, Processed
	FROM LocoCalculation;
END