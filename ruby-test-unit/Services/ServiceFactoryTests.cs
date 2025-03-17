using FluentAssertions;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Events.CreateVm;

namespace ruby_test_unit.Services;

public class ServiceFactoryTests
{
    [Fact]
    public void CreateServiceTest()
    {
        var eventType = typeof(IEventHandler<>).GetGenericTypeDefinition();
        var eventTypeGenerics = eventType.MakeGenericType(typeof(StartVmCreation));

        var assembly = AppDomain.CurrentDomain.GetAssemblies()
               .FirstOrDefault(a => a.GetName().Name == eventTypeGenerics.GetType().Assembly.GetName().Name);






        assembly.Should().NotBeNull();
    }
}


//
//
//var sp = serviceProvider.GetService(typeof(IEventHandler<StartVmCreation>))!;