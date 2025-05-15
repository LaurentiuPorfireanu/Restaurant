CREATE PROCEDURE spGetCategoryById
    @CategoryId INT
AS
BEGIN
    SELECT CategoryId, Name
    FROM Category
    WHERE CategoryId = @CategoryId;
END
GO
