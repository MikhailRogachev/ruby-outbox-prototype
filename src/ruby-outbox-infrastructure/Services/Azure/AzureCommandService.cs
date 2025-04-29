using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Models;

namespace ruby_outbox_infrastructure.Services.Azure;

public class AzureCommandService
{
    protected ISecretManager SecretManager { get; init; }

    public AzureCommandService(ISecretManager secretManager)
    {
        SecretManager = secretManager ?? throw new ArgumentNullException(nameof(secretManager));
    }

    protected ArmClient GetArmClientSubscriptionAsync(KeyVaultSecretValue secretValue)
    {
        var tokenCredentials = new ClientSecretCredential(
            secretValue.TenantId.ToString(),
            secretValue.ApplicationId.ToString(),
            secretValue.ClientSecret
            );

        return new ArmClient(tokenCredentials, secretValue.SubscriptionId.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="virtualMachineName">VM string name (where run script)</param>
    /// <param name="customerKeyValutSecret">Customer KeyVaultSecret (from DB)</param>
    /// <returns></returns>
    protected async Task<VirtualMachineResource> GetVirtualMachineResourceAsync(string virtualMachineName, string customerKeyValutSecret, string resourceGroup)
    {
        var secretValue = await SecretManager.GetSecretValueAsync(customerKeyValutSecret);
        var client = GetArmClientSubscriptionAsync(secretValue);

        ResourceIdentifier vmResourceId = VirtualMachineResource.CreateResourceIdentifier(secretValue.SubscriptionId.ToString(), resourceGroup, virtualMachineName);
        return client.GetVirtualMachineResource(vmResourceId);
    }
}
