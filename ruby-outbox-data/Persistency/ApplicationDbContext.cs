using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ruby_outbox_core.Contracts.Enums;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Models;
using ruby_outbox_data.Extensions;

namespace ruby_outbox_data.Persistency;
/// <summary>
/// migrations: dotnet ef migrations add update-outbox-table --project ../ruby-outbox-data --context ApplicationDbContext --output-dir ../ruby-outbox-data/Persistency/Migrations
/// cd 
/// dotnet ef migrations remove --project ../ruby-outbox-data --context ApplicationDbContext
/// </summary>
public class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly DbContextOptions<ApplicationDbContext> _dboptions;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        _dboptions = options;
    }

    public DbContextOptions<ApplicationDbContext> DbOptions => _dboptions;
    public DbSet<Vm> Vms => Set<Vm>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<OutboxErrorLogger> OutboxErrorLoggers => Set<OutboxErrorLogger>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Vm Configuration
        modelBuilder.Entity<Vm>().Property(p => p.Id).ValueGeneratedNever();
        modelBuilder.Entity<Vm>().Property(p => p.Status).HasConversion(new EnumToStringConverter<VmStatus>());

        // Customer Configuration
        modelBuilder.Entity<Customer>().Property(p => p.Status).HasConversion(new EnumToStringConverter<CustomerStatus>());

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