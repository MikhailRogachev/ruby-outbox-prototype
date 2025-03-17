using Microsoft.Extensions.DependencyInjection;
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

    public CreateAadLoginEventHandler(
        ILogger<CreateAadLoginEventHandler> logger,
        IVmRepository vmRepository,
        IOutboxMessageRepository outboxRepository,
        IOutboxLoggerRepository loggerRepository,
        IOptions<OutboxOptions> options) : base(logger, outboxRepository, loggerRepository, options)
    {
        _vmRepository = vmRepository;
    }

    [ActivatorUtilitiesConstructor]
    public CreateAadLoginEventHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _vmRepository = serviceProvider.GetRequiredService<IVmRepository>();
    }

    public async Task HandleAsync(CreateAadLoginExtension @event)
    {
        _logger.LogInformation("Creation Aad Login run process for Event: {event}, VM: {vm} is started", @event.EventId, @event.VmId);

        var vm = await _vmRepository.TryGetVmByIdAsync(@event.VmId);
        if (vm == null)
            return;

        DoSomething(nameof(CreateAadLoginEventHandler));

        vm.RunPowershellCommand();

        await CompleteEventAsync(@event.EventId);

        await _vmRepository.UnitOfWork.SaveAsync();

        _logger.LogInformation("Creation Aad Login run process for Event: {event}, VM: {vm} is completed", @event.EventId, @event.VmId);
    }
}
