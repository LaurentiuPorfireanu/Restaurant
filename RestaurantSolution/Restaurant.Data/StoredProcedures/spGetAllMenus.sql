CREATE PROCEDURE spGetAllMenus
AS
BEGIN
    SELECT MenuID, Name, CategoryID
    FROM Menu
    ORDER BY Name;
END
GO