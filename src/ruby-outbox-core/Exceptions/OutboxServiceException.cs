namespace ruby_outbox_core.Exceptions;

public class OutboxServiceException : Exception
{
    public Guid? EventId { get; init; }
    public Guid? CustomerId { get; init; }
    public Guid? VmId { get; init; }


    public OutboxServiceException(Guid eventId, Guid customerId, Guid vmId, string message) : base(message)
    {
        EventId = eventId;
        CustomerId = customerId;
        VmId = vmId;
    }

    public OutboxServiceException(Guid vmId, string message) : base(message)
    {
        VmId = vmId;
    }
}
