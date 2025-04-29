using Azure.Security.KeyVault.Secrets;
using ruby_outbox_core.Models;

namespace ruby_outbox_core.Contracts.Interfaces.Services;

public interface ISecretManager
{
    SecretClient Client { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="secretName">This parameter is customer keyvault value</param>
    /// <returns></returns>
    Task<KeyVaultSecretValue> GetSecretValueAsync(string secretName);
}
