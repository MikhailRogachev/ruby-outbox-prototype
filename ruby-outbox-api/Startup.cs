namespace ruby_outbox_api;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public Startup(IConfiguration configuration, IHostEnvironment environment)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

}

//public class Program
//{
//    public static async Task<int> Main(string[] args)
//    {
//        CreateSerilogLogger();

//        try
//        {
//            Log.Information("Azure Subscriptions is starting up...");
//            var host = CreateHostBuilder(args).Build();

//            var logger = host.Services.GetRequiredService<ILogger<Program>>();
//            await DatabaseMigration.MigrateDatabase(host, logger);

//            await host.RunAsync();
//            return 0;
//        }
//        catch (Exception ex)
//        {
//            Log.Fatal(ex, "Azure Subscriptions failed to start correctly");
//            return 1;
//        }
//        finally
//        {
//            Log.CloseAndFlush();
//        }
//    }

//    private static IHostBuilder CreateHostBuilder(string[] args) =>
//        Host.CreateDefaultBuilder(args)
//            .UseSerilog(
//                (context, services, configuration) => configuration
//                    .ReadFrom.Configuration(context.Configuration)
//                    .ReadFrom.Services(services))
//            .ConfigureWebHostDefaults(webBuilder =>
//            {
//                webBuilder.UseStartup<Startup>();
//            });

//    private static void CreateSerilogLogger()
//    {
//        var aspEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
//        var configuration = new ConfigurationBuilder()
//            .SetBasePath(Directory.GetCurrentDirectory())
//            .AddJsonFile("appsettings.json")
//            .AddJsonFile($"appsettings.{aspEnv ?? "Production"}.json", optional: true)
//            .Build();

//        Log.Logger = new LoggerConfiguration()
//            .ReadFrom.Configuration(configuration)
//            .CreateBootstrapLogger();
//    }
//
//



/*
public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public Startup(IConfiguration configuration, IHostEnvironment environment)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddDefaultRequestId();
        services.AddDefaultCorrelationId(options =>
        {
            options.AddToLoggingScope = true;
            options.LoggingScopeKey = BasicProprtiesExtension.CorrelationIdHeaderProperty;
        });
        services.AddHttpClient(Microsoft.Extensions.Options.Options.DefaultName)
            .AddCorrelationIdForwarding()
            .AddRequestIdForwarding();

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "AzureSubscriptions"
            });
            options.AddServer(new OpenApiServer
            {
                Url = "http://localhost:49997",
                Description = "URL for local testing"
            });
            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            // Add the SwaggerResponseHeaderFilter
            options.OperationFilter<SwaggerResponseHeaderFilter>();
        });
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddAutoMapper(typeof(Startup));
        services.AddPostgres(_configuration);

        var jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };
        services.AddSingleton(jsonSerializerOptions);

        // Configurations
        services.AddOptions<AzureConfig>()
            .Bind(_configuration.GetSection(nameof(AzureConfig)));
        services.AddOptions<KeyVaultConfig>()
            .Bind(_configuration.GetSection(nameof(KeyVaultConfig)));
        services.AddOptions<RdpGatewayVmSizesSettings>()
            .Bind(_configuration.GetSection(nameof(RdpGatewayVmSizesSettings)));
        services.AddOptions<SessionHostVmSizesSettings>()
            .Bind(_configuration.GetSection(nameof(SessionHostVmSizesSettings)));
        services.AddOptions<GoldenImageOperatingSystemConfig>()
            .Bind(_configuration.GetSection(nameof(GoldenImageOperatingSystemConfig)));

        services.AddSingleton<ISecretManager, SecretManager>();
        services.AddSingleton<IVmSizeService, VmSizeService>();
        services.AddSingleton<IResourceManager, ResourceManager>();
        services.AddScoped<IGraphClient, GraphClient>();
        services.AddSingleton<IGoldenImageOperatingSystemService, GoldenImageOperatingSystemService>();
        // configure retry mechanism for database
        services.AddOptions<EfCoreRetryConfig>()
            .Bind(_configuration.GetSection(nameof(EfCoreRetryConfig)));
        if (_environment.EnvironmentName != "Swagger")
        {
            services.AddRabbitMq(_configuration);
        }
        // configure retry mechanism for api calls
        services.AddOptions<PollyConfig>()
            .Bind(_configuration.GetSection(nameof(PollyConfig)));
        services.AddEventHandlers();
        services.AddRepositoriesWithUnitOfWork();
        services.AddApiClients(_configuration);

        // Domain services
        services.AddScoped<IAzureSubscriptionService, AzureSubscriptionService>();

        // API services
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<IAsyncOperationService, AsyncOperationService>();
        services.ConfigureCommonErrorServices();
        services.AddPolly();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRequestId().UseCorrelationId();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(x => { x.SwaggerEndpoint("v1/swagger.yaml", "AzureSubscriptionsModel"); });
        }

        app.UseSerilogRequestLogging();

        app.ConfigureExceptionHandler(env);

        app.UseRouting().UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse //Splits statuses rather than giving one unified status
            });
        });
    }
}


services.AddSingleton<ICommandBus>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMQCommandBus>>();
            var connection = sp.GetRequiredService<IRabbitMQConnection>();
            var producer = sp.GetRequiredService<IRabbitMQMessageProducer>();
            var consumer = sp.GetRequiredService<IRabbitMQEventingMessageConsumer>();
            var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            var correlationContextAccessor = sp.GetRequiredService<ICorrelationContextAccessor>();
            var requestContextAccessor = sp.GetRequiredService<IRequestContextAccessor>();
            var correlationContextFactory = sp.GetRequiredService<ICorrelationContextFactory>();
            var requestContextFactory = sp.GetRequiredService<IRequestContextFactory>();

            var commandBus = new RabbitMQCommandBus(
                connection,
                producer,
                consumer,
                logger,
                serviceScopeFactory,
                rabbitMQConfig.Exchange,
                correlationContextAccessor,
                requestContextAccessor,
                correlationContextFactory,
                requestContextFactory
                );
            ConfigureCommandBus(commandBus);
            return commandBus;
        });


*/