CREATE PROCEDURE spInsertAlergen
    @Name NVARCHAR(100)
AS
BEGIN
    INSERT INTO Alergen (Name)
    VALUES (@Name);
END
GO
