using FluentAssertions;
using Moq;
using ruby_outbox_core.Events.CreateVm;
using ruby_outbox_infrastructure.EventHandlers.CreateVm;
using ruby_outbox_infrastructure.Services;

namespace ruby_test_unit.Services;

public class ServiceFactoryTests
{
    [Theory, MemberData(nameof(GetTypesToResolve))]
    public void GetHandlerTypeByEvent(Type eventType, Type eventHandler)
    {
        var serviceFactory = new ServiceFactory(new Mock<IServiceProvider>().Object);
        var response = serviceFactory.Resolve(eventType);

        response.Should().Be(eventHandler);
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
}