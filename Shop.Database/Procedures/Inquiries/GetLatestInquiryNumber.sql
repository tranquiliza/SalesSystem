CREATE PROCEDURE [Core].[GetLatestInquiryNumber]
AS
	SELECT TOP (1) [InquiryNumber] FROM [Core].[Inquiries] 
								   ORDER BY [InquiryNumber] DESC
GO
