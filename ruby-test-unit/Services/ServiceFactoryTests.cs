using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Events.CreateVm;

namespace ruby_test_unit.Services;

public class ServiceFactoryTests
{
    [Fact]
    public void GetHandlerTypeByEvent()
    {
        var handler = typeof(IEventHandler<>).GetGenericTypeDefinition();
        var handlerType = handler.MakeGenericType(typeof(StartVmCreation));

        //var assembly = AppDomain.CurrentDomain.GetAssemblies()
        //        .FirstOrDefault(a => a.GetName().Name == handlerType.Assembly.GetName().Name);

        //var types = AppDomain.CurrentDomain
        //    .GetAssemblies()
        //    .Where(p => p.FullName!.Contains("ruby-outbox-"))
        //    .SelectMany(p => p.GetTypes());

        var types = AppDomain.CurrentDomain.GetAssemblies();

        //.GetAssemblies()
        //.GetTypes();
        //.Where(type => handlerType.IsAssignableFrom(type) && !type.IsInterface);
    }


}


//
//
//var sp = serviceProvider.GetService(typeof(IEventHandler<StartVmCreation>))!;