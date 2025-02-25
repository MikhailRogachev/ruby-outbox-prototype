using ruby_outbox_core.Contracts.Interfaces;

namespace ruby_outbox_core.Events;

public class CreateNic : IEvent
{
    public Guid EventId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid VmId { get; set; }
}
