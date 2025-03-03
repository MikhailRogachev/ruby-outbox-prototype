using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Events.CreateVm;

namespace ruby_outbox_infrastructure.Processes;

public class CreateVmProcess(
    ILogger<CreateVmProcess> logger,
    IVmRepository vmRepository,
    IOutboxMessageRepository outboxRepository
    ) :
    IEventHandler<StartVmCreation>,
    IEventHandler<CreateNic>,
    IEventHandler<CreateVmResource>,
    IEventHandler<CreateAadLoginExtension>,
    IEventHandler<CompleteCreateVmProcess>,
    IEventHandler<RunPowerShellCommand>
{

    /// <summary>
    /// This procedure complete the transaction
    /// - save the Vm status updated;
    /// - remove event record from the outbox table
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    private async Task CompleteEvent(Guid eventId)
    {
        var outboxMessage = await outboxRepository.GetMessageById(@eventId);
        outboxRepository.Remove(outboxMessage!);

        await vmRepository.UnitOfWork.SaveAsync();

        logger.LogInformation("The process for the Event {id} is completed", eventId);
    }

    public async Task HandleAsync(StartVmCreation @event)
    {
        logger.LogInformation("Initialization Creation VM for Event: {event}, VM: {vm} is started", @event.EventId, @event.VmId);

        var vm = await vmRepository.TryGetVmByIdAsync(@event.VmId);
        if (vm == null)
        {
            logger.LogInformation("The VM: {vmId} is not found.", @event.VmId);
            return;
        }

        vm!.CreateNic();

        await CompleteEvent(@event.EventId);
        logger.LogInformation("Initialization Creation VM process for Event: {event}, VM: {vm} is completed.", @event.EventId, @event.VmId);
    }

    public async Task HandleAsync(CreateNic @event)
    {
        logger.LogInformation("Creation NIC process for Event: {event}, VM: {vm} is started", @event.EventId, @event.VmId);

        var vm = await vmRepository.TryGetVmByIdAsync(@event.VmId);
        if (vm == null)
        {
            logger.LogInformation("The VM: {vmId} is not found.", @event.VmId);
            return;
        }

        vm!.CreateVmResource();

        await CompleteEvent(@event.EventId);
        logger.LogInformation("Creation NIC process for Event: {event}, VM: {vm} is completed", @event.EventId, @event.VmId);
    }

    public async Task HandleAsync(CreateVmResource @event)
    {
        logger.LogInformation("Creation VM resource process for Event: {event}, VM: {vm} is started", @event.EventId, @event.VmId);

        var vm = await vmRepository.TryGetVmByIdAsync(@event.VmId);
        if (vm == null)
        {
            logger.LogInformation("The VM: {vmId} is not found.", @event.VmId);
            return;
        }

        await CompleteEvent(@event.EventId);
        logger.LogInformation("Creation VM resource process for Event: {event}, VM: {vm} is completed", @event.EventId, @event.VmId);
    }

    public Task HandleAsync(CreateAadLoginExtension @event)
    {
        throw new NotImplementedException();
    }

    public Task HandleAsync(CompleteCreateVmProcess @event)
    {
        throw new NotImplementedException();
    }

    public Task HandleAsync(RunPowerShellCommand @event)
    {
        throw new NotImplementedException();
    }
}
