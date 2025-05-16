CREATE PROCEDURE spGetMenuById
    @MenuID INT
AS
BEGIN
    SELECT MenuID, Name, CategoryID
    FROM Menu
    WHERE MenuID = @MenuID;
END
GO