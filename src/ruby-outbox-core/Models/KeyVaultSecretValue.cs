namespace ruby_outbox_core.Models;

/// <summary>
///     Represents the credentials and identifiers required to access a Key Vault.
/// </summary>
/// <remarks>
///     This class encapsulates the necessary information for authenticating and 
///     interacting with a KeyVault, including tenant, subscription, and application 
///     identifiers, as well as the client secret.
/// </remarks>
public class KeyVaultSecretValue
{
    public Guid TenantId { get; set; }
    public Guid SubscriptionId { get; set; }
    public Guid ApplicationId { get; set; }
    public string ClientSecret { get; set; } = string.Empty;
}
