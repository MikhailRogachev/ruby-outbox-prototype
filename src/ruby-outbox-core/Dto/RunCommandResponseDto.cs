namespace ruby_outbox_core.Dto;

/// <summary>
///     Represents the response from executing a command, including the exit code and 
///     any associated message.
/// </summary>
/// <remarks>
///     This class is typically used to encapsulate the result of a command execution, 
///     providing both the exit code and a descriptive message. The <see cref="ExitCode"/> 
///     property indicates the success or  failure of the command, while the <see cref="Message"/> 
///     property provides additional context or  details about the execution.
/// </remarks>
public class RunCommandResponseDto
{
    public int? ExitCode { get; set; }
    public string Message { get; set; } = string.Empty;
}
