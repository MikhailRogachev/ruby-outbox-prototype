namespace ruby_outbox_core.Contracts.Options;

public class AzureKeyVaultClientConfig
{
    public string TenantId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string KeyVaultName { get; set; } = string.Empty;
}