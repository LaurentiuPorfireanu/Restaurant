CREATE PROCEDURE spInsertUser
    @FirstName NVARCHAR(100),
    @LastName  NVARCHAR(100),
    @Email     NVARCHAR(200),
    @Phone     NVARCHAR(20),
    @Address   NVARCHAR(300),
    @PasswordHash NVARCHAR(500),
    @Role      INT
AS
BEGIN
    INSERT INTO [User]
       (FirstName, LastName, Email, Phone, Address, PasswordHash, Role)
    VALUES
       (@FirstName, @LastName, @Email, @Phone, @Address, @PasswordHash, @Role);
END
GO
