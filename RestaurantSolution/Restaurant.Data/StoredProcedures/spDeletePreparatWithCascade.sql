CREATE PROCEDURE spDeletePreparatWithCascade
    @PreparatID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Start a transaction
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Delete Preparat Allergens
        DELETE FROM PreparatAlergen
        WHERE PreparatID = @PreparatID;
        
        -- Delete Preparat Photos
        DELETE FROM PreparatFoto
        WHERE PreparatID = @PreparatID;
        
        -- Delete MenuPreparat relationships
        DELETE FROM MenuPreparat
        WHERE PreparatID = @PreparatID;
        
        -- Delete OrderDishes
        DELETE FROM OrderDish
        WHERE PreparatID = @PreparatID;
        
        -- Finally, delete the Preparat
        DELETE FROM Preparat
        WHERE PreparatID = @PreparatID;
        
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