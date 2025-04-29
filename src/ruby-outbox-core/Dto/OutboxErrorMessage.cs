using System.Text.Json.Serialization;

namespace ruby_outbox_core.Dto;

public class OutboxErrorMessage
{
    public Guid? VmId { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? EventId { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

    [JsonIgnore]
    public Type ErrorType { get; set; }
}