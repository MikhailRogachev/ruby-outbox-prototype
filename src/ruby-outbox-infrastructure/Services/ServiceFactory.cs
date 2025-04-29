using Microsoft.Extensions.DependencyInjection;
using ruby_outbox_core.Contracts.Interfaces;

namespace ruby_outbox_infrastructure.Services;

public class ServiceFactory(IServiceProvider serviceProvider) : IServiceFactory
{
    private Dictionary<string, Type> _types = new Dictionary<string, Type>();
    private Dictionary<Type, Type> _handlers = new Dictionary<Type, Type>();

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

    private Type? TryResolve(Type eventType)
    {
        if (!_handlers.TryGetValue(eventType, out var handler))
        {
            var @interface = typeof(IEventHandler<>).GetGenericTypeDefinition();
            var handlerType = @interface.MakeGenericType(eventType);

            var types = AppDomain.CurrentDomain
               .GetAssemblies()
               .Where(p => p.FullName!.Contains("ruby-outbox-"))
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
    /// If the event handler is not resolved by event name, the function returns a null instance.
    /// </remarks>
    public (object? Instance, Type? InstanceType) GetServiceInstance(string eventName)
    {
        var eventType = TryGetType(eventName);

        if (eventType == null)
            return (null, null);

        return GetServiceInstance(eventType);
    }

    /// <inheritdoc cref="IServiceFactory.GetServiceInstance(Type)"/>
    /// <remarks>
    /// If the event handler is not resolved by event type, the function returns a null instance.
    /// </remarks>
    public (object? Instance, Type? InstanceType) GetServiceInstance(Type eventType)
    {
        var serviceType = TryResolve(eventType);
        var instance = ActivatorUtilities.CreateInstance(serviceProvider, serviceType!, new object[] { serviceProvider });

        return (instance, eventType);
    }
}
