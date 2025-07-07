using ruby_outbox_core.Contracts.Enums;

namespace ruby_outbox_core.Dto;

/// <summary>
///     Represents a virtual machine (VM) data transfer object (DTO) containing 
///     key information about the VM.
/// </summary>
/// <remarks>
///     This class is typically used to transfer VM-related data between different 
///     layers of an application.
///     It includes identifiers, metadata, and the current status of the VM.
/// </remarks>
public class VmDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PrincipalName { get; set; } = string.Empty;
    public VmStatus Status { get; set; }
}
