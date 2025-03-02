using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Interfaces.Services;
using System.Text.Json;

namespace ruby_outbox_infrastructure.Services;

public class OutboxEventPublisher(
    ILogger<OutboxEventPublisher> logger,
    IOutboxMessageRepository repository,
    IProcessResolver resolver
    ) : IOutboxEventPublisher
{
    //private readonly VmsDbContext _context;
    //private readonly IEventBus _eventBus;
    //private readonly ILogger<OutboxEventPublisher> _logger;
    //private readonly OutboxCircuitBreakerPolicy _breakerPolicy;
    //private readonly ICorrelationContextAccessor _correlationContextAccessor;
    //private readonly ICorrelationContextFactory _correlationContextFactory;
    //private readonly IRequestContextAccessor _requestContextAccessor;
    //private readonly IRequestContextFactory _requestContextFactory;

    private Dictionary<string, Type> _types = new Dictionary<string, Type>();

    //private readonly string sqlquery = @"select om.""Id"",
    //        om.""Content"",
    //        om.""ContentType"",
    //        om.""CreationDate"",
    //        om.""CorrelationId"",
    //        om.""RequestId""
    //    from public.""OutboxMessages"" om
    //    order by om.""CreationDate""
    //    limit 1
    //    for update skip locked";

    //public OutboxEventPublisher(
    //    VmsDbContext context,
    //    IEventBus eventBus,
    //    OutboxCircuitBreakerPolicy breakerPolicy,
    //    ILogger<OutboxEventPublisher> logger,
    //    ICorrelationContextAccessor correlationContextAccessor,
    //    ICorrelationContextFactory correlationContextFactory,
    //    IRequestContextAccessor requestContextAccessor,
    //    IRequestContextFactory requestContextFactory
    //    )
    //{
    //    _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    //    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    //    _context = context ?? throw new ArgumentNullException(nameof(context));
    //    _breakerPolicy = breakerPolicy ?? throw new ArgumentNullException(nameof(breakerPolicy));

    //    _correlationContextAccessor = correlationContextAccessor;
    //    _requestContextAccessor = requestContextAccessor;
    //    _correlationContextFactory = correlationContextFactory;
    //    _requestContextFactory = requestContextFactory;
    //}

    //Type type = TryGetType(message.ContentType);
    //var @event = JsonSerializer.Deserialize(message.Content, type);

    //_eventBus.Publish((IEvent) @event!);

    //                   _context.OutboxMessages.Remove(message);
    //                   _ = await _context.SaveChangesAsync();

    //await transaction.CommitAsync();

    //_logger.LogDebug("Message Id {id}, correlationId {cid} has been processed.", message.Id, message.CorrelationId);



    public async Task RunAsync()
    {
        var message = await repository.GetMessageToProc();

        if (message == null)
            return;

        logger.LogInformation("Found event {eid}", message.Id);

        Type type = TryGetType(message!.ContentType);
        logger.LogInformation("The Type is {tp}", type);


        var service = resolver.Resolve(type);





        var @event = JsonSerializer.Deserialize(message.Content!, type);

        var method = service.GetType().GetMethod("HandleAsync", new Type[] { type });

        method!.Invoke(service, new object[] { @event! });



        //logger.LogInformation("The event found {event}", @event!.ToString());












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

        //var serviceType = ProcessesContainer.Handlers[type]!;

        //return serviceType;
        return type;
    }
}
