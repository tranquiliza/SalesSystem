CREATE PROCEDURE [Core].[InsertUpdateCustomerInformation]
	@customerInformationId UNIQUEIDENTIFIER,
	@email NVARCHAR(100),
	@phoneNumber NVARCHAR(100),
	@data VARCHAR(MAX)
AS
BEGIN
	IF NOT EXISTS(SELECT * FROM [Core].[CustomerInformation] WHERE [CustomerInformation].[CustomerInformationId] = @customerInformationId)
		INSERT INTO [Core].[CustomerInformation] ([CustomerInformationId], [Email], [PhoneNumber], [Data])
		VALUES (@customerInformationId, @email, @phoneNumber, @data)
	ELSE
	UPDATE [Core].[CustomerInformation] SET
		[Email] = @email,
		[PhoneNumber] = @phoneNumber,
		[Data] = @data
	WHERE [CustomerInformationId] = @customerInformationId
END