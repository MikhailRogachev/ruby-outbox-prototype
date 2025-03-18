using Microsoft.Extensions.Options;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Options;
using System.Configuration;

namespace ruby_outbox_infrastructure.Services;

public class OptionsProvider(ConfigurationManager configurationManager, IOptions<OutboxOptions> outboxOptions) : IOptionsProvider
{
    public OutboxOptions OutboxOptions => outboxOptions.Value;
}
