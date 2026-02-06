using BuildingBlocks.Application.Outbox;

namespace MasterData.Infrastructure.Outbox;

public class OutboxAccessor(MasterDataContext context) : IOutbox
{
    public void Add(OutboxMessage message)
    {
        context.OutboxMessages.Add(message);
    }

    public Task Save()
    {
        // Save is done automatically using EF Core Change Tracking mechanism during SaveChanges.
        return Task.CompletedTask;
    }
}