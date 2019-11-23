CREATE PROCEDURE [dbo].[GetCategories]
AS
	SELECT DISTINCT Category FROM Products
GO