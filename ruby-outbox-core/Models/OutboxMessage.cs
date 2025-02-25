using ruby_outbox_core.Contracts.Enums;
using System.Text.Json;

namespace ruby_outbox_core.Models;

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
}
