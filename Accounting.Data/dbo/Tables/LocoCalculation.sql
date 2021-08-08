CREATE TABLE [dbo].[LocoCalculation]
(
	[Id] INT NOT NULL IDENTITY,
    [EmployeeId] INT NOT NULL,
    [VehicleMake] NVARCHAR(255) NOT NULL,
    [VehicleRegistration] NVARCHAR(255) NOT NULL,
    [DateOfCalculation] DATETIME2(7) NOT NULL,
    [DateOfPayment] DATETIME2(7) NOT NULL,
    [TotalCost] DECIMAL(9,2) NOT NULL
)
