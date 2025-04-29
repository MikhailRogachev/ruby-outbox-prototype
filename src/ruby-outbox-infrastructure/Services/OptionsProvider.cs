using Microsoft.Extensions.Options;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Options;

namespace ruby_outbox_infrastructure.Services;

public class OptionsProvider(
    IOptions<OutboxOptions> outboxOptions,
    IOptions<AzureKeyVaultClientConfig> azureKeyVault) : IOptionsProvider
{
    public OutboxOptions OutboxOptions => outboxOptions.Value;

    public AzureKeyVaultClientConfig AzureKeyVaultClientConfig => azureKeyVault.Value;
}
