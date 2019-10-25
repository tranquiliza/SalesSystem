CREATE PROCEDURE [dbo].[InsertUpdateProduct]
	@id UNIQUEIDENTIFIER,
	@price INT,
	@isActive BIT,
	@category NVARCHAR(50),
	@name NVARCHAR(200),
	@data NVARCHAR(MAX)
AS
BEGIN
	IF NOT EXISTS(SELECT*FROM Products WHERE Id = @id)
		INSERT INTO Products(Id, Price, IsActive, Category, Name, Data)
		VALUES(@id, @price, @isActive, @category, @name, @data)
	ELSE
	UPDATE Products SET 
		Price = @price,
		IsActive = @isActive,
		Category = @category,
		Name = @name,
		Data = @data
	WHERE Id = @id
END