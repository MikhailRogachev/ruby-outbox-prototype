using ruby_outbox_core.Contracts.Interfaces;

namespace ruby_outbox_core.Events;

public class CreateNic : IEvent
{
    public Guid EventId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid VmId { get; set; }

    public CreateNic()
    {
        EventId = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return $"EventType = {nameof(CreateNic)}, EventId = {EventId}, VmId = {VmId}";
    }
}
