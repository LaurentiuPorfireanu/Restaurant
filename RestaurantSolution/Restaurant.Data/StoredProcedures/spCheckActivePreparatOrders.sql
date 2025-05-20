CREATE PROCEDURE spCheckActivePreparatOrders
    @PreparatID INT,
    @HasActiveOrders BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if this preparat is part of an active order
    IF EXISTS (
        SELECT 1
        FROM [Order] o
        INNER JOIN OrderDish od ON o.OrderID = od.OrderID
        WHERE od.PreparatID = @PreparatID
        AND o.Status IN (1, 2, 4) -- Registered, InPreparation, OutforDelivery
    )
    OR EXISTS (
        SELECT 1
        FROM [Order] o
        INNER JOIN OrderMenu om ON o.OrderID = om.OrderID
        INNER JOIN Menu m ON om.MenuID = m.MenuID
        INNER JOIN MenuPreparat mp ON m.MenuID = mp.MenuID
        WHERE mp.PreparatID = @PreparatID
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