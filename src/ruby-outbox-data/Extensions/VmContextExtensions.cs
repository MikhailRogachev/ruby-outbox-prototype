using Microsoft.EntityFrameworkCore;
using ruby_outbox_core.Models;
using ruby_outbox_data.Persistency;

namespace ruby_outbox_data.Extensions;

public static class VmContextExtensions
{
    public static async Task<Vm?> TryGetVmByIdAsync(this ApplicationDbContext context, Guid vmId)
    {
        return await context.Vms.FirstOrDefaultAsync(p => p.Id == vmId);
    }
}
