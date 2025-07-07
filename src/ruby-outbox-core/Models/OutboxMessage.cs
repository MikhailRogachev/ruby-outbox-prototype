using ruby_outbox_core.Contracts.Enums;
using System.Text.Json;

namespace ruby_outbox_core.Models;

/// <summary>
///     Represents a message stored in an outbox for deferred processing or delivery.
/// </summary>
/// <remarks>
///     The <see cref="OutboxMessage"/> class is typically used in scenarios where messages 
///     need to be persisted temporarily before being processed or sent to their destination. 
///     It includes metadata  such as the message content, status, and timestamps for tracking 
///     and management purposes.
/// </remarks>
public class OutboxMessage
{
    public Guid Id { get; set; }
    public JsonDocument? Content { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public OutboxMessageStatus Status { get; set; }
    public int Index { get; set; }
    public string? Message { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModifiedDate { get; set; }

    public void Repeat()
    {
        Status = OutboxMessageStatus.Ini;
        Index++;
        LastModifiedDate = DateTime.UtcNow;
    }
}
