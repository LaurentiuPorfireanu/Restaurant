CREATE PROCEDURE spReportLowStockProducts
    @LowStockThreshold INT = 5
AS
BEGIN
    SELECT
        p.PreparatID,
        p.Name AS ProductName,
        p.QuantityTotal AS CurrentStock,
        c.Name AS Category,
        CASE
            WHEN p.QuantityTotal = 0 THEN 'Epuizat'
            ELSE 'Nivel scăzut'
        END AS Status
    FROM
        Preparat p
        INNER JOIN Category c ON p.CategoryID = c.CategoryId
    WHERE
        p.QuantityTotal <= @LowStockThreshold
    ORDER BY
        p.QuantityTotal ASC, p.Denumire
END
GO