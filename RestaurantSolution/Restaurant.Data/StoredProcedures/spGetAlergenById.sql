CREATE PROCEDURE spGetAlergenById
    @AlergenID INT
AS
BEGIN
    SELECT AlergenID, Name
    FROM Alergen
    WHERE AlergenID = @AlergenID;
END
GO
