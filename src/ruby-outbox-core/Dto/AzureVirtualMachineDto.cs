namespace ruby_outbox_core.Dto;

/// <summary>
///     This class represents a Data Transfer Object (DTO) for an Azure Virtual Machine.
/// </summary>
public class AzureVirtualMachineDto
{
    public string ComputerName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RunningStatus { get; set; } = string.Empty;
    public string ProvisionStatus { get; set; } = string.Empty;
    public Guid? VmsAdvancedId { get; set; }
    public DateTimeOffset DateTimeOffset { get; set; }
}
