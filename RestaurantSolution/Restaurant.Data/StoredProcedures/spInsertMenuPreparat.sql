
CREATE PROCEDURE spInsertMenuPreparat
    @MenuID INT,
    @PreparatID INT,
    @QuantityMenuPortie INT
AS
BEGIN
    
    IF EXISTS (SELECT 1 FROM MenuPreparat 
               WHERE MenuID = @MenuID AND PreparatID = @PreparatID)
    BEGIN
       
        UPDATE MenuPreparat
        SET QuantityMenuPortie = @QuantityMenuPortie
        WHERE MenuID = @MenuID AND PreparatID = @PreparatID;
    END
    ELSE
    BEGIN
       
        INSERT INTO MenuPreparat (MenuID, PreparatID, QuantityMenuPortie)
        VALUES (@MenuID, @PreparatID, @QuantityMenuPortie);
    END
END
GO