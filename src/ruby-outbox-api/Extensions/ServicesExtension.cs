namespace ruby_outbox_api.Extensions;

public static class ServicesExtension
{
    /// <summary>
    ///     Configures and binds an options class to a configuration section.
    /// </summary>
    /// <remarks>
    ///     This method binds the specified options class <typeparamref name="TOpt"/> to a configuration
    ///     section named after the class. The configuration section is retrieved using <see
    ///     cref="ConfigurationManager.GetSection(string)"/>.
    /// </remarks>
    /// <typeparam name="TOpt">The type of the options class to configure. Must be a reference type.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the options configuration will be added.</param>
    /// <param name="configuration">The <see cref="ConfigurationManager"/> instance containing the configuration data.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the options configuration added.</returns>
    public static IServiceCollection IncludeOptions<TOpt>(this IServiceCollection services, ConfigurationManager configuration) where TOpt : class
    {
        services.AddOptions<TOpt>().Bind(configuration.GetSection(nameof(TOpt)));
        return services;
    }
}
