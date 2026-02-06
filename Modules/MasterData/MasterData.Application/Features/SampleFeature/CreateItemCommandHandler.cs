using MasterData.Application.Configuration.Commands;
using MasterData.Domain.Item;

namespace MasterData.Application.Features.SampleFeature;

public class CreateItemCommandHandler(IItemRepository itemRepository) : ICommandHandler<CreateItemCommand, string>
{
    public async Task<string> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        await itemRepository.AddAsync(
            Item.Create(Guid.NewGuid(), command.Name, "Sample Description", 0));
        return $"Sample created with name: {command.Name}";
    }
}