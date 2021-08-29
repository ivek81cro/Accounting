CREATE PROCEDURE [dbo].[spLocoTravelCalc_Insert]
@Id INT,
@EmployeeName NVARCHAR(255),
@EmployeeOib NVARCHAR(255),
@VehicleMake NVARCHAR(255),
@VehicleRegistration NVARCHAR(255),
@DateOfCalculation DATETIME2(7),
@DateOfPayment DATETIME2(7),
@TotalCost DECIMAL(9,2),
@Processed BIT
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS (SELECT * FROM LocoCalculation WHERE Id = @Id)
		BEGIN
			UPDATE LocoCalculation SET EmployeeName=@EmployeeName, EmployeeOib=@EmployeeOib, VehicleMake=@VehicleMake, 
				VehicleRegistration=@VehicleRegistration, DateOfCalculation=@DateOfCalculation, DateOfPayment=@DateOfPayment, 
				TotalCost=@TotalCost, Processed=@Processed
			WHERE Id=@Id;

			DELETE FROM LocoOrder WHERE CalculationId = @Id;
		END
	ELSE
		BEGIN
			INSERT INTO LocoCalculation (EmployeeName, EmployeeOib, VehicleMake, VehicleRegistration, DateOfCalculation, DateOfPayment, TotalCost, Processed)
			VALUES (@EmployeeName, @EmployeeOib, @VehicleMake, @VehicleRegistration, @DateOfCalculation, @DateOfPayment, @TotalCost, @Processed);
		END

	SELECT TOP 1 Id FROM LocoCalculation ORDER BY Id DESC;
END
