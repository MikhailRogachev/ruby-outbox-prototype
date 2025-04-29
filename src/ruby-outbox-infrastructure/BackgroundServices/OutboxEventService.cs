using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Contracts.Options;

namespace PVAD.Vms.Infrastructure.Messaging.OutboxEvent;

public class OutboxEventService(
    ILogger<OutboxEventService> logger,
    IServiceProvider serviceProvider,
    IOptions<OutboxOptions> options
    ) : BackgroundService
{

    private int RequestTimeout => options.Value.ScanIntervalMs;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{time} The Outbox Service is starting.", DateTime.UtcNow);

        stoppingToken.Register(() => logger.LogInformation("{time} Cancelling the Outbox Service due to host shutdown", DateTime.UtcNow));

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
