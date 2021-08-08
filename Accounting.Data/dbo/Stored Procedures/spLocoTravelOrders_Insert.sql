CREATE PROCEDURE [dbo].[spLocoTravelOrders_Insert]
@Id INT,
@Date DATETIME2,
@StartingKm INT,
@FinishKm INT,
@Destination NVARCHAR(150),
@TotalDistance INT,
@Description NVARCHAR(255),
@CalculationId INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO LocoOrder (Date, StartingKm, FinishKm, Destination, TotalDistance, Description, CalculationId)
	VALUES (@Date, @StartingKm, @FinishKm, @Destination, @TotalDistance, @Description, @CalculationId);
END
