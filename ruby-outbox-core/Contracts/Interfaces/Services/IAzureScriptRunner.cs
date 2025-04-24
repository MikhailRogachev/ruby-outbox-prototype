using ruby_outbox_core.AzureRequests;
using ruby_outbox_core.Dto;

namespace ruby_outbox_core.Contracts.Interfaces.Services;

public interface IAzureScriptRunner
{
    Task<RunCommandResponseDto> RunInlineCommandAsync(AzureInlineCommandRequest request);
}
