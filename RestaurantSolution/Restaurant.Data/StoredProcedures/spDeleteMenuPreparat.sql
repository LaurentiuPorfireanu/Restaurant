CREATE PROCEDURE spDeleteMenuPreparat
    @MenuID INT,
    @PreparatID INT
AS
BEGIN
    DELETE FROM MenuPreparat
    WHERE MenuID = @MenuID AND PreparatID = @PreparatID;
END
GO