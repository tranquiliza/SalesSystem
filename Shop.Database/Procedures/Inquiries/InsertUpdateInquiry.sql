CREATE PROCEDURE [Core].[InsertUpdateInquiry]
	@inquiryId UNIQUEIDENTIFIER,
	@createdByClientId UNIQUEIDENTIFIER,
	@inquiryState INT,
	@customerEmail NVARCHAR(100) NULL,
	@userId UNIQUEIDENTIFIER NULL,
	@data VARCHAR(MAX)
AS
BEGIN
	IF NOT EXISTS(SELECT * FROM [Core].[Inquiries] WHERE [Inquiries].[InquiryId] = @inquiryId)
		INSERT INTO [Core].[Inquiries] ([InquiryId], [CreatedByClientId], [InquiryState],[CustomerEmail], [UserId], [Data])
		VALUES (@inquiryId, @createdByClientId, @inquiryState, @customerEmail, @userId, @data)
	ELSE
	UPDATE [Core].[Inquiries] SET
		[InquiryState] = @inquiryState,
		[CustomerEmail] = @customerEmail,
		[UserId] = @userId,
		[Data] = @data
	WHERE [Inquiries].[InquiryId] = @inquiryId
END
