CREATE PROCEDURE [Core].[GetProductFromId]
	@ProductId UNIQUEIDENTIFIER
AS
	SELECT TOP(1) [Data] FROM [Core].[Products] WHERE [ProductId] = @ProductId
GO