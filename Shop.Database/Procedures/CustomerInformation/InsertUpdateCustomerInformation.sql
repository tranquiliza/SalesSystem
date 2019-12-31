CREATE PROCEDURE [Core].[InsertUpdateCustomerInformation]
	@customerInformationId UNIQUEIDENTIFIER,
	@email NVARCHAR(100),
	@phoneNumber NVARCHAR(100),
	@clientId UNIQUEIDENTIFIER,
	@data VARCHAR(MAX)
AS
BEGIN
	IF NOT EXISTS(SELECT * FROM [Core].[CustomerInformation] WHERE [CustomerInformation].[CustomerInformationId] = @customerInformationId)
		INSERT INTO [Core].[CustomerInformation] ([CustomerInformationId], [Email], [PhoneNumber], [ClientId], [Data])
		VALUES (@customerInformationId, @email, @phoneNumber, @clientId, @data)
	ELSE
	UPDATE [Core].[CustomerInformation] SET
		[Email] = @email,
		[PhoneNumber] = @phoneNumber,
		[Data] = @data
	WHERE [CustomerInformationId] = @customerInformationId
END