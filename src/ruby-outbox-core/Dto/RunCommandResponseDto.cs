namespace ruby_outbox_core.Dto;

public class RunCommandResponseDto
{
    public int? ExitCode { get; set; }
    public string Message { get; set; } = string.Empty;
}
