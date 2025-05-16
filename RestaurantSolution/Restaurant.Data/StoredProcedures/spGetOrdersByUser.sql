CREATE PROCEDURE spGetOrdersByUser
    @UserID INT
AS
BEGIN
    SELECT 
        OrderID, OrderCode, UserID, OrderDateTime, Status, EstimatedDelivery,
        Discount, DeliveryCost, TotalCost
    FROM [Order]
    WHERE UserID = @UserID
    ORDER BY OrderDateTime DESC;
END
GO