CREATE PROCEDURE spDeleteAlergen
    @AlergenID INT
AS
BEGIN
    DELETE FROM Alergen
    WHERE AlergenID = @AlergenID;
END
GO
