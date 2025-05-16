CREATE PROCEDURE spInsertMenu
    @Name NVARCHAR(200),
    @CategoryID INT
AS
BEGIN
    INSERT INTO Menu (Name, CategoryID)
    VALUES (@Name, @CategoryID);
END
GO