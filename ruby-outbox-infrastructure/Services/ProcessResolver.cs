using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Events.CreateVm;

namespace ruby_outbox_infrastructure.Services;

public class ProcessResolver(IServiceProvider serviceProvider) : IProcessResolver
{
    public object Resolve(Type type)
    {
        var n = typeof(IEventHandler<>).GetGenericTypeDefinition();
        var tspd = n.MakeGenericType(type);

        var sp = serviceProvider.GetService(typeof(IEventHandler<StartVmCreation>))!;
        var spd = serviceProvider.GetService(tspd)!;


        return spd!;
    }
}
