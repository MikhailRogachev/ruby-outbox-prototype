using ruby_outbox_core.Dto;

namespace ruby_outbox_core.Contracts.Interfaces.Services;

public interface IAzureVirtualMachineService
{
    Task<IList<AzureVirtualMachineDto>> GetVirtualMachinesAsync();
}
