using ruby_outbox_core.Contracts.Interfaces;

namespace ruby_outbox_core.Events;

public class CreateVmResource : IEvent
{
    public Guid VmId { get; set; }
    public Guid EventId { get; set; }
    public DateTime CreatedAt { get; set; }

    public CreateVmResource()
    {
        EventId = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return $"EventType = {nameof(CreateVmResource)}, EventId = {EventId}, VmId = {VmId}";
    }
}
