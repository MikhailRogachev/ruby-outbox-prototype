namespace ruby_outbox_core.Contracts.Interfaces.Services;

public interface IEventHandler<T>
{
    Task Handle(T @event);
}
