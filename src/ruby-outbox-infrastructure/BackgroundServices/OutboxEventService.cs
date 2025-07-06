using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Contracts.Options;

namespace ruby_outbox_infrastructure.BackgroundServices;

/// <summary>
///     A background service that processes outbox events by periodically invoking an event publisher.
/// </summary>
/// <remarks>
///     The <see cref="OutboxEventService"/> is designed to run as a hosted service, scanning for outbox
///     events at regular intervals and publishing them using an <see cref="IOutboxEventPublisher"/> 
///     implementation. The service uses dependency injection to resolve required services and options.
/// </remarks>
/// <param name="logger"></param>
/// <param name="serviceProvider"></param>
/// <param name="options"></param>
public class OutboxEventService(
    ILogger<OutboxEventService> logger,
    IServiceProvider serviceProvider,
    IOptions<OutboxOptions> options
    ) : BackgroundService
{

    private int RequestTimeout => options.Value.ScanIntervalMs;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogDebug("{time} The Outbox Service is starting.", DateTime.UtcNow);

        stoppingToken.Register(() => logger.LogDebug("{time} Cancelling the Outbox Service due to host shutdown", DateTime.UtcNow));

        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            IOutboxEventPublisher publisher = scope.ServiceProvider.GetService<IOutboxEventPublisher>()!;

            while (!stoppingToken.IsCancellationRequested)
            {
                await publisher.RunAsync();
                await Task.Delay(RequestTimeout, stoppingToken);
            }
        }
    }
}
