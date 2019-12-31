CREATE PROCEDURE [Core].[GetProductFromId]
	@Guid UNIQUEIDENTIFIER
AS
	SELECT TOP(1) [Data] FROM [Core].[Products] WHERE [Guid] = @Guid
GO