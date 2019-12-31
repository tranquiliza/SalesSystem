CREATE PROCEDURE [Core].[GetCustomerInformationFromId]
	@customerInformationId UNIQUEIDENTIFIER
AS
	SELECT TOP (1) [Data] FROM [Core].[CustomerInformation] WHERE [CustomerInformationId] = @customerInformationId
GO