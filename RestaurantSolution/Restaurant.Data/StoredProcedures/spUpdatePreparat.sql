CREATE PROCEDURE spUpdatePreparat
    @PreparatID INT,
    @Name NVARCHAR(200),
    @Price DECIMAL(10,2),
    @QuantityPortiee INT,
    @QuantityTotal  INT,
    @CategoryId INT
AS
BEGIN
    UPDATE Preparat
    SET
        Denumire = @Name,
        Price = @Price,
        QuantityPortie = @QuantityPortiee,
        QuantityTotal = @QuantityTotal,
        CategoryID = @CategoryId
    WHERE PreparatID = @PreparatID;
END
GO
