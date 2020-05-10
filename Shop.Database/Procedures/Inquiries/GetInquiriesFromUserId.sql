CREATE PROCEDURE [Core].[GetInquiriesFromUserId]
	@userId UNIQUEIDENTIFIER
AS
	SELECT [Data] FROM [Core].[Inquiries] WHERE [UserId] = @userId
GO
