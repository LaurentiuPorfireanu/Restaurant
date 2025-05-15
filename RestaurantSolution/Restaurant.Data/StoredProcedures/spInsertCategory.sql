CREATE PROCEDURE spInsertCategory
    @Name NVARCHAR(100)
AS
BEGIN
    INSERT INTO Category (Name)
    VALUES (@Name);
END
GO
