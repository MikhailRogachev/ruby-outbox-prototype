using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Events.CreateVm;
using ruby_outbox_infrastructure.EventHandlers.CreateVm;

namespace ruby_outbox_infrastructure.Services;

[Obsolete]
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

    public Type ResolveType(Type type)
    {

        if (type == typeof(StartVmCreation))
            return typeof(StartVmCreatingEventHandler);
        else if (type == typeof(CreateNic))
            return typeof(CreateNicEventHandler);
        else if (type == typeof(CreateAadLoginExtension))
            return typeof(CreateAadLoginEventHandler);
        else if (type == typeof(CompleteCreateVmProcess))
            return typeof(CompleteVmCreateEventHandler);
        else if (type == typeof(CreateVmResource))
            return typeof(CreateVmResourceEventHandler);
        else if (type == typeof(RunPowerShellCommand))
            return typeof(RunPsCommandHandler);

        return typeof(string);
    }
}
