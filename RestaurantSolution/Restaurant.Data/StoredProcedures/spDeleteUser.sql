﻿CREATE PROCEDURE spDeleteUser
    @UserID INT
AS
BEGIN
    DELETE FROM [User]
    WHERE UserID = @UserID;
END
GO
