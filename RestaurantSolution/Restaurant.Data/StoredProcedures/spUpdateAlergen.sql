CREATE PROCEDURE spUpdateAlergen
    @AlergenID INT,
    @Name NVARCHAR(100)
AS
BEGIN
    UPDATE Alergen
    SET Name = @Name
    WHERE AlergenID = @AlergenID;
END
GO
