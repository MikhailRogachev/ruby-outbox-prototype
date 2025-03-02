using Microsoft.Extensions.DependencyInjection;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Events;
using ruby_outbox_infrastructure.Processes;

namespace ruby_outbox_infrastructure.Services;

public static class ProcessesContainer
{
    public static Dictionary<Type, Type> Handlers { get; } = new Dictionary<Type, Type>();

    public static void Init(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IEventHandler<StartVmCreation>, CreateVmProcess>();
        Handlers.Add(typeof(StartVmCreation), typeof(IEventHandler<StartVmCreation>));

        serviceCollection.AddScoped<IEventHandler<CreateNic>, CreateVmProcess>();
        Handlers.Add(typeof(CreateNic), typeof(IEventHandler<CreateNic>));
    }
}


public interface IProcessResolver
{
    object Resolve(Type @type);
}


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


//var openImplementation = _map[typeof(TContract).GetGenericTypeDefinition()];
//   var closedImplementation = openImplementation.MakeGenericType(
//       typeof(TContract).GenericTypeArguments);
//               return Create<TContract>(closedImplementation);