CREATE TABLE [dbo].[LocoOrder]
(
	[Id] INT NOT NULL IDENTITY, 
    [Date] DATETIME2 NOT NULL,
	[StartingKm] INT NOT NULL,
    [FinishKm] INT NOT NULL,
    [Destination] NVARCHAR(150) NOT NULL,
    [TotalDistance] INT NOT NULL,
    [Description] NVARCHAR(255) NULL,
    [CalculationId] INT NOT NULL
)
