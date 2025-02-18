using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Models;

namespace ruby_outbox_data.Persistency;
/// <summary>
/// migrations:
/// dotnet ef migrations add initialmigration --project ../ruby-outbox-data --context ApplicationDbContext --output-dir ../ruby-outbox-data/Persistency/Migrations
/// dotnet ef migrations remove --project ../ruby-outbox-data --context ApplicationDbContext
/// </summary>
public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    ILogger<ApplicationDbContext> logger
        ) : DbContext(options), IUnitOfWork
{

    private readonly ILogger<ApplicationDbContext> _logger = logger;

    public DbSet<Vm> Vms { get; set; }
    public DbSet<CloudProcess> CloudProcesses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfiguration(new VmConfiguration());
        //modelBuilder.ApplyConfiguration(new CloudProcessConfiguration());
    }

    public async Task<bool> SaveAsync(CancellationToken cancellationToken = default)
    {
        return true;
    }
}