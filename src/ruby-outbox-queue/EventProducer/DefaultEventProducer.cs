using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ruby_outbox_core.Contracts.Interfaces.EventHub;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Contracts.Options;
using System.Text;

namespace ruby_outbox_queue.EventProducer;

public class DefaultEventProducer(
    ILogger<DefaultEventProducer> logger,
    ISecretManager secretManager,
    IOptions<PersonalSettingsConfig> options
    ) : IProducer
{
    private readonly string _eventHubNamespace = "rogachev-eventhub-namespace.servicebus.windows.net";
    private readonly string _hubName = "rogachev-eventhub";

    private readonly string _connectionstring = "Endpoint=sb://rogachev-eventhub-namespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=gaRy0UUqFvIDP7gQvNfj3vjC0Zr7b1/Es+AEhI80jGI=";

    public async Task PublishAsync()
    {
        var numOfEvents = 3;

        //var secretValue = await secretManager.GetSecretValueAsync(options.Value.PersonalSecret);

        //var tokenCredentials = new ClientSecretCredential(
        //        secretValue.TenantId.ToString(),
        //        secretValue.ApplicationId.ToString(),
        //        secretValue.ClientSecret
        //        );

        //EventHubProducerClient producerClient = new EventHubProducerClient(_eventHubNamespace, _hubName, tokenCredentials);
        EventHubProducerClient producerClient = new EventHubProducerClient(_connectionstring, _hubName);

        // Create a batch of events 
        using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

        for (int i = 1; i <= numOfEvents; i++)
        {
            if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes($"Event {i}"))))
            {
                // if it is too large for the batch
                throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
            }
        }

        try
        {
            await producerClient.SendAsync(eventBatch);
            logger.LogInformation("A batch of {i} events has been published.", numOfEvents);
        }
        finally
        {
            await producerClient.DisposeAsync();
        }
    }
}
