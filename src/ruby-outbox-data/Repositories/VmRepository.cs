using Microsoft.EntityFrameworkCore;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Models;
using ruby_outbox_data.Persistency;

namespace ruby_outbox_data.Repositories;

public class VmRepository(ApplicationDbContext context) : IVmRepository
{
    public IUnitOfWork UnitOfWork => context;

    public Vm AddVm(Vm vm)
    {
        return context.Add(vm).Entity;
    }

    public void RemoveVm(Vm vm)
    {
        context.Vms.Remove(vm);
    }

    public async Task<IList<Vm>> GetVmsByCustomerIdAsync(Guid customerId)
    {
        return await context
            .Vms
            .Include(x => x.CustomerId)
            .Where(p => p.CustomerId == customerId).ToListAsync();
    }

    public async Task<Vm?> TryGetVmByIdAsync(Guid vmId)
    {
        return await context.Vms.FirstOrDefaultAsync(p => p.Id == vmId);
    }

    public Vm Update(Vm vm)
    {
        return context.Update(vm).Entity;
    }
}
