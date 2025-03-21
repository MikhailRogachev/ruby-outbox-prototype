using Microsoft.Extensions.DependencyInjection;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Events.CreateVm;
using ruby_outbox_infrastructure.EventHandlers.CreateVm;

namespace ruby_outbox_infrastructure.Services;

public class ServiceFactory(IServiceProvider serviceProvider) : IServiceFactory
{
    private Dictionary<string, Type> _types = new Dictionary<string, Type>();

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

    public Type Resolve(Type type)
    {
        //var handler = typeof(IEventHandler<>).GetGenericTypeDefinition();
        //var handlerType = handler.MakeGenericType(eventType);

        if (type == typeof(StartVmCreation))
            return typeof(StartVmCreatingEventHandler);
        else if (type == typeof(CreateNic))
            return typeof(CreateNicEventHandler);
        else if (type == typeof(CreateAadLoginExtension))
            return typeof(CreateAadLoginEventHandler);
        else if (type == typeof(CompleteCreateVmProcess))
            return typeof(CompleteVmCreateEventHandler);
        else if (type == typeof(CreateVmResource))
            return typeof(CreateVmResourceEventHandler);
        else if (type == typeof(RunPowerShellCommand))
            return typeof(RunPsCommandHandler);

        return typeof(string);
    }

    public object? GetServiceInstance(Type eventType)
    {
        // get service type to create an instance
        var serviceType = Resolve(eventType);

        return ActivatorUtilities.CreateInstance(serviceProvider, (Type)serviceType, new object[] { serviceProvider });
    }

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
