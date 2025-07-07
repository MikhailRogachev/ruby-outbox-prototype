using ruby_outbox_core.Contracts.Interfaces;

namespace ruby_outbox_core.Events.CreateVm;

/// <summary>
///     Represents an event that initiates the creation of a virtual machine (VM).
/// </summary>
/// <remarks>
///     This event contains information about the VM being created, the customer associated 
///     with the request, and metadata such as the event's unique identifier and creation 
///     timestamp.
/// </remarks>
public class StartVmCreation : IEvent
{
    public Guid VmId { get; set; }
    public Guid EventId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CustomerId { get; set; }

    public StartVmCreation()
    {
        EventId = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return $"EventType = {nameof(StartVmCreation)}, EventId = {EventId}, VmId = {VmId}";
    }
}
