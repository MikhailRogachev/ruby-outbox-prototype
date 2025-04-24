using Microsoft.EntityFrameworkCore;
using PVAD.Vms.Infrastructure.Messaging.OutboxEvent;
using ruby_outbox_api.Extensions;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Contracts.Options;
using ruby_outbox_data.Extensions;
using ruby_outbox_data.Persistency;
using ruby_outbox_data.Repositories;
using ruby_outbox_infrastructure.Profiles;
using ruby_outbox_infrastructure.Services;
using ruby_outbox_infrastructure.Services.Azure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// build options
builder.Services.AddOptions<OutboxOptions>().Bind(configuration.GetSection(nameof(OutboxOptions)));
builder.Services.AddOptions<AzureKeyVaultClientConfig>().Bind(configuration.GetSection(nameof(AzureKeyVaultClientConfig)));
builder.Services.AddOptions<PersonalSettingsConfig>().Bind(configuration.GetSection(nameof(PersonalSettingsConfig)));

// db injection
var dbConnection = configuration.GetSection(nameof(DatabaseConfig)).Get<DatabaseConfig>()!;
builder.Services.AddDbContext<ApplicationDbContext>(db => db.UseNpgsql(dbConnection!.NpgsqlConnectionStringBuilder()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IVmRepository, VmRepository>();
builder.Services.AddScoped<IOutboxLoggerRepository, OutboxLoggerRepository>();
builder.Services.AddScoped<IVmService, VmService>();
builder.Services.AddScoped<IOutboxMessageRepository, OutboxRepository>();
builder.Services.AddScoped<IOutboxEventPublisher, OutboxEventPublisher>();
builder.Services.AddScoped<ISecretManager, SecretManager>();
builder.Services.AddScoped<IAzureVirtualMachineService, AzureVirtualMachineService>();

builder.Services.AddScoped<IOptionsProvider, OptionsProvider>();
builder.Services.AddScoped<IServiceFactory, ServiceFactory>();

// addind automapper
builder.Services.AddAutoMapper(typeof(InfrastructureProfile));

// hosted services registration
builder.Services.AddHostedService<OutboxEventService>();

var app = builder.Build();
await app.DataBaseMigrateAsync(app.Logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();