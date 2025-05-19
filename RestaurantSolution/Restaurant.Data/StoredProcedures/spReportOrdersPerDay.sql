CREATE PROCEDURE spReportOrdersPerDay
    @StartDate DATE = NULL,
    @EndDate DATE = NULL
AS
BEGIN
    -- Set default dates if not provided
    IF @StartDate IS NULL
        SET @StartDate = DATEADD(MONTH, -1, GETDATE()) -- Last month by default
    
    IF @EndDate IS NULL
        SET @EndDate = GETDATE() -- Current date by default

    SELECT
        CAST(o.OrderDateTime AS DATE) AS [Date],
        FORMAT(o.OrderDateTime, 'dd.MM.yyyy') AS DateFormatted,
        COUNT(*) AS OrderCount,
        SUM(o.TotalCost) AS TotalValue,
        FORMAT(SUM(o.TotalCost), 'N2') + ' Lei' AS TotalValueFormatted
    FROM
        [Order] o
    WHERE
        CAST(o.OrderDateTime AS DATE) BETWEEN @StartDate AND @EndDate
    GROUP BY
        CAST(o.OrderDateTime AS DATE), FORMAT(o.OrderDateTime, 'dd.MM.yyyy')
    ORDER BY
        CAST(o.OrderDateTime AS DATE) DESC
END
GO