using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Events;

namespace ruby_outbox_infrastructure.Processes;

public class CreateVmProcess(
    ILogger<CreateVmProcess> logger,
    IVmRepository vmRepository,
    IOutboxMessageRepository outboxRepository
    ) :
    IEventHandler<StartVmCreation>,
    IEventHandler<CreateNic>
{
    public async Task HandleAsync(StartVmCreation @event)
    {
        logger.LogInformation("Starting Creation VM process for Event: {event}, VM: {vm}", @event.EventId, @event.VmId);

        var vm = await vmRepository.TryGetVmByIdAsync(@event.VmId);
        if (vm == null)
        {
            logger.LogInformation("The VM: {vmId} is not found.", @event.VmId);
            return;
        }



        vm!.CreateNic();

        var outboxMessage = await outboxRepository.GetMessageById(@event.EventId);

        await vmRepository.UnitOfWork.SaveAsync();
    }

    public Task HandleAsync(CreateNic @event)
    {
        throw new NotImplementedException();
    }
}
