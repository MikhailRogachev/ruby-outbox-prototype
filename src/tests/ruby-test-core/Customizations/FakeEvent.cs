using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Events.CreateVm;

namespace ruby_test_core.Customizations;

public class FakeEvent : IEvent
{
    public Guid EventId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid VmId { get; set; }
    public Guid CustomerId { get; set; }

    public FakeEvent()
    {
        EventId = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return $"EventType = {nameof(CreateVmResource)}, EventId = {EventId}, VmId = {VmId}";
    }
}
