CREATE PROCEDURE spGetAllMenusWithCategories
AS
BEGIN
    SELECT 
        m.MenuID, 
        m.Name AS MenuName, 
        m.CategoryID,
        c.Name AS CategoryName,
        c.CategoryId
    FROM Menu m
    LEFT JOIN Category c ON m.CategoryID = c.CategoryId
    ORDER BY m.Name;
END
GO