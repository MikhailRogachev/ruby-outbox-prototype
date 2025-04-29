namespace ruby_outbox_core.Contracts.Options;

public class OutboxOptions
{
    /// <summary>
    /// Get; Set; outbox message request interval (mSec)
    /// </summary>
    public int ScanIntervalMs { get; set; }

    public int ExceptionsAllowedBeforeBreak { get; set; }

    public int DurationOfBreakSec { get; set; }

    public int RepeatLimit { get; set; }
}
