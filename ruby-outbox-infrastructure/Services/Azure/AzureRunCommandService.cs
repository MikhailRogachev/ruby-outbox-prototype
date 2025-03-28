using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Dto;

namespace ruby_outbox_infrastructure.Services.Azure
{
    public class AzureRunCommandService : AzureCommandService, IAzureScriptRunner
    {
        public AzureRunCommandService(ISecretManager secretManager) : base(secretManager)
        {
        }

        public Task<RunCommandResponseDto> RunInlineCommandAsync(Dictionary<string, string> @params)
        {
            throw new NotImplementedException();
        }
    }
}
