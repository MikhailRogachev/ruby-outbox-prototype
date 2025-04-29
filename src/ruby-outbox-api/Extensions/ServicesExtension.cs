namespace ruby_outbox_api.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection IncludeOptions<TOpt>(this IServiceCollection services, ConfigurationManager configuration) where TOpt : class
    {
        services.AddOptions<TOpt>().Bind(configuration.GetSection(nameof(TOpt)));
        return services;
    }
}
