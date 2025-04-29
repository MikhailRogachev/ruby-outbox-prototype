using ruby_outbox_core.Contracts.Options;

namespace ruby_outbox_core.Contracts.Interfaces;

public interface IOptionsProvider
{
    OutboxOptions OutboxOptions { get; }
    AzureKeyVaultClientConfig AzureKeyVaultClientConfig { get; }
}
