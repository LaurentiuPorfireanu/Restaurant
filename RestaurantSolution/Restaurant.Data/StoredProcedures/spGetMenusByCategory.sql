CREATE PROCEDURE spGetMenusByCategory
    @CategoryID INT
AS
BEGIN
    SELECT MenuID, Name, CategoryID
    FROM Menu
    WHERE CategoryID = @CategoryID
    ORDER BY Name;
END
GO