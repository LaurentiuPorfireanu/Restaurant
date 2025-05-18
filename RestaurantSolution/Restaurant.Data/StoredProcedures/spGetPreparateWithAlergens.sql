CREATE PROCEDURE spGetPreparateWithAlergens
AS
BEGIN
    SELECT 
        p.Name   AS Preparat,
        a.Name       AS Alergen
    FROM PreparatAlergen pa
    JOIN Preparat p   ON pa.PreparatID = p.PreparatID
    JOIN Alergen a    ON pa.AlergenID   = a.AlergenID
    ORDER BY p.Name;
END
GO
