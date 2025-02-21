using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Enums;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Models;
using ruby_outbox_data.Extensions;

namespace ruby_outbox_data.Persistency;
/// <summary>
/// migrations:
/// cd 
/// dotnet ef migrations remove --project ../ruby-outbox-data --context ApplicationDbContext
/// </summary>
public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    ILogger<ApplicationDbContext> logger
        ) : DbContext(options), IUnitOfWork
{

    private readonly ILogger<ApplicationDbContext> _logger = logger;

    public DbSet<Vm> Vms => Set<Vm>();
    public DbSet<CloudProcess> CloudProcesses => Set<CloudProcess>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // CloudProcess Configuration
        modelBuilder.Entity<CloudProcess>().Property(p => p.Id).ValueGeneratedNever();

        // Vm Configuration
        modelBuilder.Entity<Vm>().Property(p => p.Id).ValueGeneratedNever();
        modelBuilder.Entity<Vm>().Property(p => p.Status).HasConversion(new EnumToStringConverter<VmStatus>());
        modelBuilder.Entity<Vm>()
            .HasMany(p => p.CloudProcesses)
            .WithOne(p => p.Vm)
            .HasForeignKey(p => p.VmId)
            .IsRequired(false);

        // Customer Configuration
        modelBuilder.Entity<Customer>()
            .HasMany(p => p.Vms)
            .WithOne(p => p.Customer)
            .HasForeignKey(p => p.CustomerId)
            .IsRequired(false);
    }

    public async Task<bool> SaveAsync(CancellationToken cancellationToken = default)
    {
        OutboxPopulate();
        await base.SaveChangesAsync(cancellationToken);
        CleanPendings();

        return true;
    }

    private void OutboxPopulate()
    {
        var pendingEventEntities = this.ChangeTracker
            .Entries<Base>()
            .Where(p => p.Entity.Events != null && p.Entity.Events.Any());

        if (pendingEventEntities == null)
            return;

        var pendingEvents = pendingEventEntities.SelectMany(p => p.Entity.Events).ToList();
        var messages = OutboxMessageBuilder.GetOutboxData(pendingEvents);

        OutboxMessages.AddRange(messages);
    }

    private void CleanPendings()
    {
        var pendingEventEntities = this.ChangeTracker
            .Entries<Base>()
            .Where(p => p.Entity.Events != null && p.Entity.Events.Any());

        pendingEventEntities.ToList().ForEach(p => p.Entity.RemoveAllEvents());

    }
}