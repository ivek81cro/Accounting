CREATE TABLE [dbo].[LocoCalculation]
(
	[Id] INT NOT NULL IDENTITY,
    [EmployeeName] NVARCHAR(255) NOT NULL,
    [EmployeeOib] NVARCHAR(11) NOT NULL,
    [VehicleMake] NVARCHAR(255) NOT NULL,
    [VehicleRegistration] NVARCHAR(255) NOT NULL,
    [DateOfCalculation] DATETIME2(7) NOT NULL,
    [DateOfPayment] DATETIME2(7) NOT NULL,
    [TotalCost] DECIMAL(9,2) NOT NULL, 
    [Processed] BIT NOT NULL
)
