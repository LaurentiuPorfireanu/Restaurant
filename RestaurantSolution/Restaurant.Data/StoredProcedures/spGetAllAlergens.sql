CREATE PROCEDURE spGetAllAlergens
AS
BEGIN
    SELECT AlergenID, Name
    FROM Alergen
    ORDER BY Name;
END
GO
