using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Interfaces.Services;
using System.Text.Json;

namespace ruby_outbox_infrastructure.Services;

public class OutboxEventPublisher(
    ILogger<OutboxEventPublisher> logger,
    IOutboxMessageRepository repository,
    IServiceFactory serviceFactory
    ) : IOutboxEventPublisher
{
    public async Task RunAsync()
    {
        var message = await repository.GetMessageToProc();

        if (message == null)
            return;

        logger.LogInformation("Found event {eid}", message.Id);

        var eventType = serviceFactory.TryGetType(message!.ContentType);
        if (eventType == null)
        {
            logger.LogError("The eventType for the {et} was not identified. Message id - {mid}", message!.ContentType, message.Id);
            return;
        }

        var service = serviceFactory.GetServiceInstance(eventType);
        if (service == null)
        {
            logger.LogError("The service for the event {en} was not identified. Message id - {mid}", message!.ContentType, message.Id);
            return;
        }

        logger.LogInformation("The eventHandler {eh} has been identified for the message id - {mid}", service.GetType(), message.Id);

        var @event = JsonSerializer.Deserialize(message.Content!, eventType);

        var method = service.GetType().GetMethod("HandleAsync", new Type[] { eventType });

        method!.Invoke(service, new object[] { @event! });
    }
}
