CREATE PROCEDURE spDeleteCategory
    @CategoryId INT
AS
BEGIN
    DELETE FROM Category
    WHERE CategoryId = @CategoryId;
END
GO
