using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Events.CreateVm;
using System.Text.Json;

namespace ruby_outbox_infrastructure.Services;

public class OutboxEventPublisher(
    ILogger<OutboxEventPublisher> logger,
    IOutboxMessageRepository repository,
    IProcessResolver resolver,
    IServiceProvider serviceProvider
    ) : IOutboxEventPublisher
{

    private Dictionary<string, Type> _types = new Dictionary<string, Type>();

    public async Task RunAsync()
    {
        var message = await repository.GetMessageToProc();

        if (message == null)
            return;

        logger.LogInformation("Found event {eid}", message.Id);

        Type type = TryGetType(message!.ContentType);   // If the type is null - what to do?
        logger.LogInformation("The Type is {tp}", type);


        if (type == typeof(StartVmCreation))
        {
            var service = ActivatorUtilities.CreateInstance(serviceProvider, type, new object[] { serviceProvider });

            var @event = JsonSerializer.Deserialize(message.Content!, type);

            var method = service.GetType().GetMethod("HandleAsync", new Type[] { type });

            method!.Invoke(service, new object[] { @event! });
        }
        else
        {
            var service = resolver.Resolve(type); // if the service is not resolved - what to do?

            var @event = JsonSerializer.Deserialize(message.Content!, type);

            var method = service.GetType().GetMethod("HandleAsync", new Type[] { type });

            method!.Invoke(service, new object[] { @event! });
        }








    }

    private Type TryGetType(string typeName)
    {
        if (!_types.TryGetValue(typeName, out var type))
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == typeof(IEvent).Assembly.GetName().Name);

            if (assembly != null)
            {
                type = assembly.GetTypes().FirstOrDefault(p => p.Name == typeName);
            }

            if (type == null)
                throw new InvalidOperationException($"Not able to recognize the type {typeName}.");

            _types.Add(typeName, type);
        }

        return type;
    }
}
