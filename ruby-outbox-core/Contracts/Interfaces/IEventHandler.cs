namespace ruby_outbox_core.Contracts.Interfaces;

public interface IEventHandler<T>
{
    Task HandleAsync(T @event);
}
