using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Options;
using ruby_outbox_core.Events.CreateVm;

namespace ruby_outbox_infrastructure.EventHandlers.CreateVm;

public class CompleteVmCreateEventHandler : BaseEventHandler, IEventHandler<CompleteCreateVmProcess>
{
    private readonly IVmRepository _vmRepository;
    private readonly ILogger<CompleteVmCreateEventHandler> _logger;

    public CompleteVmCreateEventHandler(
        ILogger<CompleteVmCreateEventHandler> logger,
        IVmRepository vmRepository,
        IOutboxMessageRepository outboxRepository,
        IOutboxLoggerRepository loggerRepository,
        IOptions<OutboxOptions> options) : base(outboxRepository, loggerRepository, options)
    {
        _vmRepository = vmRepository;
        _logger = logger;
    }

    public async Task HandleAsync(CompleteCreateVmProcess @event)
    {
        _logger.LogInformation("Complete VM creation process for Event: {event}, VM: {vm} is started", @event.EventId, @event.VmId);

        var vm = await _vmRepository.TryGetVmByIdAsync(@event.VmId);
        if (vm == null)
            return;

        // TODO Send VM ready integration process

        await CompleteEventAsync(@event.EventId);
        _logger.LogInformation("Complete VM creation process for Event: {event}, VM: {vm} is completed", @event.EventId, @event.VmId);
    }
}
