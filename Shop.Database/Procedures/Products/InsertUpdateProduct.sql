CREATE PROCEDURE [Core].[InsertUpdateProduct]
	@productId UNIQUEIDENTIFIER,
	@price INT,
	@isActive BIT,
	@category NVARCHAR(50),
	@name NVARCHAR(200),
	@data NVARCHAR(MAX)
AS
BEGIN
	IF NOT EXISTS(SELECT * FROM [Core].[Products] WHERE [ProductId] = @productId)
		INSERT INTO [Core].[Products]([ProductId], [Price], [IsActive], [Category], [Name], [Data])
		VALUES(@productId, @price, @isActive, @category, @name, @data)
	ELSE
	UPDATE [Core].[Products] SET 
		[Price] = @price,
		[IsActive] = @isActive,
		[Category] = @category,
		[Name] = @name,
		[Data] = @data
	WHERE [ProductId] = @productId
END