CREATE PROCEDURE spGetPreparateByCategory
    @CategoryID INT
AS
BEGIN
    SELECT PreparatID,Denumire, Price, QuantityPortie, QuantityTotal
    FROM Preparat
    WHERE CategoryID = @CategoryID
    ORDER BY Denumire;
END
GO
