CREATE PROCEDURE [Core].[GetCustomerInformationFromClientId]
	@clientId UNIQUEIDENTIFIER
AS
	SELECT TOP (1) [Data] FROM [Core].[CustomerInformation] WHERE [ClientId] = @clientId
GO