namespace ruby_outbox_core.Contracts.Interfaces;

public interface IEvent
{
    Guid EventId { get; set; }
    DateTime CreatedAt { get; set; }
}
