CREATE PROCEDURE spInsertOrder
    @OrderCode NVARCHAR(50),
    @UserID INT,
    @OrderDateTime DATETIME,
    @Status INT,
    @EstimatedDelivery DATETIME = NULL,
    @Discount DECIMAL(10,2),
    @DeliveryCost DECIMAL(10,2),
    @TotalCost DECIMAL(10,2)
AS
BEGIN
    INSERT INTO [Order] 
        (OrderCode, UserID, OrderDateTime, Status, EstimatedDelivery, Discount, DeliveryCost, TotalCost)
    VALUES
        (@OrderCode, @UserID, @OrderDateTime, @Status, @EstimatedDelivery, @Discount, @DeliveryCost, @TotalCost);
END
GO