using Microsoft.Extensions.Options;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Options;
using ruby_outbox_core.Dto;
using ruby_outbox_core.Exceptions;
using ruby_outbox_core.Models;
using System.Text.Json;

namespace ruby_outbox_infrastructure.EventHandlers;

public class BaseEventHandler
{
    private IOutboxMessageRepository outboxRepository;
    private IOutboxLoggerRepository loggerRepository;
    private IOptions<OutboxOptions> options;

    public BaseEventHandler(
        IOutboxMessageRepository outboxRepository,
        IOutboxLoggerRepository loggerRepository,
        IOptions<OutboxOptions> options
        )
    {
        this.outboxRepository = outboxRepository;
        this.loggerRepository = loggerRepository;
        this.options = options;
    }

    protected async Task CompleteEventAsync(Guid eventId)
    {
        var outboxMessage = await outboxRepository.GetMessageById(@eventId);
        outboxRepository.Remove(outboxMessage!);
    }

    protected async Task FailEventAsync(OutboxErrorMessage message)
    {
        if (message.ErrorType == typeof(VmNotFoundException))
            await VmNotFound_FailEventAsync(message);
        else if (message.ErrorType == typeof(OutboxServiceException))
            await OutboxOperational_FailEventAsync(message);
    }

    private async Task OutboxOperational_FailEventAsync(OutboxErrorMessage message)
    {
        var outboxMessage = await outboxRepository.GetMessageById(message.EventId!.Value);

        if (outboxMessage!.Index == options.Value.RepeatLimit)
        {
            outboxRepository.Remove(outboxMessage!);

            // add to critical logger
            var logMessage = GetErrorLoggerMessage(message);
            loggerRepository.CreateRecord(logMessage);
        }

        outboxMessage.Repeat();
        outboxRepository.Update(outboxMessage);
    }

    /// <summary>
    /// The VM is not found and it mean, the transaction is closed. 
    /// However, we need to mention it in critical logger
    /// 
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private async Task VmNotFound_FailEventAsync(OutboxErrorMessage message)
    {
        var outboxMessage = await outboxRepository.GetMessageById(message.EventId!.Value);
        outboxRepository.Remove(outboxMessage!);

        // add to critical logger
        var logMessage = GetErrorLoggerMessage(message);
        loggerRepository.CreateRecord(logMessage);
    }

    private OutboxErrorLogger GetErrorLoggerMessage(OutboxErrorMessage message)
    {
        return new OutboxErrorLogger
        {
            CustomerId = message.CustomerId!.Value,
            UpdatedAt = DateTime.UtcNow,
            Comment = JsonSerializer.Serialize<OutboxErrorMessage>(message)
        };
    }
}
