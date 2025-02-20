using Microsoft.EntityFrameworkCore;
using ruby_outbox_api.Extensions;
using ruby_outbox_data.Configuration;
using ruby_outbox_data.Persistency;

var builder = WebApplication.CreateBuilder(args);

// get options
var configuration = builder.Configuration;
var connectionString = configuration.GetSection(nameof(DatabaseConfig)).Get<DatabaseConfig>()!.GetNpgsqlConnectionString();

builder.Services.AddDbContext<ApplicationDbContext>(db =>
{
    db.UseNpgsql(connectionString);
});


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
await app.DataBaseMigrateAsync(app.Logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
