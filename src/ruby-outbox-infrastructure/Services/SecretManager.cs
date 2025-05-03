using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Options;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Contracts.Options;
using ruby_outbox_core.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ruby_outbox_infrastructure.Services;

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

    public SecretManager(IOptions<AzureKeyVaultClientConfig> azureConfig)
    {
        var keyVaultUri = new Uri($"https://{azureConfig.Value.KeyVaultName}.vault.azure.net");
        var credentials = new DefaultAzureCredential();

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
