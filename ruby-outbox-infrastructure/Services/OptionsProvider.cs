using Microsoft.Extensions.Options;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Options;

namespace ruby_outbox_infrastructure.Services;

public class OptionsProvider(IOptions<OutboxOptions> outboxOptions) : IOptionsProvider
{
    public OutboxOptions OutboxOptions => outboxOptions.Value;
}
