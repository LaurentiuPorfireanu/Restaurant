CREATE PROCEDURE spUpdateMenu
    @MenuID INT,
    @Name NVARCHAR(200),
    @CategoryID INT
AS
BEGIN
    UPDATE Menu
    SET Name = @Name,
        CategoryID = @CategoryID
    WHERE MenuID = @MenuID;
END
GO