CREATE PROCEDURE [Core].[GetCategories]
	@onlyActive BIT = 0
AS
	IF (@onlyActive = 1)
		SELECT DISTINCT [Category] FROM [Core].[Products]
		WHERE IsActive = 1 AND Deleted = 0
	ELSE
		SELECT DISTINCT [Category] FROM [Core].[Products]
		WHERE Deleted = 0
GO