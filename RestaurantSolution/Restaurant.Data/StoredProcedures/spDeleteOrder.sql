CREATE PROCEDURE spDeleteOrder
    @OrderID INT
AS
BEGIN
    DELETE FROM [Order]
    WHERE OrderID = @OrderID;
END
GO