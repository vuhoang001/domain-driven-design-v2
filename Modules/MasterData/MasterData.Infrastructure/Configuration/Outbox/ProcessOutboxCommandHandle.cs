using BuildingBlocks.Application.Data;
using BuildingBlocks.Application.Events;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Dapper;
using MasterData.Application.Configuration.Commands;
using MediatR;
using Newtonsoft.Json;

namespace MasterData.Infrastructure.Configuration.Outbox;

public class ProcessOutboxCommandHandle(
    IMediator mediator,
    ISqlConnectionFactory sqlConnectionFactory,
    IDomainEventNotificationMapper domainNotificationMapper) : ICommandHandler<ProcessOutboxCommand>
{
    public async Task Handle(ProcessOutboxCommand request, CancellationToken cancellationToken)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = $"""
                            SELECT
                                [OutboxMessage].[Id] AS [{nameof(OutboxMessageDto.Id)}],
                                [OutboxMessage].[Type] AS [{nameof(OutboxMessageDto.Type)}],
                                [OutboxMessage].[Data] AS [{nameof(OutboxMessageDto.Data)}]
                            FROM [dbo].[OutboxMessages] AS [OutboxMessage]
                            WHERE [OutboxMessage].[ProcessedDate] IS NULL
                            ORDER BY [OutboxMessage].[OccurredOn]
                            """;

        var messages = await connection.QueryAsync<OutboxMessageDto>(sql);

        var messagesList = messages.ToList();

        const string sqlUpdateProcessedDate = """
                                              UPDATE [dbo].[OutboxMessages]
                                              SET
                                                  ProcessedDate = @ProcessedDate,
                                                  [Error] = @Error
                                              WHERE [Id] = @Id
                                              """;
        if (messagesList.Count > 0)
        {
            foreach (var message in messages)
            {
                DateTime? processedDate = null;
                string?   error         = null;

                try
                {
                    var type = domainNotificationMapper.GetType(message.Type)
                        ?? throw new InvalidOperationException($"Unknown type {message.Type}");

                    var @event = JsonConvert.DeserializeObject(message.Data, type)
                            as IDomainEventNotification
                        ?? throw new InvalidOperationException("Deserialize failed");

                    await mediator.Publish(@event, cancellationToken);

                    processedDate = DateTime.UtcNow;
                    error         = null;
                }
                catch (Exception ex)
                {
                    processedDate = null;
                    error         = ex.Message;
                }

                await connection.ExecuteAsync(sqlUpdateProcessedDate, new
                {
                    Id            = message.Id,
                    ProcessedDate = processedDate,
                    Error         = error
                });
            }
        }
    }
}