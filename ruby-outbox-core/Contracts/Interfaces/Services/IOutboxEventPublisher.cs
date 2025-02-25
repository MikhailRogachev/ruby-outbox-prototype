namespace ruby_outbox_core.Contracts.Interfaces.Services;

public interface IOutboxEventPublisher
{
    Task RunAsync();
}
