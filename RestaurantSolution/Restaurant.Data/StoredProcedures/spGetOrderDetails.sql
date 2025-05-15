CREATE PROCEDURE spGetOrderDetails
    @OrderID INT
AS
BEGIN
    SELECT 
        o.OrderCode,
        u.FirstName + ' ' + u.LastName AS Client,
        o.OrderDateTime,
        od.DishCount,
        od.TotalDishesCost,
        o.DeliveryCost,
        o.Discount,
        (od.TotalDishesCost + o.DeliveryCost - o.Discount) AS TotalCost
    FROM [Order] o
    JOIN [User] u ON o.UserID = u.UserID
    CROSS APPLY (
        SELECT 
            COUNT(*)                     AS DishCount,
            SUM(d.Quantity * d.UnitPrice) AS TotalDishesCost
        FROM OrderDish d
        WHERE d.OrderID = o.OrderID
    ) od
    WHERE o.OrderID = @OrderID;
END
GO
