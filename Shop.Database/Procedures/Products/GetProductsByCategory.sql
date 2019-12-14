CREATE PROCEDURE [Core].[GetProductsByCategory]
	@category nvarchar(50)
AS
	IF @category = ''
		SELECT [Data] FROM [Core].[Products]
	ELSE
		SELECT [Data] FROM [Core].[Products] WHERE [Category] = @category
GO