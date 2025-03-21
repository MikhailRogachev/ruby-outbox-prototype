using Microsoft.Extensions.DependencyInjection;
using ruby_outbox_core.Contracts.Interfaces;

namespace ruby_outbox_infrastructure.Services;

public class ServiceFactory(IServiceProvider serviceProvider) : IServiceFactory
{
    private Dictionary<string, Type> _types = new Dictionary<string, Type>();
    private Dictionary<Type, Type> _handlers = new Dictionary<Type, Type>();

    public Type TryGetType(string typeName)
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

    public Type Resolve(Type eventType)
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
                throw new InvalidOperationException($"Not able to resolve event type {eventType.Name}");

            handler = types.First();
            _handlers.Add(eventType, handler);
        }

        return handler;
    }



    // TODO : consider when Resolve generates an error
    public object? GetServiceInstance(Type eventType)
    {
        // get service type to create an instance
        var serviceType = Resolve(eventType);

        return ActivatorUtilities.CreateInstance(serviceProvider, (Type)serviceType, new object[] { serviceProvider });
    }


    // TODO : consider when TryGetType and Resolve generates an error
    public object? GetServiceInstance(string eventName)
    {
        var eventType = TryGetType(eventName);
        if (eventType == null)
            return null;

        // get service type to create an instance
        var serviceType = Resolve(eventType);

        return ActivatorUtilities.CreateInstance(serviceProvider, (Type)serviceType, new object[] { serviceProvider });
    }
}
