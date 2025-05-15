CREATE PROCEDURE spInsertPreparat
    @Name NVARCHAR(200),
    @Price DECIMAL(10,2),
    @QuantityPortie INT,
    @QuantityTotal INT,
    @CategoryId INT
AS
BEGIN
    INSERT INTO Preparat
        (Denumire, Price, QuantityPortie, QuantityTotal, CategoryID)
    VALUES
        (@Name, @Price, @QuantityPortie, @QuantityTotal, @CategoryId);
END
GO
