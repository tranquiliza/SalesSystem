CREATE PROCEDURE [Core].[GetAllInquiries]
	@minimumState INT
AS
	SELECT [Data] FROM [Core].[Inquiries] WHERE InquiryState >= @minimumState
GO
