using ruby_outbox_core.Models;

namespace ruby_outbox_core.Contracts.Interfaces.Repositories;

public interface IVmRepository
{
    IUnitOfWork UnitOfWork { get; }
    Task<Vm?> TryGetVmByIdAsync(Guid vmId);
    Task<IList<Vm>> GetVmsByCustomerIdAsync(Guid customerId);
    Vm Update(Vm vm);
    Vm AddVm(Vm vm);
    void RemoveVm(Vm vm);
}
