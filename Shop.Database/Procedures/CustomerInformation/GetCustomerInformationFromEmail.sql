CREATE PROCEDURE [Core].[GetCustomerInformationFromEmail]
	@email NVARCHAR(100)
AS
	SELECT TOP (1) [Data] FROM [Core].[CustomerInformation] WHERE [Email] = @email
GO