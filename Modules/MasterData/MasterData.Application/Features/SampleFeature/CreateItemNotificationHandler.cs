using BuildingBlocks.Application.Email;
using MasterData.Domain.Item;
using MediatR;

namespace MasterData.Application.Features.SampleFeature;

public class CreateItemNotificationHandler(IItemRepository itemRepository, IEmailSender emailSender)
    : INotificationHandler<CreateItemNotification>
{
    public async Task Handle(CreateItemNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var item        = await itemRepository.GetByIdAsync(domainEvent.ItemId);
        item?.Update("Updated by CreateItemNotificationHandler", domainEvent.ItemDesc, domainEvent.Price);
        await emailSender.SendMessage(new EmailMsg("dotrongtan113@gmail.com", "Toi ten la hoanggggf ",
                                                       "Tôi là nội dung email gửi từ CreateItemNotificationHandler"));

        Console.WriteLine("hahaha");
    }
}