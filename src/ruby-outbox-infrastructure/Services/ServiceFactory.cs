using Microsoft.Extensions.DependencyInjection;
using ruby_outbox_core.Contracts.Interfaces;

namespace ruby_outbox_infrastructure.Services;

/// <summary>
/// This class includes functions for services that create new instances of 
/// eventhandles based on specified parameters.
/// </summary>
public class ServiceFactory(IServiceProvider serviceProvider) : IServiceFactory
{
    private Dictionary<string, Type> _events = new Dictionary<string, Type>();
    private Dictionary<Type, Type> _handlers = new Dictionary<Type, Type>();

    private Type? TryGetEventType(string eventName)
    {
        if (!_events.TryGetValue(eventName, out var type))
        {
            var assembly = AppDomain.CurrentDomain
                .GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == typeof(IEvent).Assembly.GetName().Name);

            if (assembly != null)
                type = assembly.GetTypes().FirstOrDefault(p => p.Name == eventName);

            if (type == null)
                return null;

            _events.Add(eventName, type);
        }

        return type;
    }

    private Type? TryResolveEventHandler(Type eventType)
    {
        if (!_handlers.TryGetValue(eventType, out var handler))
        {
            var @interface = typeof(IEventHandler<>).GetGenericTypeDefinition();
            var handlerType = @interface.MakeGenericType(eventType);

            var types = AppDomain.CurrentDomain
               .GetAssemblies()
               .SelectMany(p => p.GetTypes().Where(type => handlerType.IsAssignableFrom(type) && !type.IsInterface));

            if (types == null || !types.Any())
                return null;

            handler = types.First();
            _handlers.Add(eventType, handler);
        }

        return handler;
    }

    /// <inheritdoc cref="IServiceFactory.GetServiceInstance(string)"/>
    /// <remarks>
    /// If the event handler is not resolved by event name, the function returns
    ///  <see cref="Tuple{object?, Type?}">Tuple{ Instance = null, InstanceType = null}</see>.
    /// </remarks>
    public (object? Instance, Type? InstanceType) GetServiceInstance(string eventName)
    {
        var eventType = TryGetEventType(eventName);

        if (eventType == null)
            return (null, null);

        return GetServiceInstance(eventType);
    }

    /// <inheritdoc cref="IServiceFactory.GetServiceInstance(Type)"/>
    /// <remarks>
    /// If the event handler is not resolved by event type, the function returns 
    /// <see cref="Tuple{object?, Type?}">Tuple{ Instance = null, InstanceType = null}</see>.
    /// </remarks>
    public (object? Instance, Type? InstanceType) GetServiceInstance(Type eventType)
    {
        var serviceType = TryResolveEventHandler(eventType);
        if (serviceType == null)
            return (null, null);

        var instance = ActivatorUtilities.CreateInstance(serviceProvider, serviceType!, new object[] { serviceProvider });

        return (instance, eventType);
    }
}
