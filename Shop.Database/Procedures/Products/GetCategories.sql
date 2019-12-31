CREATE PROCEDURE [Core].[GetCategories]
AS
	SELECT DISTINCT [Category] FROM [Core].[Products]
GO