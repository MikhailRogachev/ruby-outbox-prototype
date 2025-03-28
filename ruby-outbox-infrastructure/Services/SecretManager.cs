using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Contracts.Options;
using ruby_outbox_core.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ruby_outbox_infrastructure.Services;

//{
//  "MySecret": "azure-subscription-6c3b57d1-74cc-41c5-a6a3-05be9db0bcdf",
//  "KeyVaultConfig:KeyVaultName": "pvad-dev-weu-app-kv",
//  "AzureConfig:TenantId": "9993c174-ed13-464f-8384-2e28f515ff6a",
//  "AzureConfig:ClientSecret": "1hP8Q~uApXi7xaHj-7m6XvTbP9eetVA_TpBE8bCY",
//  "AzureConfig:ClientId": "ccd16d25-d15e-4308-a7a5-5faa68f01010",
//  "MyRegion": "westeurope",
//  "ResourceGroup": "mikhail-rogachev-vms-rg"
//}
public class SecretManager : ISecretManager
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public SecretClient Client { get; init; }

    public SecretManager(AzureKeyVaultClientConfig azureConfig)
    {
        var keyVaultUri = new Uri($"https://{azureConfig.KeyVaultName}.vault.azure.net");
        var credentials = new ClientSecretCredential(azureConfig.TenantId, azureConfig.ClientId, azureConfig.ClientSecret);

        Client = new SecretClient(keyVaultUri, credentials);
    }

    public async Task<KeyVaultSecretValue> GetSecretValueAsync(string secretName)
    {
        var secret = await Client.GetSecretAsync(secretName);
        var secretValueJson = secret.Value.Value;
        var secretValue = JsonSerializer
            .Deserialize<KeyVaultSecretValue>(secretValueJson, _jsonSerializerOptions);

        if (secretValue == null)
            throw new Exception("Null value received while trying to deserialize secret value");

        return secretValue;
    }
}
