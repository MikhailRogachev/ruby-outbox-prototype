using Microsoft.Extensions.Logging;
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
    private IOutboxMessageRepository _outboxRepository;
    private IOutboxLoggerRepository _loggerRepository;
    private IOptions<OutboxOptions> _options;
    private Random randomGen = new Random();

    protected readonly ILogger<BaseEventHandler> _logger;

    public BaseEventHandler(
        ILogger<BaseEventHandler> logger,
        IOutboxMessageRepository outboxRepository,
        IOutboxLoggerRepository loggerRepository,
        IOptions<OutboxOptions> options
        )
    {
        _logger = logger;
        _outboxRepository = outboxRepository;
        _loggerRepository = loggerRepository;
        _options = options;
    }

    protected bool DoSomething(string name)
    {
        int mSec = randomGen.Next(1000, 10000);

        _logger.LogInformation("Start {name} process for {msec} mS", name, mSec);

        Thread.Sleep(mSec);

        _logger.LogInformation("The process {name} is completed", name);
        return true;
    }

    protected async Task CompleteEventAsync(Guid eventId)
    {
        var outboxMessage = await _outboxRepository.GetMessageById(@eventId);
        _outboxRepository.Remove(outboxMessage!);
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
        var outboxMessage = await _outboxRepository.GetMessageById(message.EventId!.Value);

        if (outboxMessage!.Index == _options.Value.RepeatLimit)
        {
            _outboxRepository.Remove(outboxMessage!);

            // add to critical logger
            var logMessage = GetErrorLoggerMessage(message);
            _loggerRepository.CreateRecord(logMessage);
        }

        outboxMessage.Repeat();
        _outboxRepository.Update(outboxMessage);
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
        var outboxMessage = await _outboxRepository.GetMessageById(message.EventId!.Value);
        _outboxRepository.Remove(outboxMessage!);

        // add to critical logger
        var logMessage = GetErrorLoggerMessage(message);
        _loggerRepository.CreateRecord(logMessage);
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
