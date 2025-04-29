using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ruby_outbox_api;

namespace ruby_test_core.Helpers;

public class ServiceProviderFactoryHelper
{
    public IServiceProvider ServiceProvider { get; }

    ServiceProviderFactoryHelper(string appSettingsFile)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(appSettingsFile)
            .Build();

        var serviceCollection = new ServiceCollection();
        var startup = new Startup(configuration);

        startup.ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }


    public static ServiceProviderFactoryHelper Init(string appSettingsFileName)
    {
        var fi = new FileInfo(appSettingsFileName);

        if (!fi.Exists)
            throw new FileNotFoundException(appSettingsFileName);

        return new ServiceProviderFactoryHelper(fi.Name);
    }
}
