namespace ruby_outbox_core.Events;

public abstract class BaseEvent
{
    public Guid EventId { get; set; }
    public abstract Guid Id { get; set; }
}
