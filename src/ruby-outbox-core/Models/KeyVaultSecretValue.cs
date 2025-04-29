namespace ruby_outbox_core.Models;

public class KeyVaultSecretValue
{
    public Guid TenantId { get; set; }
    public Guid SubscriptionId { get; set; }
    public Guid ApplicationId { get; set; }
    public string ClientSecret { get; set; } = string.Empty;
}
