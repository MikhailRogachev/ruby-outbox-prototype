using ruby_outbox_core.AzureRequests;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Dto;

namespace ruby_outbox_infrastructure.Services.Azure
{
    public class AzureRunCommandService : AzureCommandService, IAzureScriptRunner
    {
        public AzureRunCommandService(ISecretManager secretManager) : base(secretManager)
        {
        }

        public Task<RunCommandResponseDto> RunInlineCommandAsync(AzureInlineCommandRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
