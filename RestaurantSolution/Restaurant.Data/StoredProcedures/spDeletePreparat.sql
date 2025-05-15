CREATE PROCEDURE spDeletePreparat
    @PreparatID INT
AS
BEGIN
    DELETE FROM Preparat
    WHERE PreparatID = @PreparatID;
END
GO
