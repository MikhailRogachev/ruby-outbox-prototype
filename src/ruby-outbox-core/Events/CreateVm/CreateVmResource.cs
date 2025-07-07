using ruby_outbox_core.Contracts.Interfaces;

namespace ruby_outbox_core.Events.CreateVm;

/// <summary>
///     Represents an event for creating a virtual machine (VM) resource.
/// </summary>
/// <remarks>
///     This class is used to encapsulate the details of an event related to the 
///     creation of a virtual machine resource. It includes information such as the 
///     unique identifier of the VM, the event identifier, and the timestamp when 
///     the event was created.
/// </remarks>
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
