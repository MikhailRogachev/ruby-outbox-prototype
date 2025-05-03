namespace ruby_outbox_core.Contracts.Interfaces.EventHub;

public interface IProducer
{
    Task PublishAsync();
}
