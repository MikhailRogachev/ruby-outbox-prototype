using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ruby_outbox_api;

namespace ruby_test_core.Helpers;

public static class ServiceProviderFactoryHelper
{
    public static IServiceProvider ServiceProvider { get; }

    static ServiceProviderFactoryHelper()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var serviceCollection = new ServiceCollection();
        var startup = new Startup(configuration);

        startup.ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }
}
