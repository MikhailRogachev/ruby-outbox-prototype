using ruby_outbox_core.Contracts.Enums;

namespace ruby_outbox_core.Dto;

public class VmDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PrincipalName { get; set; } = string.Empty;
    public VmStatus Status { get; set; }
}
