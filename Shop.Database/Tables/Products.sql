CREATE TABLE [dbo].[Products]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Price] INT NOT NULL, 
    [IsActive] BIT NOT NULL, 
    [Category] NVARCHAR(50) NOT NULL, 
    [Name] NVARCHAR(200) NOT NULL, 
    [Data] NVARCHAR(MAX) NOT NULL
)
GO

CREATE INDEX IX_Products_Category ON Products(Category)
GO
CREATE INDEX IX_Products_Title ON Products([Name])
GO