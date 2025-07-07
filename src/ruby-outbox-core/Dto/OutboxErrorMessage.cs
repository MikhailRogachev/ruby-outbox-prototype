using System.Text.Json.Serialization;

namespace ruby_outbox_core.Dto;

/// <summary>
///     Represents an error message associated with an outbox operation, including details 
///     about the  virtual machine, customer, event, and error type.
/// </summary>
/// <remarks>
///     This class is typically used to encapsulate error information for logging, debugging, 
///     or  handling failed operations in an outbox pattern. The properties provide contextual 
///     details  about the error, such as the related virtual machine, customer, and event identifiers.
/// </remarks>
public class OutboxErrorMessage
{
    public Guid? VmId { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? EventId { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

    [JsonIgnore]
    public Type ErrorType { get; set; }
}