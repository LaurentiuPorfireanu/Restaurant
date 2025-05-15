CREATE PROCEDURE spGetAllCategories
AS
BEGIN
    SELECT CategoryId, Name
    FROM Category
    ORDER BY Name;
END
GO
