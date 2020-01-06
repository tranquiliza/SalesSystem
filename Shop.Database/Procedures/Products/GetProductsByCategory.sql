CREATE PROCEDURE [Core].[GetProductsByCategory]
	@category NVARCHAR(50),
	@onlyActive BIT = 0
AS
	IF (@category = '' AND @onlyActive = 1)
		SELECT [Data] FROM [Core].[Products]
		WHERE IsActive = 1 
		AND Deleted = 0
	ELSE 
	IF (@category = '' AND @onlyActive = 0)
		SELECT [Data] FROM [Core].[Products]
		WHERE Deleted = 0
	ELSE
	IF (@onlyActive = 1)
		SELECT [Data] FROM [Core].[Products] 
		WHERE [Category] = @category
		AND IsActive = 1 
		AND Deleted = 0
	ELSE
		SELECT [Data] FROM [Core].[Products] 
		WHERE [Category] = @category
		AND Deleted = 0
GO