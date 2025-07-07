namespace ruby_outbox_core.Dto;

/// <summary>
///     Represents a data transfer object for customer information.
/// </summary>
/// <remarks>
///     This class is typically used to transfer customer data between different 
///     layers of an application. It contains basic customer identification details.
/// </remarks>
public class CustomerDto
{
    public Guid CustomerId { get; set; }
}
