namespace BuildingBlocks.Application.Outbox;

public interface IOutbox
{
    void Add(OutboxMessage outboxMessage);
    Task Save();
}