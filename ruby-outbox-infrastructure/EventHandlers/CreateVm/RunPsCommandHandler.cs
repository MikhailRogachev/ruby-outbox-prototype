using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Options;
using ruby_outbox_core.Events.CreateVm;

namespace ruby_outbox_infrastructure.EventHandlers.CreateVm;

public class RunPsCommandHandler : BaseEventHandler, IEventHandler<RunPowerShellCommand>
{
    private readonly IVmRepository _vmRepository;

    public RunPsCommandHandler(
        ILogger<RunPsCommandHandler> logger,
        IVmRepository vmRepository,
        IOutboxMessageRepository outboxRepository,
        IOutboxLoggerRepository loggerRepository,
        IOptions<OutboxOptions> options) : base(logger, outboxRepository, loggerRepository, options)
    {
        _vmRepository = vmRepository;
    }

    public async Task HandleAsync(RunPowerShellCommand @event)
    {
        _logger.LogInformation("Run powershell command process for Event: {event}, VM: {vm} is started", @event.EventId, @event.VmId);

        var vm = await _vmRepository.TryGetVmByIdAsync(@event.VmId);
        if (vm == null)
            return;

        DoSomething(nameof(RunPsCommandHandler));

        vm.CompleteVmCreation();

        await CompleteEventAsync(@event.EventId);

        await _vmRepository.UnitOfWork.SaveAsync();

        _logger.LogInformation("Run powershell command process for Event: {event}, VM: {vm} is completed", @event.EventId, @event.VmId);
    }
}
