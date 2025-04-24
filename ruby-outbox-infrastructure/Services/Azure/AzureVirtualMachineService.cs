using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Contracts.Options;
using ruby_outbox_core.Dto;
using ruby_outbox_core.Models;

namespace ruby_outbox_infrastructure.Services.Azure;

public class AzureVirtualMachineService : IAzureVirtualMachineService
{
    private readonly string _vmsAdvancedTadKey = "PVAD_VM_ID";
    private readonly string _runningStatusCode = "PowerState";
    private readonly string _provosionStatusCode = "ProvisioningState";
    private ILogger<AzureVirtualMachineService> _logger;
    private readonly ISecretManager _secretManager;
    private readonly PersonalSettingsConfig _personalSettingsConfig;

    public AzureVirtualMachineService(
        ILogger<AzureVirtualMachineService> logger,
        IOptions<PersonalSettingsConfig> options,
        ISecretManager secretManager
        )
    {
        _logger = logger;
        _secretManager = secretManager;
        _personalSettingsConfig = options.Value;
    }

    public async Task<IList<AzureVirtualMachineDto>> GetVirtualMachinesAsync()
    {
        var data = new List<AzureVirtualMachineDto>();

        var secretValue = await _secretManager.GetSecretValueAsync(_personalSettingsConfig.PersonalSecret);
        var client = GetArmClientSubscriptionAsync(secretValue);
        var subscription = await client!.GetDefaultSubscriptionAsync();

        ResourceIdentifier resourceGroupResourceId = ResourceGroupResource.CreateResourceIdentifier(subscription.Id.SubscriptionId, _personalSettingsConfig.ResourceGroup);
        ResourceGroupResource resourceGroupResource = client.GetResourceGroupResource(resourceGroupResourceId);

        VirtualMachineCollection collection = resourceGroupResource.GetVirtualMachines();

        await foreach (VirtualMachineResource item in collection.GetAllAsync())
        {
            var view = item.InstanceView();

            data.Add(new AzureVirtualMachineDto
            {
                ComputerName = view.Value.ComputerName,
                Name = item.Data.Name,
                DateTimeOffset = item.Data.TimeCreated.GetValueOrDefault(),
                VmsAdvancedId = GetVmsAdvancedId(item.Data.Tags),
                RunningStatus = GetStatus(view.Value.Statuses, _runningStatusCode),
                ProvisionStatus = GetStatus(view.Value.Statuses, _provosionStatusCode)
            });
        }

        return data;
    }

    private ArmClient GetArmClientSubscriptionAsync(KeyVaultSecretValue secretValue)
    {
        try
        {
            var tokenCredentials = new ClientSecretCredential(
                secretValue.TenantId.ToString(),
                secretValue.ApplicationId.ToString(),
                secretValue.ClientSecret
                );

            return new ArmClient(tokenCredentials, secretValue.SubscriptionId.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Exception {name} - {ex}", nameof(GetArmClientSubscriptionAsync), ex.Message);
            throw;
        }
    }

    private Guid? GetVmsAdvancedId(IDictionary<string, string> tags)
    {
        if (tags.ContainsKey(_vmsAdvancedTadKey))
            return Guid.Parse(tags[_vmsAdvancedTadKey]);

        return null;
    }

    private string GetStatus(IReadOnlyList<InstanceViewStatus> listStatus, string code)
    {
        var status = listStatus.FirstOrDefault(p => p.Code.StartsWith(code));
        if (status != null)
            return status.DisplayStatus;

        return string.Empty;
    }
}
