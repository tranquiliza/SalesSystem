CREATE PROCEDURE [dbo].[InsertUpdateUser]
	@id UNIQUEIDENTIFIER,
	@username NVARCHAR(1000),
	@email NVARCHAR(1000),
	@data NVARCHAR(MAX)
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Users WHERE Id = @id)
		INSERT INTO Users(Id, Username, Email, Data)
		VALUES(@id, @username, @email, @data)
	ELSE
		UPDATE Users 
		SET 
		Username = @username,
		Email = @email,
		Data = @data
		WHERE Id = @id
END