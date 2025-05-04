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

        logger.LogDebug("Found event {eid}", message.Id);

        var service = serviceFactory.GetServiceInstance(message.ContentType);
        if (service.Instance == null)
        {
            logger.LogError("The service for the event {en} was not identified. Message id - {mid}", message!.ContentType, message.Id);
            return;
        }

        logger.LogDebug("The eventHandler {eh} has been identified for the message id - {mid}", service.Instance.GetType(), message.Id);

        var @event = JsonSerializer.Deserialize(message.Content!, service.InstanceType!);


        try
        {
            var method = service.Instance.GetType().GetMethod("HandleAsync", new Type[] { service.InstanceType! });

            method!.Invoke(service.Instance, new object[] { @event! });

        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
        }
    }
}
