using Microsoft.EntityFrameworkCore;
using PVAD.Vms.Infrastructure.Messaging.OutboxEvent;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.EventHub;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Contracts.Options;
using ruby_outbox_data.Extensions;
using ruby_outbox_data.Persistency;
using ruby_outbox_data.Repositories;
using ruby_outbox_infrastructure.Profiles;
using ruby_outbox_infrastructure.Services;
using ruby_outbox_infrastructure.Services.Azure;
using ruby_outbox_queue.EventProducer;

namespace ruby_outbox_api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // add logging
        services.AddLogging();

        // build options
        services.AddOptions<OutboxOptions>().Bind(_configuration.GetSection(nameof(OutboxOptions)));
        services.AddOptions<AzureKeyVaultClientConfig>().Bind(_configuration.GetSection(nameof(AzureKeyVaultClientConfig)));
        services.AddOptions<PersonalSettingsConfig>().Bind(_configuration.GetSection(nameof(PersonalSettingsConfig)));

        // db injection
        var dbConnection = _configuration.GetSection(nameof(DatabaseConfig)).Get<DatabaseConfig>()!;
        services.AddDbContext<ApplicationDbContext>(db => db.UseNpgsql(dbConnection!.NpgsqlConnectionStringBuilder()));

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Add services to the container.
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IVmRepository, VmRepository>();
        services.AddScoped<IOutboxLoggerRepository, OutboxLoggerRepository>();
        services.AddScoped<IVmService, VmService>();
        services.AddScoped<IOutboxMessageRepository, OutboxRepository>();
        services.AddScoped<IOutboxEventPublisher, OutboxEventPublisher>();
        services.AddScoped<ISecretManager, SecretManager>();
        services.AddScoped<IAzureVirtualMachineService, AzureVirtualMachineService>();

        // event hub
        services.AddScoped<IProducer, DefaultEventProducer>();

        services.AddScoped<IOptionsProvider, OptionsProvider>();
        services.AddScoped<IServiceFactory, ServiceFactory>();

        // addind automapper
        services.AddAutoMapper(typeof(InfrastructureProfile));

        // hosted services registration
        services.AddHostedService<OutboxEventService>();
    }
}