using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using ruby_outbox_core.Contracts.Enums;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Events.CreateVm;
using ruby_outbox_core.Models;
using ruby_outbox_infrastructure.EventHandlers.CreateVm;
using ruby_test_core.Attributes;
using ruby_test_unit.Helpers;

namespace ruby_test_unit.Processes;

public class StartVmCreationEventHandlerTests
{
    public static Vm GetVm(VmStatus status) => TestHelper.GetVm(vmStatus: status);

    [Theory, AutoMock]
    public async Task StartVmCreationEventHandlerTests_Success(
        StartVmCreation @event,
        [RegInstance(nameof(GetVm), new object[] { VmStatus.NotStarted })] Vm vm,
        [Frozen] Mock<IVmRepository> vmRepository,
        [Greedy] StartVmCreatingEventHandler sut
        )
    {
        // arrange
        vmRepository.Setup(p => p.TryGetVmByIdAsync(It.IsAny<Guid>())).ReturnsAsync(vm);

        // act 
        await sut.HandleAsync(@event);

        // assert
        vm.Status.Should().Be(VmStatus.CreateNic);
        vm.Events.Any(p => p.GetType() == typeof(CreateVmResource)).Should().BeTrue();
    }

    [Theory, AutoMock]
    public async Task StartVmCreationEventHandlerTests_VmNotFoundException(
        StartVmCreation @event,
        [Frozen] Mock<IVmRepository> vmRepository,
        [Greedy] StartVmCreatingEventHandler sut
        )
    {
        // arrange
        vmRepository.Setup(p => p.TryGetVmByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(Vm));

        // act 
        await sut.HandleAsync(@event);

        // assert
    }
}


