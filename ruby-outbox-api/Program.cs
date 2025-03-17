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

var builder = WebApplication.CreateBuilder(args);

// get options
var configuration = builder.Configuration;
var dbConnection = configuration.GetSection(nameof(DatabaseConfig)).Get<DatabaseConfig>()!;
builder.Services.AddOptions<OutboxOptions>().Bind(configuration.GetSection(nameof(OutboxOptions)));

// db injection
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
builder.Services.AddScoped<IProcessResolver, ProcessResolver>();

builder.Services.AddScoped<IOptionsProvider, OptionsProvider>();

// adding events
//builder.Services.AddScoped<IEventHandler<StartVmCreation>, StartVmCreatingEventHandler>();
//builder.Services.AddScoped<IEventHandler<CreateNic>, CreateNicEventHandler>();
//builder.Services.AddScoped<IEventHandler<CreateVmResource>, CreateVmResourceEventHandler>();
//builder.Services.AddScoped<IEventHandler<CreateAadLoginExtension>, CreateAadLoginEventHandler>();
//builder.Services.AddScoped<IEventHandler<RunPowerShellCommand>, RunPsCommandHandler>();
//builder.Services.AddScoped<IEventHandler<CompleteCreateVmProcess>, CompleteVmCreateEventHandler>();

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