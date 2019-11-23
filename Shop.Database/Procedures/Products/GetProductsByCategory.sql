CREATE PROCEDURE [dbo].[GetProductsByCategory]
	@category nvarchar(50)
AS
	IF @category = ''
		SELECT Data FROM Products
	ELSE
		SELECT Data FROM Products WHERE Category = @category
GO