using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Options;
using ruby_outbox_core.Dto;
using ruby_outbox_core.Events.CreateVm;
using ruby_outbox_core.Exceptions;
using ruby_outbox_core.Models;

namespace ruby_outbox_infrastructure.Processes;

public class CreateVmProcess(
        ILogger<CreateVmProcess> logger,
        IVmRepository vmRepository,
        IOutboxMessageRepository outboxRepository,
        IOutboxLoggerRepository loggerRepository,
        IOptions<OutboxOptions> options
    )
//:
//IEventHandler<CreateNic>,
//IEventHandler<CreateVmResource>,
//IEventHandler<CreateAadLoginExtension>,
//IEventHandler<CompleteCreateVmProcess>,
//IEventHandler<RunPowerShellCommand>
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

    /// <summary>
    /// 
    ///
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private async Task FailEventAsync(OutboxErrorMessage message)
    {
        var outboxMessage = await outboxRepository.GetMessageById(message.EventId!.Value);







    }

    /// <summary>
    /// This function returns <see cref="Vm">VM</see> selected by Id.
    /// In case the Vm is not found, the function generates an exception.
    /// </summary>
    /// <param name="vmId"></param>
    /// <returns></returns>
    private async Task<Vm?> GetVirtualMachineAsync(Guid vmId)
    {
        var vm = await vmRepository.TryGetVmByIdAsync(vmId);
        if (vm == null)
            throw new VmNotFoundException(vmId, $"The VM: {vmId} is not found.");

        return vm;
    }



    public async Task HandleAsync(CreateNic @event)
    {
        logger.LogInformation("Creation NIC process for Event: {event}, VM: {vm} is started", @event.EventId, @event.VmId);

        var vm = await GetVirtualMachineAsync(@event.VmId);
        if (vm == null)
            return;

        vm!.CreateVmResource();

        await CompleteEvent(@event.EventId);
        logger.LogInformation("Creation NIC process for Event: {event}, VM: {vm} is completed", @event.EventId, @event.VmId);
    }

    public async Task HandleAsync(CreateVmResource @event)
    {
        logger.LogInformation("Creation VM resource process for Event: {event}, VM: {vm} is started", @event.EventId, @event.VmId);

        var vm = await GetVirtualMachineAsync(@event.VmId);
        if (vm == null)
            return;

        vm.CreateAadLogin();

        await CompleteEvent(@event.EventId);
        logger.LogInformation("Creation VM resource process for Event: {event}, VM: {vm} is completed", @event.EventId, @event.VmId);
    }

    public async Task HandleAsync(CreateAadLoginExtension @event)
    {
        logger.LogInformation("Creation Aad Login run process for Event: {event}, VM: {vm} is started", @event.EventId, @event.VmId);

        var vm = await GetVirtualMachineAsync(@event.VmId);
        if (vm == null)
            return;

        vm.RunPowershellCommand();

        await CompleteEvent(@event.EventId);
        logger.LogInformation("Creation Aad Login run process for Event: {event}, VM: {vm} is completed", @event.EventId, @event.VmId);
    }

    public async Task HandleAsync(RunPowerShellCommand @event)
    {
        logger.LogInformation("Run powershell command process for Event: {event}, VM: {vm} is started", @event.EventId, @event.VmId);

        var vm = await GetVirtualMachineAsync(@event.VmId);
        if (vm == null)
            return;

        vm.CompleteVmCreation();

        await CompleteEvent(@event.EventId);
        logger.LogInformation("Run powershell command process for Event: {event}, VM: {vm} is completed", @event.EventId, @event.VmId);
    }

    public async Task HandleAsync(CompleteCreateVmProcess @event)
    {
        logger.LogInformation("Complete VM creation process for Event: {event}, VM: {vm} is started", @event.EventId, @event.VmId);

        var vm = await GetVirtualMachineAsync(@event.VmId);
        if (vm == null)
            return;

        // TODO Send VM ready integration process

        await CompleteEvent(@event.EventId);
        logger.LogInformation("Complete VM creation process for Event: {event}, VM: {vm} is completed", @event.EventId, @event.VmId);
    }


}
