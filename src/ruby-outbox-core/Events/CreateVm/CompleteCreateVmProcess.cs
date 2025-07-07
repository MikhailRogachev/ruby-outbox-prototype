using ruby_outbox_core.Contracts.Interfaces;

namespace ruby_outbox_core.Events.CreateVm;

/// <summary>
///     Represents an event indicating the completion of a virtual machine creation process.
/// </summary>
/// <remarks>
///     This event contains information about the virtual machine creation process, including 
///     the event's unique identifier, the timestamp when the event was created, and the 
///     identifier of the virtual machine associated with the event.
/// </remarks>
public class CompleteCreateVmProcess : IEvent
{
    public Guid EventId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid VmId { get; set; }

    public CompleteCreateVmProcess()
    {
        EventId = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return $"EventType = {nameof(CompleteCreateVmProcess)}, EventId = {EventId}, VmId = {VmId}";
    }
}
