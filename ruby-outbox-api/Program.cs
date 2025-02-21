using Microsoft.EntityFrameworkCore;
using ruby_outbox_api.Extensions;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_data.Configuration;
using ruby_outbox_data.Persistency;
using ruby_outbox_data.Repositories;
using ruby_outbox_infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// get options
var configuration = builder.Configuration;
var connectionString = configuration.GetSection(nameof(DatabaseConfig)).Get<DatabaseConfig>()!.GetNpgsqlConnectionString();

builder.Services.AddDbContext<ApplicationDbContext>(db =>
{
    db.UseNpgsql(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();
await app.DataBaseMigrateAsync(app.Logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
