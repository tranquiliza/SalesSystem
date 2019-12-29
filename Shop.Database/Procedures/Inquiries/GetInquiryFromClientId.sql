CREATE PROCEDURE [Core].[GetInquiryFromClientId]
	@clientId UNIQUEIDENTIFIER
AS
	SELECT TOP (1) [Data] FROM [Core].[Inquiries] WHERE [CreatedByClientId] = @clientId
GO
