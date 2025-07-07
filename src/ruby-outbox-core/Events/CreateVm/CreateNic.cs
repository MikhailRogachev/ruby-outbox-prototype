using ruby_outbox_core.Contracts.Interfaces;

namespace ruby_outbox_core.Events.CreateVm;

/// <summary>
///     Represents an event for creating a network interface card (NIC) associated with 
///     a virtual machine (VM).
/// </summary>
/// <remarks>
///     This event contains information about the creation of a NIC, including the 
///     event's unique identifier, the timestamp of when the event was created, and 
///     the identifier of the associated virtual machine.
/// </remarks>
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
        return $"EventType = {nameof(CreateVmResource)}, EventId = {EventId}, VmId = {VmId}";
    }
}
