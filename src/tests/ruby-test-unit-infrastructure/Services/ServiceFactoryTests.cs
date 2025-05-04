using FluentAssertions;
using ruby_outbox_core.Events.CreateVm;
using ruby_outbox_infrastructure.EventHandlers.CreateVm;
using ruby_outbox_infrastructure.Services;
using ruby_test_core.Attributes;
using ruby_test_core.Helpers;

namespace ruby_test_unit.Services;

public class ServiceFactoryTests
{
    [Theory, MemberData(nameof(GetTypesToResolve))]
    public void GetHandlerTypeByEvent(Type eventType, Type eventHandler)
    {
        var serviceProvider = ServiceProviderFactoryHelper.Init("appsettings.json");
        var serviceFactory = new ServiceFactory(serviceProvider.ServiceProvider);
        var response = serviceFactory.GetServiceInstance(eventType);

        response.Instance.Should().BeAssignableTo(eventHandler);
        response.InstanceType.Should().Be(eventType);
    }

    [Fact]
    public void GetHandlerByTypeNotDefined()
    {
        var serviceProvider = ServiceProviderFactoryHelper.Init("appsettings.json");
        var serviceFactory = new ServiceFactory(serviceProvider.ServiceProvider);
        var response = serviceFactory.GetServiceInstance(typeof(int));

        Assert.Null(response.Instance);
        Assert.Null(response.InstanceType);
    }


    [Theory, MemberData(nameof(GetNameToResolve))]
    public void GetHandlerTypeByEventName(string eventName, Type eventHandler)
    {
        var serviceProvider = ServiceProviderFactoryHelper.Init("appsettings.json");
        var serviceFactory = new ServiceFactory(serviceProvider.ServiceProvider);
        var response = serviceFactory.GetServiceInstance(eventName);

        response.Instance.Should().BeAssignableTo(eventHandler);
    }

    [Theory, AutoMock]
    public void GetHandlerByNameNotDefined(string eventName)
    {
        var serviceProvider = ServiceProviderFactoryHelper.Init("appsettings.json");
        var serviceFactory = new ServiceFactory(serviceProvider.ServiceProvider);
        var response = serviceFactory.GetServiceInstance(eventName);

        Assert.Null(response.Instance);
        Assert.Null(response.InstanceType);
    }

    public static TheoryData<Type, Type> GetTypesToResolve()
    {
        return new TheoryData<Type, Type>
        {
            { typeof(StartVmCreation), typeof(StartVmCreatingEventHandler) },
            { typeof(CreateNic), typeof(CreateNicEventHandler) },
            { typeof(StartVmCreation), typeof(StartVmCreatingEventHandler) },
            { typeof(CreateNic), typeof(CreateNicEventHandler) },
            { typeof(CreateAadLoginExtension), typeof(CreateAadLoginEventHandler) },
            { typeof(CreateNic), typeof(CreateNicEventHandler) },
            { typeof(CreateVmResource), typeof(CreateVmResourceEventHandler) },
            { typeof(RunPowerShellCommand), typeof(RunPsCommandHandler) },
            { typeof(CompleteCreateVmProcess), typeof(CompleteVmCreateEventHandler) },
            { typeof(RunPowerShellCommand), typeof(RunPsCommandHandler) }
        };
    }

    public static TheoryData<string, Type> GetNameToResolve()
    {
        return new TheoryData<string, Type>
        {
            { nameof(StartVmCreation), typeof(StartVmCreatingEventHandler) },
            { nameof(CreateNic), typeof(CreateNicEventHandler) },
            { nameof(StartVmCreation), typeof(StartVmCreatingEventHandler) },
            { nameof(CreateNic), typeof(CreateNicEventHandler) },
            { nameof(CreateAadLoginExtension), typeof(CreateAadLoginEventHandler) },
            { nameof(CreateNic), typeof(CreateNicEventHandler) },
            { nameof(CreateVmResource), typeof(CreateVmResourceEventHandler) },
            { nameof(RunPowerShellCommand), typeof(RunPsCommandHandler) },
            { nameof(CompleteCreateVmProcess), typeof(CompleteVmCreateEventHandler) },
            { nameof(RunPowerShellCommand), typeof(RunPsCommandHandler) }
        };
    }
}