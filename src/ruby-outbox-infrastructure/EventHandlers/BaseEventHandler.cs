using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Options;
using ruby_outbox_core.Dto;
using ruby_outbox_core.Exceptions;
using ruby_outbox_core.Models;
using ruby_outbox_data.Persistency;
using ruby_outbox_data.Repositories;
using System.Text.Json;

namespace ruby_outbox_infrastructure.EventHandlers;

public class BaseEventHandler
{
    private readonly ApplicationDbContext _context;
    private readonly IVmRepository _vmRepository;
    private readonly IOutboxMessageRepository _outboxRepository;
    private readonly IOutboxLoggerRepository _loggerRepository;
    private readonly OutboxOptions _options;
    private readonly ILogger<BaseEventHandler> _logger;

    private readonly Random randomGen = new Random();

    protected IVmRepository VirtualMachineRepository => _vmRepository;
    protected ILogger<BaseEventHandler> Logger => _logger;

    [ActivatorUtilitiesConstructor]
    public BaseEventHandler(IServiceProvider serviceProvider)
    {
        // new context instance initialization
        var contextScoped = serviceProvider.GetRequiredService<ApplicationDbContext>() ?? throw new ArgumentNullException(nameof(ApplicationDbContext));
        _context = new ApplicationDbContext(contextScoped.DbOptions);

        // outbox options
        _options = serviceProvider.GetRequiredService<IOptionsProvider>().OutboxOptions;

        // create repositories
        _vmRepository = new VmRepository(_context);
        _outboxRepository = new OutboxRepository(_context);
        _loggerRepository = new OutboxLoggerRepository(_context);

        // retrieve logger
        _logger = serviceProvider.GetRequiredService<ILogger<BaseEventHandler>>();
        //_logger = serviceProvider.GetService
    }

    protected bool DoSomething(string name)
    {
        int mSec = randomGen.Next(1000, 10000);

        _logger.LogDebug("Start {name} process for {msec} mS", name, mSec);

        Thread.Sleep(mSec);

        _logger.LogDebug("The process {name} is completed", name);
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

        if (outboxMessage!.Index == _options.RepeatLimit)
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
