CREATE PROCEDURE spGetAllOrders
AS
BEGIN
    SELECT 
        OrderID, OrderCode, UserID, OrderDateTime, Status, EstimatedDelivery,
        Discount, DeliveryCost, TotalCost
    FROM [Order]
    ORDER BY OrderDateTime DESC;
END
GO