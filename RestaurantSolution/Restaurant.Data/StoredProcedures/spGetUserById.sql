CREATE PROCEDURE spGetUserById
    @UserID INT
AS
BEGIN
    SELECT UserID, FirstName, LastName, Email, Phone, Address, Role
    FROM [User]
    WHERE UserID = @UserID;
END
GO
