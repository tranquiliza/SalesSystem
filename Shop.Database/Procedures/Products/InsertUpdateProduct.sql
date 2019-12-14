﻿CREATE PROCEDURE [Core].[InsertUpdateProduct]
	@guid UNIQUEIDENTIFIER,
	@price INT,
	@isActive BIT,
	@category NVARCHAR(50),
	@name NVARCHAR(200),
	@data NVARCHAR(MAX)
AS
BEGIN
	IF NOT EXISTS(SELECT * FROM Products WHERE [Guid] = @guid)
		INSERT INTO Products([Guid], [Price], [IsActive], [Category], [Name], [Data])
		VALUES(@guid, @price, @isActive, @category, @name, @data)
	ELSE
	UPDATE [Core].[Products] SET 
		[Price] = @price,
		[IsActive] = @isActive,
		[Category] = @category,
		[Name] = @name,
		[Data] = @data
	WHERE [Guid] = @Guid
END