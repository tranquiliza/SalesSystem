CREATE PROCEDURE [Core].[GetInquiriesFromClientId]
	@clientId UNIQUEIDENTIFIER
AS
	SELECT [Data] FROM [Core].[Inquiries] WHERE [CreatedByClientId] = @clientId
GO
