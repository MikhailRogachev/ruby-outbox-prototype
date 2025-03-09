using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Options;
using ruby_outbox_core.Events.CreateVm;

namespace ruby_outbox_infrastructure.EventHandlers.CreateVm;

public class CreateAadLoginEventHandler : BaseEventHandler, IEventHandler<CreateAadLoginExtension>
{
    private readonly IVmRepository _vmRepository;
    private readonly ILogger<CreateAadLoginEventHandler> _logger;

    public CreateAadLoginEventHandler(
        ILogger<CreateAadLoginEventHandler> logger,
        IVmRepository vmRepository,
        IOutboxMessageRepository outboxRepository,
        IOutboxLoggerRepository loggerRepository,
        IOptions<OutboxOptions> options) : base(outboxRepository, loggerRepository, options)
    {
        _vmRepository = vmRepository;
        _logger = logger;
    }

    public async Task HandleAsync(CreateAadLoginExtension @event)
    {
        _logger.LogInformation("Creation Aad Login run process for Event: {event}, VM: {vm} is started", @event.EventId, @event.VmId);

        var vm = await _vmRepository.TryGetVmByIdAsync(@event.VmId);
        if (vm == null)
            return;

        vm.RunPowershellCommand();

        await CompleteEventAsync(@event.EventId);
        _logger.LogInformation("Creation Aad Login run process for Event: {event}, VM: {vm} is completed", @event.EventId, @event.VmId);
    }
}
