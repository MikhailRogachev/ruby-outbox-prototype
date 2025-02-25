using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Events;

namespace ruby_outbox_infrastructure.EventHandlers.CreateVm;

public class StartCreateVmHandler(
    ILogger<StartCreateVmHandler> logger,
    //IOutboxMessageRepository outboxRepository,
    IVmRepository vmRepository
    ) : IEventHandler<StartVmCreation>
{
    public async Task Handle(StartVmCreation @event)
    {
        logger.LogInformation("Started Event {ev}", nameof(StartCreateVmHandler));

        // get virtual machine
        var vm = await vmRepository.TryGetVmByIdAsync(@event.VmId);
    }
}
