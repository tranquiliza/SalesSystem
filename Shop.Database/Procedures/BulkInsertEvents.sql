CREATE TYPE DomainEventsTable AS TABLE
(eventName VARCHAR(50), domainEventData varchar(max), eventTimestamp datetime2)
GO

CREATE PROCEDURE [Core].[BulkInsertEvents]
	@values DomainEventsTable READONLY
AS
BEGIN 
	INSERT INTO [Core].[DomainEvents]([EventName], [Data], [Timestamp])
	SELECT eventName, domainEventData, eventTimestamp FROM @values
END
GO