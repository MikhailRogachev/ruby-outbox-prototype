using ruby_outbox_core.Dto;

namespace ruby_outbox_core.Contracts.Interfaces.Services;

public interface IVmService
{
    Task<VmDto?> TryGetVmByIdAsync(Guid vmId);
    Task<IList<VmDto>> GetVmsByCustomerIdAsync(Guid customerId);
    Task<VmDto> AddVmAsync(Guid customerId);
}
