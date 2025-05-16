CREATE PROCEDURE spUpdateUser
    @UserID    INT,
    @FirstName NVARCHAR(100),
    @LastName  NVARCHAR(100),
    @Email     NVARCHAR(200),
    @Phone     NVARCHAR(20),
    @Address   NVARCHAR(300),
    @Role      INT
AS
BEGIN
    UPDATE [User]
    SET FirstName   = @FirstName,
        LastName    = @LastName,
        Email       = @Email,
        Phone       = @Phone,
        Address     = @Address,
        Role        = @Role
    WHERE UserID = @UserID;
END
GO
