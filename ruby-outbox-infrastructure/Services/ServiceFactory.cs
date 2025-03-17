namespace ruby_outbox_infrastructure.Services;

internal class ServiceFactory(IServiceProvider serviceProvider) : IServiceProvider
{
    public object? GetService(Type serviceType)
    {
        throw new NotImplementedException();
    }
}
