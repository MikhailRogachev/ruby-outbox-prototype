using AutoMapper;
using Microsoft.Extensions.Options;
using ruby_outbox_core.AzureRequests;
using ruby_outbox_core.Contracts.Options;
using ruby_test_core.Attributes;
using ruby_test_core.Helpers;

namespace ruby_test_unit.Profiles;

public class AzureInlineCommandRequestMapping
{
    public static IOptions<AzureKeyVaultClientConfig> GetAzureConfigOptions() =>
        Options.Create(TestHelper.GetOptionsFromAppSettings<AzureKeyVaultClientConfig>());
    public static IMapper Mapper() => TestHelper.Mapper();

    [Theory, AutoMock]
    public void RequestMapping(
        [RegInstance(nameof(GetAzureConfigOptions))] IOptions<AzureKeyVaultClientConfig> options,
        [RegInstance(nameof(Mapper))] IMapper mapper
        )
    {
        // act
        var request = mapper.Map<AzureInlineCommandRequest>(options.Value);

        // assert
        Assert.True(request.TenantId == options.Value.TenantId);
        Assert.True(request.ClientSecret == options.Value.ClientSecret);
        Assert.True(request.KeyVaultName == options.Value.KeyVaultName);
        Assert.True(request.ClientId == options.Value.ClientId);
    }
}
