CREATE PROCEDURE [Core].[GetInquiryFromId]
	@inquiryId UNIQUEIDENTIFIER
AS
	SELECT TOP (1) [Data] FROM [Core].[Inquiries] WHERE [InquiryId] = @inquiryId
GO
