using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Models;
using System.Text.Json;

namespace ruby_outbox_data.Extensions;

public static class OutboxMessageBuilder
{


    public static IEnumerable<OutboxMessage> GetOutboxData(IEnumerable<IEvent> eventSource)
    {
        var outboxMessage = new List<OutboxMessage>();

        if (eventSource == null || !eventSource.Any())
            return outboxMessage;

        foreach (var @event in eventSource)
        {
            outboxMessage.Add(new OutboxMessage
            {
                Id = @event.Id,
                CreationDate = @event.CreationDate,
                ContentType = @event.GetType().Name,
                Content = JsonSerializer.SerializeToDocument(@event, @event.GetType())
            });
        }

        return outboxMessage;
    }
}
