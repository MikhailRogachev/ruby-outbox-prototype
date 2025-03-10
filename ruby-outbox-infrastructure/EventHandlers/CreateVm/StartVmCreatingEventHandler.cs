using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Options;
using ruby_outbox_core.Dto;
using ruby_outbox_core.Events.CreateVm;
using ruby_outbox_core.Exceptions;

namespace ruby_outbox_infrastructure.EventHandlers.CreateVm;

public class StartVmCreatingEventHandler : BaseEventHandler, IEventHandler<StartVmCreation>
{
    private readonly IVmRepository _vmRepository;

    public StartVmCreatingEventHandler(
            ILogger<StartVmCreatingEventHandler> logger,
            IVmRepository vmRepository,
            IOutboxMessageRepository outboxRepository,
            IOutboxLoggerRepository loggerRepository,
            IOptions<OutboxOptions> options) : base(logger, outboxRepository, loggerRepository, options)
    {
        _vmRepository = vmRepository;
    }

    public async Task HandleAsync(StartVmCreation @event)
    {
        _logger.LogInformation("Initialization Creation VM for Event: {event}, Customer: {cid}, VM: {vm} is started", @event.EventId, @event.CustomerId, @event.VmId);

        try
        {
            var vm = await _vmRepository.TryGetVmByIdAsync(@event.VmId)
                ?? throw new VmNotFoundException(@event.VmId, $"The Virtual Machine {@event.VmId} for Customer {@event.CustomerId} is not found");

            // do something
            //throw new OutboxServiceException(@event.EventId, @event.CustomerId, @event.VmId, "Test Exception");

            DoSomething(nameof(StartVmCreatingEventHandler));

            vm!.CreateNic();
            await CompleteEventAsync(@event.EventId);

            _logger.LogInformation("Initialization Creation VM process for Event: {event}, VM: {vm} is completed.", @event.EventId, @event.VmId);
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

            _logger.LogError("Received Exception {type}: {msg}", ex.GetType(), ex.Message);
        }
        finally
        {
            await _vmRepository.UnitOfWork.SaveAsync();
        }
    }
}
