CREATE OR ALTER PROCEDURE spGetMenuById
    @MenuID INT
AS
BEGIN
    SELECT m.MenuID, m.Name, m.CategoryID, c.Name AS CategoryName
    FROM Menu m
    LEFT JOIN Category c ON m.CategoryID = c.CategoryId
    WHERE m.MenuID = @MenuID;
END
GO