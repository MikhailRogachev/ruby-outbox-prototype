using ruby_outbox_core.Contracts.Interfaces;

namespace ruby_outbox_core.Events.CreateVm;

/// <summary>
///     Represents an event for creating an Azure Active Directory (AAD) login 
///     extension on a virtual machine.
/// </summary>
/// <remarks>
///     This event is typically used to track the creation of an AAD login extension 
///     for a specific virtual machine. The <see cref="EventId"/> uniquely identifies 
///     the event, while <see cref="CreatedAt"/> indicates the timestamp when the event 
///     was created. The <see cref="VmId"/> property specifies the virtual machine 
///     associated with the event.
/// </remarks>
public class CreateAadLoginExtension : IEvent
{
    public Guid EventId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid VmId { get; set; }

    public CreateAadLoginExtension()
    {
        EventId = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return $"EventType = {nameof(CreateAadLoginExtension)}, EventId = {EventId}, VmId = {VmId}";
    }
}
