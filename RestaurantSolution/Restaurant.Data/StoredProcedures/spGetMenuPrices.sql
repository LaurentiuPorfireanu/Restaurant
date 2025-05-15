CREATE PROCEDURE spGetMenuPrices
AS
BEGIN
    SELECT 
        m.Name AS MenuName,
        (
            SELECT SUM(mp.QuantityMenuPortie * p.Price)
            FROM MenuPreparat mp
            JOIN Preparat p ON mp.PreparatID = p.PreparatID
            WHERE mp.MenuID = m.MenuID
        ) AS CalculatedPrice
    FROM Menu m;
END
GO
