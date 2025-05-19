CREATE PROCEDURE spDeleteAllMenuPreparate
    @MenuID INT
AS
BEGIN
    DELETE FROM MenuPreparat
    WHERE MenuID = @MenuID;
END
GO