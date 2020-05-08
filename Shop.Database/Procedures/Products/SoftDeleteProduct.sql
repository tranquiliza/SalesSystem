CREATE PROCEDURE [Core].[SoftDeleteProduct]
	@productId UNIQUEIDENTIFIER
AS
BEGIN
	UPDATE [Core].[Products] 
	SET Deleted = 1
	Where [ProductId] = @productId
END