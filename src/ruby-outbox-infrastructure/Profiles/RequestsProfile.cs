using AutoMapper;
using ruby_outbox_core.AzureRequests;
using ruby_outbox_core.Contracts.Options;

namespace ruby_outbox_infrastructure.Profiles;

public class RequestsProfile : Profile
{
    public RequestsProfile()
    {
        CreateMap<AzureKeyVaultClientConfig, AzureInlineCommandRequest>();
    }
}
