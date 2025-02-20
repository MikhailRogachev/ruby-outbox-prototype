namespace ruby_outbox_core.Contracts.Interfaces;

public interface IEvent
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }
}
