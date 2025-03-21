namespace ruby_outbox_core.Contracts.Interfaces;

public interface IServiceFactory
{
    Type TryGetType(string typeName);
    Type Resolve(Type eventType);
    object? GetServiceInstance(string eventName);
    object? GetServiceInstance(Type eventType);
}
