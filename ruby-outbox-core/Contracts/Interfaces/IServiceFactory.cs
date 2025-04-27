namespace ruby_outbox_core.Contracts.Interfaces;

public interface IServiceFactory
{
    (object? Instance, Type? InstanceType) GetServiceInstance(string eventName);

    (object? Instance, Type? InstanceType) GetServiceInstance(Type eventType);
}
