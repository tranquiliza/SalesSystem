CREATE TYPE DomainEventsTable AS TABLE
(eventName VARCHAR(50), domainEventData varchar(max), eventTimestamp datetime2)
GO

CREATE PROCEDURE [dbo].[BulkInsertEvents]
	@values DomainEventsTable READONLY
AS
BEGIN 
	INSERT INTO DomainEvents(EventName, Data, Timestamp)
	SELECT eventName, domainEventData, eventTimestamp FROM @values
END
GO