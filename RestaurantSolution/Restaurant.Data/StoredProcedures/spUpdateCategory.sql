CREATE PROCEDURE spUpdateCategory
    @CategoryId INT,
    @Name NVARCHAR(100)
AS
BEGIN
    UPDATE Category
    SET Name = @Name
    WHERE CategoryId = @CategoryId;
END
GO
