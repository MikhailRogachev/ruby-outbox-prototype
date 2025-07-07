using ruby_outbox_core.Contracts.Interfaces;

namespace ruby_outbox_core.Events.CreateVm;

/// <summary>
///     Represents an event that triggers the execution of a PowerShell command 
///     on a virtual machine.
/// </summary>
/// <remarks>
///     This class is used to encapsulate the details of an event, including its 
///     unique identifier, creation timestamp, and the associated virtual machine 
///     identifier. It implements the <see cref="IEvent"/> interface.
/// </remarks>
public class RunPowerShellCommand : IEvent
{
    public Guid EventId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid VmId { get; set; }

    public RunPowerShellCommand()
    {
        EventId = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return $"EventType = {nameof(RunPowerShellCommand)}, EventId = {EventId}, VmId = {VmId}";
    }
}
