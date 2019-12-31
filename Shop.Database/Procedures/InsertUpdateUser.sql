CREATE PROCEDURE [Core].[InsertUpdateUser]
	@guid UNIQUEIDENTIFIER,
	@username NVARCHAR(1000),
	@email NVARCHAR(1000),
	@data NVARCHAR(MAX)
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM [Core].[Users] WHERE [Guid] = @guid)
		INSERT INTO [Core].[Users]([Guid], [Username], [Email], [Data])
		VALUES(@guid, @username, @email, @data)
	ELSE
		UPDATE [Core].[Users]
		SET 
		[Username] = @username,
		[Email] = @email,
		[Data] = @data
		WHERE [Guid] = @guid
END