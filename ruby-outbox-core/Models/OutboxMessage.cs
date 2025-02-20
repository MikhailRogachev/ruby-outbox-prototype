using System.Text.Json;

namespace ruby_outbox_core.Models;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public JsonDocument? Content { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
}
