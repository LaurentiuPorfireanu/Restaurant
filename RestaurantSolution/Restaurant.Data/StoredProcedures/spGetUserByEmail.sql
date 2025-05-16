CREATE PROCEDURE spGetUserByEmail
    @Email NVARCHAR(200)
AS
BEGIN
    SELECT UserID, FirstName, LastName, Email, Phone, Address, Role, PasswordHash
    FROM [User]
    WHERE Email = @Email;
END
GO
