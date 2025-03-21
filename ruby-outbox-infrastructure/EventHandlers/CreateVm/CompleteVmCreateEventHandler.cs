using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Events.CreateVm;

namespace ruby_outbox_infrastructure.EventHandlers.CreateVm;

public class CompleteVmCreateEventHandler : BaseEventHandler, IEventHandler<CompleteCreateVmProcess>, IDisposable
{
    [ActivatorUtilitiesConstructor]
    public CompleteVmCreateEventHandler(IServiceProvider serviceProvider) : base(serviceProvider) { }

    public async Task HandleAsync(CompleteCreateVmProcess @event)
    {
        Logger.LogInformation("Complete VM creation process for Event: {event}, VM: {vm} is started", @event.EventId, @event.VmId);

        var vm = await VirtualMachineRepository.TryGetVmByIdAsync(@event.VmId);
        if (vm == null)
            return;

        // TODO Send VM ready integration process
        await CompleteEventAsync(@event.EventId);

        await VirtualMachineRepository.UnitOfWork.SaveAsync();

        Logger.LogInformation("Complete VM creation process for Event: {event}, VM: {vm} is completed", @event.EventId, @event.VmId);
    }

    public void Dispose()
    {

    }
}
