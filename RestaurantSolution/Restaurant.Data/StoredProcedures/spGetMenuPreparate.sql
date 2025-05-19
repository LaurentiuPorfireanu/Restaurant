CREATE PROCEDURE spGetMenuPreparate
    @MenuID INT
AS
BEGIN
    SELECT mp.MenuID, mp.PreparatID, mp.QuantityMenuPortie,
           p.Name AS PreparatName, p.Price, p.QuantityPortie
    FROM MenuPreparat mp
    JOIN Preparat p ON mp.PreparatID = p.PreparatID
    WHERE mp.MenuID = @MenuID;
END
GO