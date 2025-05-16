CREATE PROCEDURE spGetAllUsers
AS
BEGIN
    SELECT UserID, FirstName, LastName, Email, Phone, Address, Role
    FROM [User]
    ORDER BY LastName, FirstName;
END
GO
