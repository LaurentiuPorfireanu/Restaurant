CREATE PROCEDURE spUpdateOrderStatus
    @OrderID INT,
    @Status INT
AS
BEGIN
    UPDATE [Order]
    SET Status = @Status
    WHERE OrderID = @OrderID;
END
GO