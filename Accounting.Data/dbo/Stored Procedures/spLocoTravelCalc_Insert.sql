CREATE PROCEDURE [dbo].[spLocoTravelCalc_Insert]
@Id INT,
@EmployeeId INT,
@VehicleMake NVARCHAR(255),
@VehicleRegistration NVARCHAR(255),
@DateOfCalculation DATETIME2(7),
@DateOfPayment DATETIME2(7),
@TotalCost DECIMAL(9,2)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO LocoCalculation (EmployeeId, VehicleMake, VehicleRegistration, DateOfCalculation, DateOfPayment, TotalCost)
	VALUES (@EmployeeId, @VehicleMake, @VehicleRegistration, @DateOfCalculation, @DateOfPayment, @TotalCost);

	SELECT TOP 1 Id FROM LocoCalculation ORDER BY Id DESC;
END
