CREATE PROCEDURE spGetActiveOrders
AS
BEGIN
    SELECT 
        OrderID, OrderCode, UserID, OrderDateTime, Status, EstimatedDelivery,
        Discount, DeliveryCost, TotalCost
    FROM [Order]
    WHERE Status NOT IN (8, 16)
    ORDER BY OrderDateTime DESC;
END
GO