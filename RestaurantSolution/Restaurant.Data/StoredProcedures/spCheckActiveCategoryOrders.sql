CREATE PROCEDURE spCheckActiveCategoryOrders
    @CategoryId INT,
    @HasActiveOrders BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if any dish in this category is part of an active order
    IF EXISTS (
        SELECT 1
        FROM [Order] o
        INNER JOIN OrderDish od ON o.OrderID = od.OrderID
        INNER JOIN Preparat p ON od.PreparatID = p.PreparatID
        WHERE p.CategoryID = @CategoryId
        AND o.Status IN (1, 2, 4) -- Registered, InPreparation, OutforDelivery
    )
    OR EXISTS (
        SELECT 1
        FROM [Order] o
        INNER JOIN OrderMenu om ON o.OrderID = om.OrderID
        INNER JOIN Menu m ON om.MenuID = m.MenuID
        WHERE m.CategoryID = @CategoryId
        AND o.Status IN (1, 2, 4) -- Registered, InPreparation, OutforDelivery
    )
    BEGIN
        SET @HasActiveOrders = 1;
    END
    ELSE
    BEGIN
        SET @HasActiveOrders = 0;
    END
END
GO