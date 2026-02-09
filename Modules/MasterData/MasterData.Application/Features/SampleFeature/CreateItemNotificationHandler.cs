using MasterData.Domain.Item;
using MediatR;

namespace MasterData.Application.Features.SampleFeature;

public class CreateItemNotificationHandler(IItemRepository itemRepository)
    : INotificationHandler<CreateItemNotification>
{
    public async Task Handle(CreateItemNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var item        = await itemRepository.GetByIdAsync(domainEvent.ItemId);
        item?.Update("Updated by CreateItemNotificationHandler", domainEvent.ItemDesc, domainEvent.Price);
    }
}