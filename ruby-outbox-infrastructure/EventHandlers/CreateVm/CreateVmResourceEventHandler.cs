using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Events.CreateVm;

namespace ruby_outbox_infrastructure.EventHandlers.CreateVm;

public class CreateVmResourceEventHandler : BaseEventHandler, IEventHandler<CreateVmResource>
{
    [ActivatorUtilitiesConstructor]
    public CreateVmResourceEventHandler(IServiceProvider serviceProvider) : base(serviceProvider) { }

    public async Task HandleAsync(CreateVmResource @event)
    {
        Logger.LogInformation("Creation NIC process for Event: {event}, VM: {vm} is started", @event.EventId, @event.VmId);

        var vm = await VirtualMachineRepository.TryGetVmByIdAsync(@event.VmId);
        if (vm == null)
            return;

        DoSomething(nameof(CreateVmResourceEventHandler));

        vm!.CreateAadLogin();

        await CompleteEventAsync(@event.EventId);

        await VirtualMachineRepository.UnitOfWork.SaveAsync();

        Logger.LogInformation("Creation NIC process for Event: {event}, VM: {vm} is completed", @event.EventId, @event.VmId);
    }
}
