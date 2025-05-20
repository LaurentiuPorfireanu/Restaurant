CREATE PROCEDURE spDeleteCategoryWithCascade
    @CategoryId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Start a transaction
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Store PreparatIDs for this category in a temp table
        DECLARE @PreparatesToDelete TABLE (PreparatID INT);
        INSERT INTO @PreparatesToDelete
        SELECT PreparatID FROM Preparat WHERE CategoryID = @CategoryId;
        
        -- Store MenuIDs for this category in a temp table
        DECLARE @MenusToDelete TABLE (MenuID INT);
        INSERT INTO @MenusToDelete
        SELECT MenuID FROM Menu WHERE CategoryID = @CategoryId;
        
        -- Delete Preparat Allergens
        DELETE FROM PreparatAlergen
        WHERE PreparatID IN (SELECT PreparatID FROM @PreparatesToDelete);
        
        -- Delete Preparat Photos
        DELETE FROM PreparatFoto
        WHERE PreparatID IN (SELECT PreparatID FROM @PreparatesToDelete);
        
        -- Delete MenuPreparat relationships for the preparate
        DELETE FROM MenuPreparat
        WHERE PreparatID IN (SELECT PreparatID FROM @PreparatesToDelete);
        
        -- Delete OrderDishes that reference the preparate
        DELETE FROM OrderDish
        WHERE PreparatID IN (SELECT PreparatID FROM @PreparatesToDelete);
        
        -- Delete MenuPreparat relationships for the menus
        DELETE FROM MenuPreparat
        WHERE MenuID IN (SELECT MenuID FROM @MenusToDelete);
        
        -- Delete OrderMenus that reference the menus
        DELETE FROM OrderMenu
        WHERE MenuID IN (SELECT MenuID FROM @MenusToDelete);
        
        -- Delete the Preparate
        DELETE FROM Preparat
        WHERE CategoryID = @CategoryId;
        
        -- Delete the Menus
        DELETE FROM Menu
        WHERE CategoryID = @CategoryId;
        
        -- Finally, delete the Category
        DELETE FROM Category
        WHERE CategoryId = @CategoryId;
        
        -- Commit the transaction
        COMMIT TRANSACTION;
        
        RETURN 0; -- Success
    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of error
        ROLLBACK TRANSACTION;
        
        -- Return error info
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
        
        RETURN -1; -- Error
    END CATCH
END
GO