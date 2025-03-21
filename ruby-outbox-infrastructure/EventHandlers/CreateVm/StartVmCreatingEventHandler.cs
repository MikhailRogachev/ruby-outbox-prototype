using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Dto;
using ruby_outbox_core.Events.CreateVm;
using ruby_outbox_core.Exceptions;

namespace ruby_outbox_infrastructure.EventHandlers.CreateVm;

public class StartVmCreatingEventHandler : BaseEventHandler, IEventHandler<StartVmCreation>
{
    [ActivatorUtilitiesConstructor]
    public StartVmCreatingEventHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async Task HandleAsync(StartVmCreation @event)
    {
        Logger.LogInformation("Initialization Creation VM for Event: {event}, Customer: {cid}, VM: {vm} is started", @event.EventId, @event.CustomerId, @event.VmId);

        try
        {
            var vm = await VirtualMachineRepository.TryGetVmByIdAsync(@event.VmId)
                ?? throw new VmNotFoundException(@event.VmId, $"The Virtual Machine {@event.VmId} for Customer {@event.CustomerId} is not found");

            // do something
            //throw new OutboxServiceException(@event.EventId, @event.CustomerId, @event.VmId, "Test Exception");

            DoSomething(nameof(StartVmCreatingEventHandler));

            vm!.CreateNic();
            await CompleteEventAsync(@event.EventId);

            Logger.LogInformation("Initialization Creation VM process for Event: {event}, VM: {vm} is completed.", @event.EventId, @event.VmId);
        }
        catch (Exception ex)
        {
            await FailEventAsync(new OutboxErrorMessage
            {
                EventId = @event.EventId,
                VmId = @event.VmId,
                CustomerId = @event.CustomerId,
                ErrorMessage = ex.Message,
                ErrorType = ex.GetType()
            });

            Logger.LogError("Received Exception {type}: {msg}", ex.GetType(), ex.Message);
        }
        finally
        {
            await VirtualMachineRepository.UnitOfWork.SaveAsync();
        }
    }
}
