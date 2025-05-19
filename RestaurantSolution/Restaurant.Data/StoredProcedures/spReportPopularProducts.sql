CREATE PROCEDURE spReportPopularProducts
    @TopCount INT = 20
AS
BEGIN
    SELECT TOP (@TopCount)
        p.Name AS ProductName,
        COUNT(od.OrderID) AS OrderCount,
        SUM(od.Quantity) AS TotalQuantity,
        SUM(od.Quantity * od.UnitPrice) AS Revenue,
        FORMAT(SUM(od.Quantity * od.UnitPrice), 'N2') + ' Lei' AS RevenueFormatted
    FROM
        OrderDish od
        INNER JOIN Preparat p ON od.PreparatID = p.PreparatID
        INNER JOIN [Order] o ON od.OrderID = o.OrderID
    WHERE
        o.Status <> 16 -- Exclude canceled orders
    GROUP BY
        p.Name
    ORDER BY
        OrderCount DESC, TotalQuantity DESC
END
GO